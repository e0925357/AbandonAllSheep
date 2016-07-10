using UnityEngine;
using System.Collections;

public class Extender : MonoBehaviour {

	public delegate void StateChanged();

	/// <summary>
	/// Gets called when the Extender changes from beeing retracted to extending.
	/// </summary>
	public event StateChanged extendingEvent;

	/// <summary>
	/// Gets called when the Extender stops extending.
	/// </summary>
	public event StateChanged extendedEvent;

	/// <summary>
	/// Gets called when the Extender changes from beeing extended to retracting.
	/// </summary>
	public event StateChanged retractingEvent;

	/// <summary>
	/// Gets called when the Extender stops retracting.
	/// </summary>
	public event StateChanged retractedEvent;

	public Transform moveablePart;
	public bool activated = false;
	public float retractedTime = 1.0f;
	public float extendedTime = 1.0f;
	public Vector3 extendedDelta = Vector3.up;
	public float extensionDuration = 1.0f;
	public bool paused = false;
	public bool loop = true;

	private float timer = 0.0f;
	private Vector3 localStartPosition;

	public enum State
	{
		RETRACTED, RETRACTED_WAIT, RETRACTED_STOP, EXTENDING, EXTENDED, EXTENDED_WAIT, RETRACTING
	}
	private State state = State.RETRACTED_STOP;

	// Use this for initialization
	void Start () {
		if(moveablePart == null)
		{
			moveablePart = transform;
		}

		localStartPosition = moveablePart.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (!activated && (state == State.RETRACTED || state == State.RETRACTED_STOP) || paused) return;

		switch (state)
		{
			case State.RETRACTED_STOP:
				state = State.EXTENDING;
				if (extendingEvent != null) extendingEvent();
				break;
			case State.RETRACTED:
				state = State.RETRACTED_WAIT;
				StartCoroutine(waitRetracted());
				break;
			case State.EXTENDING:
				extend();
				break;
			case State.EXTENDED:
				state = State.EXTENDED_WAIT;
				StartCoroutine(waitExtended());
				break;
			case State.RETRACTING:
				retract();
				break;
		}
	}

	private IEnumerator waitRetracted()
	{
		yield return new WaitForSeconds(retractedTime);

		if(activated)
		{
			state = State.EXTENDING;
			if (extendingEvent != null) extendingEvent();
		}
		else
		{
			state = State.RETRACTED_STOP;
		}
	}

	private void extend()
	{
		timer += Time.deltaTime;

		if(timer > extensionDuration)
		{
			timer = extensionDuration;
			stopHere();
		}

		moveablePart.localPosition = localStartPosition + extendedDelta * (timer / extensionDuration);
	}

	private IEnumerator waitExtended()
	{
		yield return new WaitForSeconds(extendedTime);

		state = State.RETRACTING;
		if (retractingEvent != null) retractingEvent();
	}

	private void retract()
	{
		timer -= Time.deltaTime;

		if (timer < 0)
		{
			timer = 0;
			stopHere();
		}

		moveablePart.localPosition = localStartPosition + extendedDelta * (timer / extensionDuration);
	}

	/// <summary>
	/// Cases the extender to halt and wait before going into the opposite direction again. Also causes the extended or
	/// retracted event to fire.
	/// </summary>
	public void stopHere()
	{
		if(state == State.EXTENDING)
		{
			state = State.EXTENDED;
			if (extendedEvent != null) extendedEvent();
		}
		else if(state == State.RETRACTING)
		{
			if (activated && loop)
			{
				state = State.RETRACTED;
			}
			else
			{
				state = State.RETRACTED_STOP;
				activated = false;
			}

			if (retractedEvent != null) retractedEvent();
		}
		else
		{
			Debug.LogWarning("Can't stop here when not in extending/retracting state,  current state=" + state);
		}
	}

	/// <summary>
	/// Toggles between extending and retracting. Only possible when the Extender is in one of these two states.
	/// </summary>
	public void toggleExtending()
	{
		if(state != State.EXTENDING && state != State.RETRACTING)
		{
			Debug.LogWarning("Can't toggle between extending/retracting when in different state=" + state);
			return;
		}

		if (state == State.EXTENDING)
			state = State.RETRACTING;
		else
			state = State.EXTENDING;
	}

	public State CurrentState
	{
		get
		{
			return state;
		}
	}

	/// <summary>
	/// The current extension in percentage from 0 to 1 where 0 means retracted and 1 means extended.
	/// </summary>
	public float Extension
	{
		get
		{
			return timer / extensionDuration;
		}
	}
}
