using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
	public float fadeInTime = 0.6f;
	public AudioSource doorOpenAudio;
	public AudioSource doorCloseAudio;

	private PlayerMover playerMover;
	private SpriteRenderer playerSpriteRenderer;
	private GameObject respawnObject;
	private Animator doorAnimator;
	private float fadeTimer;

	private static readonly string RESPAWN_TAG_NAME = "Respawn";
	private static readonly Color FADE_START_COLOR = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	private static readonly Color FADE_END_COLOR = Color.white;

	void Awake()
	{
		fadeTimer = 0.0f;
		playerMover = GetComponent<PlayerMover>();
		playerSpriteRenderer = GetComponent<SpriteRenderer>();
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

		playerMover.CanMove = false;
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
		while (true)
		{
			fadeTimer += Time.deltaTime;
			float fadeAmount = Mathf.Min(1.0f, fadeTimer / fadeInTime);
			playerSpriteRenderer.color = Color.Lerp(FADE_START_COLOR, FADE_END_COLOR, fadeAmount);

			if (fadeAmount >= 1.0f)
			{
				playerMover.CanMove = true;
				doorAnimator.SetBool("open", false);
				doorCloseAudio.Play();
				Debug.Log("Door should close");
				break;
			}

			yield return null;
		}
	}
}
