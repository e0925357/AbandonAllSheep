using UnityEngine;
using System.Collections;
using System;

public class SparkTrigger : MonoBehaviour, Trigger {
	private volatile bool triggered = false;

	public bool Triggered
	{
		get
		{
			return triggered;
		}
	}

	public void trigger(float activationDuration)
	{
		triggered = true;
		StartCoroutine(deactivate(activationDuration));
	}

	IEnumerator deactivate(float delay)
	{
		yield return new WaitForSeconds(delay);

		triggered = false;
	}
}
