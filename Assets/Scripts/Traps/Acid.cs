using UnityEngine;
using System.Collections;
using System;

public class Acid : MonoBehaviour, SheepKiller {

	public GameObject DeadSheep;
	public bool TopLayer;

	public AudioClip acidDeathClip;

	public bool Active
	{
		get
		{
			return true;
		}
	}

	// Use this for initialization
	void Start () {
		GetComponent<Animator>().SetBool("IsTopLayer", TopLayer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public AudioClip SheepHit(GameObject sheep)
	{
		GameObject deadSheep = Instantiate(DeadSheep);
		deadSheep.transform.position = sheep.transform.position;
		deadSheep.transform.position -= new Vector3(0.0f, 0.0f, -1.0f);

		CorpseStateManager corpseManager = deadSheep.GetComponent<CorpseStateManager>();

		if (corpseManager != null)
		{
			corpseManager.currentState = CorpseStateManager.CorpseStateEnum.Acid;
			corpseManager.CurrentPhysicsState = CorpseStateManager.PhysicsState.Dynamic;
		}

		return acidDeathClip;
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpseManager)
	{
		return new CorpseHitInfo(CorpseStateManager.CorpseStateEnum.Acid);
	}
}
