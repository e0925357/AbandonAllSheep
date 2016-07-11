using UnityEngine;
using System.Collections;

public class StomperSoundController : MonoBehaviour {
	public LoopSoundController slidingLoop;
	public AudioSource haltSource;
	public Extender stomperExtender;

	void stomperExtended()
	{
		slidingLoop.ShouldPlay = false;
		haltSource.Play();
	}

	void stomperMoving()
	{
		if(!stomperExtender.Paused)
			slidingLoop.ShouldPlay = true;
	}

	void stomperStopped()
	{
		slidingLoop.ShouldPlay = false;
	}

	void OnEnable()
	{
		stomperExtender.extendingEvent += stomperMoving;
		stomperExtender.extendedEvent += stomperExtended;
		stomperExtender.retractingEvent += stomperMoving;
		stomperExtender.retractedEvent += stomperStopped;
		stomperExtender.pausedEvent += stomperStopped;
		stomperExtender.unpausedEvent += stomperMoving;
	}

	void OnDisable()
	{
		stomperExtender.extendingEvent -= stomperMoving;
		stomperExtender.extendedEvent -= stomperExtended;
		stomperExtender.retractingEvent -= stomperMoving;
		stomperExtender.retractedEvent -= stomperStopped;
		stomperExtender.pausedEvent -= stomperStopped;
		stomperExtender.unpausedEvent -= stomperMoving;
	}
}
