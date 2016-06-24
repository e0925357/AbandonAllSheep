using UnityEngine;
using System.Collections;
using System;

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

	// Use this for initialization
	void Start ()
	{
	
	}

	void FixedUpdate()
	{
		bool wasOnGround = onGround;
		onGround = Physics2D.BoxCast(groundOffset.position, new Vector2(1.1259f, 0.1f), 0f, Vector2.down, 0.46f, groundMask);
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();

		Debug.DrawLine(groundOffset.transform.position, groundOffset.transform.position + new Vector3(0, -groundCheckRadius, 0));

		float hSpeed = Input.GetAxis("Horizontal");

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
		if(!freezeInput && onGround && Input.GetButtonDown("Jump"))
		{
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
