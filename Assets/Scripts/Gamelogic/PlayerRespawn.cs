using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerRespawn : MonoBehaviour
{
	public float fadeInTime = 0.6f;
	public AudioSource doorOpenAudio;
	public AudioSource doorCloseAudio;

	private PlayerMover playerMover;
	PhysicsPlayerController ppc;
	private SpriteRenderer playerSpriteRenderer;
	private GameObject respawnObject;
	private Animator doorAnimator;
	private float fadeTimer;

	private static readonly string RESPAWN_TAG_NAME = "Respawn";
	private static readonly Color FADE_START_COLOR = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	private static readonly Color FADE_END_COLOR = Color.white;

	private void setPlayerMoveable(bool moveable)
	{
		if (playerMover)
		{
			playerMover.CanMove = moveable;
		}
		else if (ppc)
		{
			ppc.freezeInput = !moveable;
		}
	}

	void Awake()
	{
		fadeTimer = 0.0f;
		playerMover = GetComponent<PlayerMover>();
		ppc = GetComponent<PhysicsPlayerController>();
		playerSpriteRenderer = GetComponent<SpriteRenderer>();

		if (playerSpriteRenderer == null)
		{
			Queue<Transform> objects2LookAt = new Queue<Transform>();
			objects2LookAt.Enqueue(transform);

			while (playerSpriteRenderer == null && objects2LookAt.Count > 0)
			{
				Transform objectTrans = objects2LookAt.Dequeue();
				playerSpriteRenderer = objectTrans.GetComponent<SpriteRenderer>();

				for (int i = 0; i < objectTrans.childCount; i++)
				{
					objects2LookAt.Enqueue(objectTrans.GetChild(i));
				}
			}
		}
	}

	// Use this for initialization
	void Start()
	{
		MoveToRespawn();
	}

	public void MoveToRespawn()
	{
		respawnObject = GameObject.FindWithTag(RESPAWN_TAG_NAME);
		if (respawnObject != null)
		{
			transform.position = respawnObject.transform.position;
			doorAnimator = respawnObject.GetComponent<Animator>();
		}
		else
		{
			Debug.Log(string.Format("No respawn point found! Please add a gameobject with a \"{0}\" tag or the SpawnPointPrefab to the level", RESPAWN_TAG_NAME));
		}

		setPlayerMoveable(false);
	}

	public void FadeIn()
	{
		doorAnimator.SetBool("open", true);
		doorOpenAudio.Play();
		StartCoroutine(FadeCoroutine());
	}

	private IEnumerator FadeCoroutine()
	{
		fadeTimer = 0.0f;
		yield return new WaitForEndOfFrame();

		while (true)
		{
			fadeTimer += Time.deltaTime;
			float fadeAmount = Mathf.Min(1.0f, fadeTimer / fadeInTime);
			playerSpriteRenderer.color = Color.Lerp(FADE_START_COLOR, FADE_END_COLOR, fadeAmount);

			if (fadeAmount >= 1.0f)
			{
				setPlayerMoveable(true);
				doorAnimator.SetBool("open", false);
				doorCloseAudio.Play();
				Debug.Log("Door should close");
				break;
			}

			yield return null;
		}
	}
}
