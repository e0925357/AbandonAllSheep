using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prime31;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class LevelChanger : MonoBehaviour
{
	public delegate void NextLevelDelegate();
	public static event NextLevelDelegate nextLevelEvent;

	public GameObject playerPrefab = null;
	public float deathCamDuration = 1.0f;
	private Image fadePanel;
	private Text levelnameText;

	private GameObject playerObject;

	public void Start()
	{
		fadePanel = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Image>();
		levelnameText = GameObject.FindGameObjectWithTag("LevelnameText").GetComponent<Text>();
		RespawnPlayer(true);
		StartCoroutine(FadeIn());
		StartCoroutine(ShowAndHideLevelname(SceneManager.GetActiveScene().name));
	}

	IEnumerator ShowAndHideLevelname(string levelname)
	{
		if (levelname == null) yield break;

		levelnameText.text = levelname;
		Color startColor = levelnameText.color;
		startColor.a = 0.0f;
		levelnameText.color = startColor;

		while (levelnameText.color.a < 1.0f)
		{
			Color c = levelnameText.color;
			c.a += Time.deltaTime*2;

			if (c.a > 1.0f) c.a = 1.0f;

			levelnameText.color = c;
			yield return new WaitForEndOfFrame();
		}

		yield return new WaitForSeconds(1.5f);

		while (levelnameText.color.a > 0)
		{
			Color c = levelnameText.color;
			c.a -= Time.deltaTime;

			if (c.a < 0) c.a = 0;

			levelnameText.color = c;
			yield return new WaitForEndOfFrame();
		}
	}

	void Update()
	{
		if (CrossPlatformInputManager.GetButtonDown("SkipLevel"))
		{
			NextLevel();
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			RespawnPlayer();
		}
		if (CrossPlatformInputManager.GetButtonDown("ResetLevel"))
		{
			DestroyPlayer();
			SceneManager.LoadScene(gameObject.scene.buildIndex, LoadSceneMode.Single);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	void OnEnable()
	{
		Health.onDeath += OnPlayerDeath;
	}

	void OnDisable()
	{
		Health.onDeath -= OnPlayerDeath;

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
		if (nextLevelEvent != null)
		{
			nextLevelEvent();
		}

		int currentScene = gameObject.scene.buildIndex;
		int nextSceneIndex = currentScene + 1;
		if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings - 1)
		{
			nextSceneIndex = 0;
		}

		// Load next level
		SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
	}

	public void NextLevel(string levelname)
	{
		if (nextLevelEvent != null)
		{
			nextLevelEvent();
		}

		// Load next level
		SceneManager.LoadScene(levelname, LoadSceneMode.Single);

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
			playerObject = null;
		}

		UnregisterCallbacks();
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

	private IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(1.0f);

		while (fadePanel.color.a > 0)
		{
			Color color = fadePanel.color;
			color.a = Mathf.Max(0, color.a - Time.deltaTime);
			fadePanel.color = color;

			yield return new WaitForEndOfFrame();
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

	private void FadeOut(string levelname)
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
			NextLevel(levelname);
		}
	}

	private void OnGoalEntered(Collider2D collider)
	{
		if (collider.gameObject == gameObject)
		{
			StartCoroutine(NextLevelCoroutine());
		}
	}

	public void JumpToScene(string LevelName)
	{

		StartCoroutine(NextLevelCoroutine(LevelName));

	}

	private void setPlayerMoveable(bool moveable)
	{
		if (playerObject != null)
		{
			PlayerMover mover = playerObject.GetComponent<PlayerMover>();
			if (mover)
			{
				mover.CanMove = moveable;
			}
			else
			{
				PhysicsPlayerController ppc = playerObject.GetComponent<PhysicsPlayerController>();

				if(ppc)
				{
					ppc.freezeInput = !moveable;
				}
			}
		}
	}

	private IEnumerator NextLevelCoroutine()
	{
		yield return new WaitForEndOfFrame();
		setPlayerMoveable(false);
		InvokeRepeating("FadeOut", 0f, 0.05f);
		yield return null;
	}

	private IEnumerator NextLevelCoroutine(string levelname)
	{
		yield return new WaitForEndOfFrame();
		FadeOut(levelname);
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
			if (playerController != null)
			{
				playerController.onTriggerEnterEvent += OnGoalEntered;
			}
			else
			{
				PhysicsPlayerController ppc = playerObject.GetComponent<PhysicsPlayerController>();
				if (ppc != null)
				{
					ppc.onTriggerEnterEvent += OnGoalEntered;
				}
			}
		}
	}

	private void UnregisterCallbacks()
	{
		if (playerObject != null)
		{
			CharacterController2D playerController = playerObject.GetComponent<CharacterController2D>();
			if (playerController != null)
			{
				playerController.onTriggerEnterEvent -= OnGoalEntered;
			}
			else
			{
				PhysicsPlayerController ppc = playerObject.GetComponent<PhysicsPlayerController>();
				if (ppc != null)
				{
					ppc.onTriggerEnterEvent -= OnGoalEntered;
				}
			}
		}
	}
}
