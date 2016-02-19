using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float movementForce = 100;
	public float jumpForce = 1000;
	public float maxSpeed = 100;

	private Rigidbody2D rBody;
	private HashSet<int> collidingGameObjects = new HashSet<int>();

	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		float movement = Input.GetAxis("Horizontal") * movementForce * Time.deltaTime;

		Vector2 velocity = rBody.velocity;

		if (Mathf.Abs(velocity.x) < maxSpeed)
		{
			rBody.AddForce(new Vector2(movement, 0));
		}

		if(!Input.GetButton("Horizontal"))
		{
			velocity.x = 0;
			rBody.velocity = velocity;
		}

		if(collidingGameObjects.Count > 0 && Input.GetButtonDown("Jump"))
		{
			rBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
		}
	}

	void OnCollisionExit2D(Collision2D collisionInfo)
	{
		collidingGameObjects.Remove(collisionInfo.gameObject.GetInstanceID());
	}

	void OnCollisionEnter2D(Collision2D collisionInfo)
	{
		foreach (ContactPoint2D contact in collisionInfo.contacts)
		{
			Vector2 lcPoint = contact.point - new Vector2(transform.position.x, transform.position.y);
			if (lcPoint.y < 0 && lcPoint.x > -1 && lcPoint.x < 1)
			{
				collidingGameObjects.Add(collisionInfo.gameObject.GetInstanceID());
				break;
			}
		}
	}
}
