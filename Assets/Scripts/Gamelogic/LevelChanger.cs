using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prime31;

public class LevelChanger : MonoBehaviour
{
	public GameObject playerPrefab = null;

	private bool goalTriggered;
	private GameObject playerObject;

	private static readonly string PLAYER_TAG_NAME = "Player";

	public void Start()
	{
		goalTriggered = false;
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

	public void NextLevel()
	{
		int currentScene = gameObject.scene.buildIndex;
		int nextSceneIndex = currentScene + 1;
		if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
		{
			// Unregister trigger event
			CharacterController2D playerController = playerObject.GetComponent<CharacterController2D>();
			playerController.onTriggerEnterEvent -= OnGoalEntered;

			// Load next level
			SceneManager.UnloadScene(currentScene);
			SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
		}
	}

	public void RespawnPlayer()
	{
		playerObject = GameObject.FindGameObjectWithTag(PLAYER_TAG_NAME);
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
			playerRespawn.Respawn();
		}
		else
		{
			Debug.Log("No respawn component found on player. Please add PlayerRespawn component to player");
		}
	}

	private void OnGoalEntered(Collider2D collider)
	{
		if (!goalTriggered)
		{
			if (collider.gameObject == gameObject)
			{
				StartCoroutine(NextLevelCoroutine());
				//goalTriggered = true;
			}
		}
	}

	private IEnumerator NextLevelCoroutine()
	{
		yield return new WaitForEndOfFrame();
		NextLevel();
		yield return null;
	}
}
