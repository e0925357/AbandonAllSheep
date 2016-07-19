using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersonalBest : MonoBehaviour
{
	public Text displayText;
	private int personalBest;
	private int currentScore;

	void Start()
	{
		OnLevelWasLoaded(0);
	}

	// Load last best of the current level & display it
	void OnLevelWasLoaded(int level)
	{
		currentScore = 0;

		if (PlayerPrefs.HasKey("PB-" + SceneManager.GetActiveScene().name))
		{
			personalBest = PlayerPrefs.GetInt("PB-" + SceneManager.GetActiveScene().name);
			displayText.text = "" + personalBest;
		}
		else
		{
			personalBest = int.MaxValue;
			displayText.text = "-";
		}
	}

	//Save new best
	void OnNextLevel()
	{
		if (currentScore < personalBest)
		{
			PlayerPrefs.SetInt("PB-" + SceneManager.GetActiveScene().name, currentScore);
		}
	}

	void OnSheepDeath(GameObject player)
	{
		currentScore++;
	}

	void OnEnable()
	{
		LevelChanger.nextLevelEvent += OnNextLevel;
		Health.onDeath += OnSheepDeath;
	}

	void OnDisable()
	{
		LevelChanger.nextLevelEvent -= OnNextLevel;
		Health.onDeath -= OnSheepDeath;
		PlayerPrefs.Save();
	}
}
