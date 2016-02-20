using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
	private PlayerRespawn playerRespawn;

	private static readonly string PLAYER_TAG_NAME = "Player";

	public void Start()
	{
		if (SceneManager.sceneCount == 1)
		{
			int levelSceneIndex = gameObject.scene.buildIndex;

			SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
			SceneManager.LoadScene(levelSceneIndex, LoadSceneMode.Additive);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			NextLevel();
		}
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

			// Respawn player
			GameObject playerObject = GameObject.FindGameObjectWithTag(PLAYER_TAG_NAME);
			if (playerObject != null)
			{
				playerRespawn = playerObject.GetComponent<PlayerRespawn>();
				if (playerRespawn)
				{
					playerRespawn.Respawn();
				}
				else
				{
					Debug.Log("No respawn component found on player. Please add PlayerRespawn component to player");
				}
			}
			else
			{
				Debug.Log("No player object found");
			}
		}
	}
}
