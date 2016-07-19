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
	void onLevelFinished()
	{
		if (currentScore < personalBest)
		{
			PlayerPrefs.SetInt("PB-" + SceneManager.GetActiveScene().name, currentScore);
		}
	}

	//Reset score
	void onLevelAbort()
	{
		currentScore = 0;
	}

	void OnSheepDeath(GameObject player)
	{
		currentScore++;
	}

	void OnEnable()
	{
		LevelChanger.LevelFinishedEvent += onLevelFinished;
		LevelChanger.LevelAbortedEvent += onLevelAbort;
		Health.onDeath += OnSheepDeath;
	}

	void OnDisable()
	{
		LevelChanger.LevelFinishedEvent -= onLevelFinished;
		LevelChanger.LevelAbortedEvent -= onLevelAbort;
		Health.onDeath -= OnSheepDeath;
		PlayerPrefs.Save();
	}
}
