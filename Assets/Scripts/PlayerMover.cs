﻿using UnityEngine;
using System.Collections;
using Prime31;

public class PlayerMover : MonoBehaviour {

	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private bool _canMove;
	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private AudioSource _jumpAudio;

	public bool CanMove
	{
		get { return _canMove; }
		set { _canMove = value; }
	}

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		_jumpAudio = GetComponent<AudioSource>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

		_canMove = true;
	}


	#region Event Listeners

	void onControllerCollider(RaycastHit2D hit)
	{
		// bail out on plain old ground hits cause they arent very interesting
		if (hit.normal.y == 1f)
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent(Collider2D col)
	{
		Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
	}


	void onTriggerExitEvent(Collider2D col)
	{
		Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		if (_controller.isGrounded)
			_velocity.y = 0;

		if (_canMove)
		{
			normalizedHorizontalSpeed = Input.GetAxis("Horizontal");
		}
		else
		{
			normalizedHorizontalSpeed = 0.0f;
		}

		if (normalizedHorizontalSpeed > 0)
		{
			if (transform.localScale.x < 0f)
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			if (_controller.isGrounded && _animator != null)
				_animator.Play(Animator.StringToHash("Run"));
		}
		else if (normalizedHorizontalSpeed < 0)
		{
			if (transform.localScale.x > 0f)
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			if (_controller.isGrounded && _animator != null)
				_animator.Play(Animator.StringToHash("Run"));
		}
		else
		{
			if (_controller.isGrounded && _animator != null)
				_animator.Play(Animator.StringToHash("Idle"));
		}


		// we can only jump whilst grounded
		if (_canMove && _controller.isGrounded && Input.GetButtonDown("Jump"))
		{
			_velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
			_jumpAudio.Play();

			if (_animator != null)
				_animator.Play(Animator.StringToHash("Jump"));
		}


		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets uf jump down through one way platforms
		if (_canMove && _controller.isGrounded && Input.GetAxis("Vertical") < -0.5 && !Input.GetButtonDown("Jump"))
		{
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}

		_controller.move(_velocity * Time.deltaTime);

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}
}
