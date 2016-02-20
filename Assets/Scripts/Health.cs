using System;
using UnityEngine;
using System.Collections;
using Prime31;

public class Health : MonoBehaviour {

	public delegate void lifeChange(GameObject go);
	public static event lifeChange onBirth;
	public static event lifeChange onDeath;

	public GameObject BloodParticleSystem;

	private bool isMarkedForDeath;

	public bool IsMarkedForDeath
	{
		get { return isMarkedForDeath; }
	}

	// Use this for initialization
	void Start () {
		isMarkedForDeath = false;

		if(onBirth != null)
		{
			onBirth(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider2D)
	{
		SheepKiller killer = collider2D.GetComponent<SheepKiller>();
		if (killer != null && killer.Active)
		{
			killer.SheepHit(gameObject);
			SpawnBlood();
			KillSheep();
		}
	}

	private void SpawnBlood()
	{
		GameObject particle = (GameObject) Instantiate(BloodParticleSystem, transform.position, Quaternion.Euler(-89.99f, 179.99f, 0.0f));
		Destroy(particle, 2.0f);
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
