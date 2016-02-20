using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prime31;

public class LevelChanger : MonoBehaviour
{
	public GameObject playerPrefab = null;

	private GameObject playerObject;

	private static readonly string PLAYER_TAG_NAME = "Player";

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
			playerObject = GameObject.FindGameObjectWithTag(PLAYER_TAG_NAME);
			Health.onDeath += OnPlayerDeath;
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

	void OnDetroy()
	{
		// Unregister trigger event
		Health.onDeath -= OnPlayerDeath;
	}

	private void OnPlayerDeath(GameObject go)
	{
		RespawnPlayer(true);
	}

	public void NextLevel()
	{
		int currentScene = gameObject.scene.buildIndex;
		int nextSceneIndex = currentScene + 1;
		if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
		{
			CharacterController2D playerController = playerObject.GetComponent<CharacterController2D>();
			playerController.onTriggerEnterEvent -= OnGoalEntered;

			// Load next level
			SceneManager.UnloadScene(currentScene);
			SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
		}
	}

	public void RespawnPlayer(bool forceNewSheep = false)
	{
		if (playerObject != null && forceNewSheep)
		{
			Health playerHealth = playerObject.GetComponent<Health>();
			if (playerHealth && !playerHealth.IsMarkedForDeath)
			{
				Destroy(playerObject);
			}

			playerObject = null;
		}

		if (playerObject == null)
		{
			// There is no player so we respawn him
			playerObject = Instantiate(playerPrefab);
		}

		// Register for trigger event
		CharacterController2D playerController = playerObject.GetComponent<CharacterController2D>();
		playerController.onTriggerEnterEvent += OnGoalEntered;

		// Respawn player
		PlayerRespawn playerRespawn = playerObject.GetComponent<PlayerRespawn>();
		if (playerRespawn)
		{
			playerRespawn.MoveToRespawn();
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
}
