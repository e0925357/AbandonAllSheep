using UnityEngine;
using System.Collections;

public class SpikeDoor : MonoBehaviour, SheepKiller
{
	public float doorDeathDelay = 0.5f;

	private Animator animator;
	private BoxCollider2D collider2d;
	private volatile bool isClosing;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator>();
		collider2d = GetComponent<BoxCollider2D>();
		isClosing = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Open()
	{
		animator.SetTrigger("Open");
		Debug.Log("Open");
	}


	public void Close()
	{
		Debug.Log("Close");
		animator.SetTrigger("Close");

		Invoke("beginDeadlyPhase", doorDeathDelay);
	}

	private void beginDeadlyPhase()
	{
		collider2d.enabled = true;
		collider2d.isTrigger = true;
		gameObject.layer = LayerMask.NameToLayer("Trigger");
		isClosing = true;
	}

	public void ClosingFinished()
	{
		Debug.Log("ClosingFinished");
		isClosing = false;
		collider2d.isTrigger = false;
		gameObject.layer = LayerMask.NameToLayer("Default");
	}

	public void OpeningFinished()
	{
		Debug.Log("OpeningFinished");
		collider2d.enabled = false;
	}

	public AudioClip SheepHit(GameObject sheep)
	{
		return null;
	}

	public bool Active { get { return isClosing; } }
}
