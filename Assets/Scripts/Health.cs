using System;
using UnityEngine;
using System.Collections;
using Prime31;

public class Health : MonoBehaviour {

	public delegate void lifeChange(GameObject go);
	public static event lifeChange onBirth;
	public static event lifeChange onDeath;

	public GameObject BloodParticleSystem;

	public AudioClip[] sheepDeathClips;

	private bool isMarkedForDeath;
	private AudioClip scheduledAudioClip;

	private static readonly float MIN_PITCH = 0.8f;
	private static readonly float MAX_PITCH = 1.3f;

	public bool IsMarkedForDeath
	{
		get { return isMarkedForDeath; }
	}

	// Use this for initialization
	void Start () {
		isMarkedForDeath = false;
		scheduledAudioClip = null;

		if(onBirth != null)
		{
			onBirth(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider2D)
	{
		handleCollision(collider2D.gameObject);
	}

	void OnCollisionEnter2D(Collision2D collision2D)
	{
		handleCollision(collision2D.gameObject);
	}

	void handleCollision(GameObject other)
	{
		SheepKiller killer = other.GetComponent<SheepKiller>();
		if (killer != null && killer.Active)
		{
			AudioClip deathSound = killer.SheepHit(gameObject);
			if (scheduledAudioClip == null && deathSound != null)
			{
				scheduledAudioClip = deathSound;
				Vector3 clipPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
				AudioSource.PlayClipAtPoint(scheduledAudioClip, clipPosition);
				StartCoroutine(ResetScheduledAudioClip());
			}

			SpawnBlood();
			SpawnDeathSound(other);
			KillSheep();
		}
	}

	private IEnumerator ResetScheduledAudioClip()
	{
		yield return new WaitForEndOfFrame();
		scheduledAudioClip = null;
	}

	private void SpawnBlood()
	{
		GameObject particle = (GameObject) Instantiate(BloodParticleSystem, transform.position, Quaternion.Euler(-89.99f, 179.99f, 0.0f));
		Destroy(particle, 2.0f);
	}

	private void SpawnDeathSound(GameObject targetObject)
	{
		if (sheepDeathClips.Length > 0)
		{
			AudioSource audio = targetObject.AddComponent<AudioSource>();

			// Select a random clip
			audio.clip = sheepDeathClips[UnityEngine.Random.Range(0, sheepDeathClips.Length - 1)];
			audio.loop = false;
			audio.spatialBlend = 1.0f;
			audio.minDistance = 25.0f;
			audio.pitch = UnityEngine.Random.Range(MIN_PITCH, MAX_PITCH);
			audio.Play();
		}
	}

	void KillSheep()
	{
		isMarkedForDeath = true;

		if (onDeath != null)
		{
			onDeath(gameObject);
		}

		Destroy(gameObject);
	}
}
