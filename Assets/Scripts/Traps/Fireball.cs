using UnityEngine;
using System.Collections;
using System;

public class Fireball : MonoBehaviour, SheepKiller {

	public GameObject DeadSheep;
	public float lifetime = 10;
	public float heat = 60;
	public GameObject spawner;
	public GameObject root;
	public Animator fireballAnimator;
	public LinearMover mover;

	public bool Active
	{
		get
		{
			return fireballAnimator.GetBool("alive");
		}
	}

	void Start()
	{
		StartCoroutine(startSelfDestruction());
	}

	IEnumerator startSelfDestruction()
	{
		yield return new WaitForSeconds(lifetime);

		dissolve();
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

		dissolve();

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

		dissolve();
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpseManager)
	{
		return new CorpseHitInfo(CorpseStateManager.CorpseStateEnum.Burnt);
	}

	void dissolve()
	{
		fireballAnimator.SetBool("alive", false);

		Collider2D collider = GetComponent<Collider2D>();
		if (collider != null)
			collider.enabled = false;

		mover.enabled = false;
	}

	public void killInstance()
	{
		if (root == null)
			Destroy(gameObject);
		else
			Destroy(root);
	}
}
