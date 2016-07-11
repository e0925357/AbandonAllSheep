using UnityEngine;
using System.Collections;

public class LoopSoundController : MonoBehaviour {
	public AudioSource sourceToControl;
	public AnimationCurve volume;
	public float transitionTime = 1.0f;
	public bool startActivated = true;

	private float transition = 0.0f;
	private bool shouldPlay = false;

	// Use this for initialization
	void Start () {
		if(startActivated)
		{
			ShouldPlay = true;
			transition = transitionTime;
		}

		sourceToControl.volume = volume.Evaluate(transition/transitionTime);
	}
	
	// Update is called once per frame
	void Update () {
		if(shouldPlay && transition < transitionTime)
		{
			transition += Time.deltaTime;

			if(transition > transitionTime)
			{
				transition = transitionTime;
			}

			sourceToControl.volume = volume.Evaluate(transition / transitionTime);
		}
		else if (!shouldPlay && transition > 0.0f)
		{
			transition -= Time.deltaTime;

			if (transition <= 0.0f)
			{
				transition = 0.0f;
				sourceToControl.Stop();
			}

			sourceToControl.volume = volume.Evaluate(transition / transitionTime);
		}
	}

	public bool ShouldPlay
	{
		get
		{
			return shouldPlay;
		}

		set
		{
			shouldPlay = value;

			if(shouldPlay && !sourceToControl.isPlaying)
			{
				sourceToControl.Play();
			}
		}
	}
}
