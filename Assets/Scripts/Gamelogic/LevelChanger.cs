using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prime31;

public class LevelChanger : MonoBehaviour
{
	public GameObject playerPrefab = null;

	private GameObject playerObject;

	private static readonly string PLAYER_TAG_NAME = "Player";
	private static readonly string RESPAWN_TAG_NAME = "Respawn";

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
			RespawnPlayer();
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			NextLevel();
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			RespawnPlayer();
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
		RespawnPlayer();
	}

	public void NextLevel()
	{
		int currentScene = gameObject.scene.buildIndex;
		int nextSceneIndex = currentScene + 1;
		if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
		{
			// Load next level
			SceneManager.UnloadScene(currentScene);
			SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
		}
	}

	public void RespawnPlayer()
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

		
		// There is no player so we respawn him
		playerObject = Instantiate(playerPrefab);

		// Register for trigger event
		RegisterCallbacks();

		// Respawn player
		PlayerRespawn playerRespawn = playerObject.GetComponent<PlayerRespawn>();
		if (playerRespawn)
		{
			playerRespawn.MoveToRespawn();
			playerRespawn.FadeIn();
		}
		else
		{
			Debug.Log("No respawn component found on player. Please add PlayerRespawn component to player");
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
		NextLevel();
		yield return null;
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
