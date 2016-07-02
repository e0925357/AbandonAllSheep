﻿using UnityEngine;
using System.Collections;
using System;

public class BigSpike : MonoBehaviour, SheepKiller, SheepSquasher
{
	public Sprite BloodySprite;
	public GameObject DeadSheep;
	public GameObject SpikeSprite;

	public AudioSource spikeOutAudio;
	public AudioClip spikeDeathClip;

	public Transform moveablePartent;
	public float hiddenY = 0;
	public float extendedY = 2;
	public float spikeSpeed = 2;
	public float spikeExtendedTime = 1;
	public float HiddenTime;
	public bool StartEnabled;

	private bool spikeActivated;

	private SpriteRenderer spriteRenderer;
	private bool spikeDeadly;

	private enum State
	{
		HIDDEN, HIDDEN_WAIT, UP, EXTENDED, EXTENDED_WAIT, DOWN
	}
	private State state = State.HIDDEN;

	void Start ()
	{
		spikeDeadly = true;
		spikeActivated = StartEnabled;
		spriteRenderer = SpikeSprite.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!spikeActivated && state == State.HIDDEN) return;

		switch(state)
		{
			case State.HIDDEN:
				state = State.HIDDEN_WAIT;
				Invoke("waitHidden", HiddenTime);
				break;
			case State.UP:
				stepUp();
				break;
			case State.EXTENDED:
				state = State.EXTENDED_WAIT;
				Invoke("waitExtended", spikeExtendedTime);
				break;
			case State.DOWN:
				stepDown();
				break;
		}
	}

	private void waitHidden()
	{
		if (spikeActivated)
		{
			spikeOutAudio.PlayDelayed(0.1f);
			state = State.UP;
		}
		else
		{
			state = State.HIDDEN;
		}
	}

	private void stepUp()
	{
		Vector3 pos = moveablePartent.localPosition;
		float sign = Mathf.Sign(extendedY - hiddenY);
		pos.y += sign * Time.deltaTime * spikeSpeed;

		if(sign * pos.y >= sign * extendedY)
		{
			state = State.EXTENDED;
			pos.y = extendedY;
		}

		moveablePartent.localPosition = pos;
	}

	private void waitExtended()
	{
		state = State.DOWN;
	}

	private void stepDown()
	{
		Vector3 pos = moveablePartent.localPosition;
		float sign = Mathf.Sign(extendedY - hiddenY);
		pos.y -= sign * Time.deltaTime * spikeSpeed;

		if (sign * pos.y <= sign * hiddenY)
		{
			state = State.HIDDEN;
			pos.y = hiddenY;
		}

		moveablePartent.localPosition = pos;
	}

	public AudioClip SheepHit(GameObject sheep)
	{
		spriteRenderer.sprite = BloodySprite;

		GameObject deadSheep = Instantiate(DeadSheep);

		deadSheep.transform.position = sheep.transform.position;
		deadSheep.transform.parent = moveablePartent;

		Vector3 deadPosition = new Vector3(Mathf.Clamp(deadSheep.transform.localPosition.x, -0.2f, 0.5f),
			Mathf.Clamp(deadSheep.transform.localPosition.y, -0.2f, 0.3f), 0.0f);
		deadSheep.transform.localPosition = deadPosition;
		spikeDeadly = false;

		PlayerSensor sensor = deadSheep.GetComponent<PlayerSensor>();
		if (sensor != null)
		{
			sensor.Listener = this;
		}

		return spikeDeathClip;
	}

	public bool Active {
		get {
			return spikeDeadly && (state == State.UP || state == State.EXTENDED || state == State.EXTENDED_WAIT);
		}
	}


	void OnParticleCollision(GameObject other)
	{
		spriteRenderer.sprite = BloodySprite;
	}

	public void Enable()
	{
		spikeActivated = true;
	}

	public void Disable()
	{
		spikeActivated = false;
	}

	public void playerUnderneath(GameObject player)
	{
		handleSomthingsUnderneath();
	}

	public void objectUnderneath(GameObject o)
	{
		//handleSomthingsUnderneath();
		//Do nothing
	}

	private void handleSomthingsUnderneath()
	{
		if(state == State.DOWN)
		{
			state = State.UP;
		}
	}
}
