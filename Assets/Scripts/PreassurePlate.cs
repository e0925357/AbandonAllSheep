using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreassurePlate : MonoBehaviour, Trigger
{
	public Sprite EnabledStateSprite;
	public Sprite DisabledStateSprite;
	public float DisableDelay;
	public AudioSource audioSource;
	public AudioClip activateClip;
	public AudioClip deactivateClip;

	public bool Active
	{
		get { return objectsOnPlate.Count > 0; }
	}

	private HashSet<int> objectsOnPlate = new HashSet<int>();
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start()
	{
		objectsOnPlate.Clear();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	

	void OnTriggerEnter2D(Collider2D collider2D)
	{
		if (objectsOnPlate.Count == 0)
		{
			CancelInvoke("Disable");
			spriteRenderer.sprite = EnabledStateSprite;
			Triggered = true;
			audioSource.clip = activateClip;
			audioSource.Play();
		}

		objectsOnPlate.Add(collider2D.gameObject.GetInstanceID());
	}

	void OnTriggerExit2D(Collider2D collider2D)
	{
		gameobjectNotOnPlate(collider2D.gameObject);
	}

	void gameobjectNotOnPlate(GameObject go)
	{
		bool wasActive = Active;
		objectsOnPlate.Remove(go.GetInstanceID());
		if (wasActive && objectsOnPlate.Count == 0)
		{
			Invoke("Disable", DisableDelay);
		}
	}

	private void Disable()
	{
		spriteRenderer.sprite = DisabledStateSprite;
		Triggered = false;
		audioSource.clip = deactivateClip;
		audioSource.Play();
	}

	void handlePlayerDeath(GameObject player)
	{
		gameobjectNotOnPlate(player);
	}

	void OnEnable()
	{
		Health.onDeath += handlePlayerDeath;
	}

	void OnDisable()
	{
		Health.onDeath -= handlePlayerDeath;
	}

	public bool Triggered { get; private set; }
}
