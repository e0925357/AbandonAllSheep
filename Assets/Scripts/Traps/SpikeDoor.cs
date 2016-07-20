using UnityEngine;
using System.Collections;
using System;

public class SpikeDoor : MonoBehaviour
{
	private Animator animator;
	private volatile bool isClosing;

	public bool IsClosing
	{
		get { return isClosing; }
	}

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator>();
		isClosing = false;
	}

	public void Open()
	{
		animator.SetBool("Open", true);
	}


	public void Close()
	{
		animator.SetBool("Open", false);

		beginDeadlyPhase();
	}

	private void beginDeadlyPhase()
	{
		isClosing = true;
	}

	public void ClosingFinished()
	{
		isClosing = false;
	}

	public void OpeningFinished()
	{
		// Called by an event, but not needed --> thus it is empty
	}
}
