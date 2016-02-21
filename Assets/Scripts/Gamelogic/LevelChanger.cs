using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prime31;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
	public GameObject playerPrefab = null;
	public float deathCamDuration = 1.0f;
    private Image fadePanel;

	private GameObject playerObject;

	public void Start()
	{
		if (SceneManager.sceneCount == 1)
		{
			int levelSceneIndex = gameObject.scene.buildIndex;

			SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
			SceneManager.LoadScene(levelSceneIndex, LoadSceneMode.Additive);
		}
		else
		{
            fadePanel = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Image>();
            RespawnPlayer(true);
            InvokeRepeating("FadeIn", 0f, 0.05f);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5) || Input.GetKeyDown(KeyCode.Joystick1Button6))
		{
			NextLevel();
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			RespawnPlayer();
		}
	    if (Input.GetAxis("ResetLevel") > 0)
	    {
            DestroyPlayer();
            SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
            SceneManager.LoadScene(gameObject.scene.buildIndex, LoadSceneMode.Additive);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

	void OnDestroy()
	{
		// Unregister trigger event
		UnregisterCallbacks();
		Destroy(playerObject);
	}

	private void OnPlayerDeath(GameObject go)
	{
		DestroyPlayer();
		StartCoroutine(RespawnPlayerCoroutine());
	}

	public void NextLevel()
	{
		int currentScene = gameObject.scene.buildIndex;
		int nextSceneIndex = currentScene + 1;
	    if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings - 1)
	    {
	        nextSceneIndex = 0;
	    }

	    // Load next level
		SceneManager.UnloadScene(currentScene);
		SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
		
	}

	private void DestroyPlayer()
	{
		if (playerObject != null)
		{
			Health playerHealth = playerObject.GetComponent<Health>();
			if (playerHealth && !playerHealth.IsMarkedForDeath)
			{
				Destroy(playerObject);
			}

			UnregisterCallbacks();
			playerObject = null;
		}
	}

	public void RespawnPlayer(bool warpCameraToSpawnPoint = false)
	{
		DestroyPlayer();
		
		// There is no player so we respawn him
		playerObject = Instantiate(playerPrefab);

		// Register for trigger event
		RegisterCallbacks();

		// Respawn player
		PlayerRespawn playerRespawn = playerObject.GetComponent<PlayerRespawn>();
		if (playerRespawn)
		{
			if (warpCameraToSpawnPoint)
			{
				ResetCameraToPosition(playerRespawn.transform.position);
				StartCoroutine(ResetCameraToSheepCoroutine());
			}

			playerRespawn.MoveToRespawn();
			playerRespawn.FadeIn();
		}
		else
		{
			Debug.Log("No respawn component found on player. Please add PlayerRespawn component to player");
		}
	}

    private void FadeIn()
    {
        Color color = fadePanel.color;
        if (color.a > 0)
        {
            color.a = Mathf.Max(0, color.a - 0.03f);
            fadePanel.color = color;
        }
        else
        {
            CancelInvoke("FadeIn");
        }
    }

    private void FadeOut()
    {
        Color color = fadePanel.color;
        if (color.a < 1)
        {
            color.a = Mathf.Min(1, color.a + 0.03f);
            fadePanel.color = color;
        }
        else
        {
            CancelInvoke("FadeOut");
            NextLevel();
        }
    }

	private void OnGoalEntered(Collider2D collider)
	{
		if (collider.gameObject == gameObject)
		{
			StartCoroutine(NextLevelCoroutine());
		}
	}

	private IEnumerator NextLevelCoroutine()
	{
		yield return new WaitForEndOfFrame();
        if (playerObject != null)
        {
            PlayerMover mover = playerObject.GetComponent<PlayerMover>();
            if (mover)
            {
                mover.CanMove = false;
            }
        }
        InvokeRepeating("FadeOut", 0f, 0.05f);
		yield return null;
	}

	private IEnumerator RespawnPlayerCoroutine()
	{
		yield return new WaitForSeconds(deathCamDuration);
		RespawnPlayer();
	}

	private IEnumerator ResetCameraToSheepCoroutine()
	{
		yield return new WaitForEndOfFrame();
		if (playerObject)
		{
			ResetCameraToPosition(playerObject.transform.position);
		}
	}

	private void ResetCameraToPosition(Vector3 newCameraPosition)
	{
		Vector3 oldCameraPosition = Camera.main.transform.position;
		Camera.main.transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, oldCameraPosition.z);
	}

	private void RegisterCallbacks()
	{
		if (playerObject != null)
		{
			CharacterController2D playerController = playerObject.GetComponent<CharacterController2D>();
			playerController.onTriggerEnterEvent += OnGoalEntered;

			Health.onDeath += OnPlayerDeath;
		}
	}

	private void UnregisterCallbacks()
	{
		if (playerObject != null)
		{
			CharacterController2D playerController = playerObject.GetComponent<CharacterController2D>();
			playerController.onTriggerEnterEvent -= OnGoalEntered;

			Health.onDeath -= OnPlayerDeath;
		}
	}
}
