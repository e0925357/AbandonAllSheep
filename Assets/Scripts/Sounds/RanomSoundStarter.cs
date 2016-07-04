using UnityEngine;
using System.Collections;

public class RanomSoundStarter : MonoBehaviour {
	public AudioSource sourceToStart;

	// Starts the sound at a random position
	void Start () {
		applyRandomOffset();
	}

	/// <summary>
	/// Starts the sound at a random position.
	/// </summary>
	public void applyRandomOffset()
	{
		if(sourceToStart == null)
		{
			Debug.LogWarning("No audio source set! Can't start sound.");
			return;
		}

		AudioClip clip = sourceToStart.clip;
		if (clip == null)
		{
			Debug.LogWarning("The set audio source has no audio clip! Can't start sound.");
			return;
		}

		if (!sourceToStart.isPlaying)
			sourceToStart.Play();

		sourceToStart.time = Random.Range(0.0f, clip.length);
	}
}
