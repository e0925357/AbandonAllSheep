using UnityEngine;
using System.Collections;
using System;

public class CorpseStateChanger : MonoBehaviour, Burnable
{
	public CorpseStateManager corpseStateManager;

	public void addHeat(float heat)
	{
		corpseStateManager.addHeat(heat);
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
			CorpseHitInfo hitInfo = killer.CorpseHit(corpseStateManager);

			if(hitInfo.corpseStateSet)
				corpseStateManager.CurrentState = hitInfo.newState;

			if(hitInfo.physicsStateSet)
				corpseStateManager.CurrentPhysicsState = hitInfo.newPhysicsState;
		}
	}
}
