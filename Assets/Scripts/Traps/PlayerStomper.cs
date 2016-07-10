using UnityEngine;
using System.Collections;
using System;

public class PlayerStomper : MonoBehaviour, SheepKiller {
	public Extender stomperExtender;
	public float deadlyExtension = 0.5f;
	public GameObject corpsePrefab;

	public bool Active
	{
		get
		{
			return stomperExtender.CurrentState == Extender.State.EXTENDING && stomperExtender.Extension > 0.5;
		}
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpseManager)
	{
		return new CorpseHitInfo();
	}

	public AudioClip SheepHit(GameObject sheep)
	{
		GameObject deadSheep = Instantiate(corpsePrefab);
		deadSheep.transform.position = sheep.transform.position;

		CorpseStateManager corpseManager = deadSheep.GetComponent<CorpseStateManager>();

		if (corpseManager != null)
		{
			corpseManager.currentState = CorpseStateManager.CorpseStateEnum.Normal;
			corpseManager.CurrentPhysicsState = CorpseStateManager.PhysicsState.Dynamic;
		}

		return null;
	}
}
