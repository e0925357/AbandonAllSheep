using UnityEngine;
using System.Collections;
using System;

public class ElectricConnector : MonoBehaviour, SheepKiller, Trigger
{

	public GameObject SheepRoot;
	public GameObject ElectricParticles;
	public GameObject DeadSheep;

	public AudioClip electricDeath;

	// Use this for initialization
	void Start ()
	{
		Active = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public AudioClip SheepHit(GameObject sheep)
	{
		GameObject particles = Instantiate(ElectricParticles);
		particles.transform.position = SheepRoot.transform.position;
		particles.transform.parent = SheepRoot.transform;
		Destroy(particles, 2.0f);

		GameObject deadSheep = Instantiate(DeadSheep);
		deadSheep.transform.position = SheepRoot.transform.position;
		deadSheep.transform.parent = transform;
		Active = false;

		return electricDeath;
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpseManager)
	{
		return new CorpseHitInfo();
	}

	public bool Active { get; private set; }
	public bool Triggered {
		get { return !Active;  }
	}
}
