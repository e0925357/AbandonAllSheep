using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioSource musicSource;

	public AudioClip[] clips;
	private int currentClipIndex;

	private bool triggered = false;

	
	
	void OnEnable()
	{
		Health.onDeath += startPlaying;
	}

	void OnDisable()
	{
		Health.onDeath -= startPlaying;
	}

	void startPlaying(GameObject sheep)
	{
		if (triggered || clips.Length <= 0) return;
		
		triggered = true;
		currentClipIndex = clips.Length;

		nextSong();
	}

	void nextSong()
	{
		currentClipIndex++;

		if (currentClipIndex >= clips.Length)
		{
			currentClipIndex = 0;

			for(int i = 1; i < clips.Length; i++)
			{
				int randomIndex = Random.Range(0, i - 1);

				AudioClip buffer = clips[i];

				clips[i] = clips[randomIndex];
				clips[randomIndex] = buffer;
			}
		}

		musicSource.clip = clips[currentClipIndex];
		musicSource.Play();

		Invoke("nextSong", clips[currentClipIndex].length);
	}
}
