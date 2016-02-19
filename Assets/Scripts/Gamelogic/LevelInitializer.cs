using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelInitializer : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
		if (nextScene < SceneManager.sceneCountInBuildSettings)
		{
			SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
		}
	}
}
