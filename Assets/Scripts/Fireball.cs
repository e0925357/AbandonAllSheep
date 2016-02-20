using UnityEngine;
using System.Collections;
using System;

public class Fireball : MonoBehaviour, SheepKiller {

	public GameObject DeadSheep;
	public float lifetime = 10;

	void Start()
	{
		StartCoroutine(startSelfDestruction());
	}

	IEnumerator startSelfDestruction()
	{
		yield return new WaitForSeconds(lifetime);

		Destroy(gameObject);
	}

	public void SheepHit(GameObject sheep)
	{
		GameObject deadSheep = Instantiate(DeadSheep);
		deadSheep.transform.position = sheep.transform.position;

		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(!other.CompareTag("Player"))
			Destroy(gameObject);
	}
}
