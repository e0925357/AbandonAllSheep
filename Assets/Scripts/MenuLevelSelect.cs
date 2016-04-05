using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuLevelSelect : MonoBehaviour {

	public void LoadLevel(string levelName)
	{
		if (!string.IsNullOrEmpty(levelName))
		{
			SceneManager.LoadScene(levelName, LoadSceneMode.Single);
		}
		else
		{
			Debug.Log("No level name given for loading level");
		}
	}
}
