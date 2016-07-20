using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(AudioSource))]
public class PhysicsPlayerController : MonoBehaviour {

	public Animator animator;
	public float maxSpeed = 10f;
	public Transform groundOffset;

	bool flipped = false;
	bool onGround = false;
	float groundCheckRadius = 0.458f;
	public LayerMask groundMask;
	public float jumpForce = 700f;

	public event Action<Collider2D> onTriggerEnterEvent;

	public bool freezeInput = false;

	private AudioSource jumpAudio;

	private static readonly float MIN_JUMP_PITCH = 0.7f;
	private static readonly float MAX_JUMP_PITCH = 1.4f;

	void Awake ()
	{
		jumpAudio = GetComponent<AudioSource>();
	}

	void FixedUpdate()
	{
		bool wasOnGround = onGround;
		onGround = Physics2D.BoxCast(groundOffset.position, new Vector2(1.1259f, 0.1f), 0f, Vector2.down, 0.46f, groundMask);
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();

		Debug.DrawLine(groundOffset.transform.position, groundOffset.transform.position + new Vector3(0, -groundCheckRadius, 0));

		float hSpeed = CrossPlatformInputManager.GetAxis("Horizontal");

		if (freezeInput) return;

		rigidbody2D.velocity = new Vector2(maxSpeed * hSpeed, rigidbody2D.velocity.y);

		if(hSpeed < 0 && !flipped)
		{
			Vector3 scale = transform.localScale;
			scale.x = -Mathf.Abs(scale.x);
			transform.localScale = scale;
			flipped = true;
		}
		else if(hSpeed > 0 && flipped)
		{
			Vector3 scale = transform.localScale;
			scale.x = Mathf.Abs(scale.x);
			transform.localScale = scale;
			flipped = false;
		}

		if (onGround)
		{
			if (Mathf.Abs(hSpeed) > 0.01f)
			{
				animator.Play(Animator.StringToHash("Run"));
			}
			else
			{
				animator.Play(Animator.StringToHash("Idle"));
			}
		}

		if(wasOnGround && !onGround)
		{
			animator.Play(Animator.StringToHash("Jump"));
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!freezeInput && onGround && CrossPlatformInputManager.GetButtonDown("Jump"))
		{
			jumpAudio.pitch = UnityEngine.Random.Range(MIN_JUMP_PITCH, MAX_JUMP_PITCH);
			jumpAudio.Play();

			Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
			onGround = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(onTriggerEnterEvent != null)
			onTriggerEnterEvent(other);
	}
}
