using UnityEngine;
using System.Collections;
using System;

public class Fireball : MonoBehaviour, SheepKiller {

	public GameObject DeadSheep;
	public float lifetime = 10;
	public float heat = 60;
	public GameObject spawner;
	public GameObject root;

	public bool Active
	{
		get
		{
			return true;
		}
	}

	void Start()
	{
		StartCoroutine(startSelfDestruction());
	}

	IEnumerator startSelfDestruction()
	{
		yield return new WaitForSeconds(lifetime);

		Destroy(gameObject);
	}

	public AudioClip SheepHit(GameObject sheep)
	{
		GameObject deadSheep = Instantiate(DeadSheep);
		deadSheep.transform.position = sheep.transform.position;

		CorpseStateManager corpseManager = deadSheep.GetComponent<CorpseStateManager>();

		if (corpseManager != null)
		{
			corpseManager.currentState = CorpseStateManager.CorpseStateEnum.Burnt;
			corpseManager.CurrentPhysicsState = CorpseStateManager.PhysicsState.Dynamic;
		}

		Destroy(gameObject);

		return null;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.gameObject == spawner) return;

		Flamable f = other.GetComponent<Flamable>();

		if(f != null)
		{
			f.Heat += heat;
		}

		Burnable b = other.GetComponent<Burnable>();

		if(b != null)
		{
			b.addHeat(heat);
		}

		if (root == null)
			Destroy(gameObject);
		else
			Destroy(root);
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpseManager)
	{
		return new CorpseHitInfo(CorpseStateManager.CorpseStateEnum.Burnt);
	}
}
