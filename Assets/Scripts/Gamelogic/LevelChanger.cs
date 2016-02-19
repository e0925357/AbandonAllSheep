using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
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
			SceneManager.UnloadScene(currentScene);
			SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
		}
	}
}
