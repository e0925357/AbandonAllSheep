using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface SheepKiller
{
	AudioClip SheepHit(GameObject sheep);

	CorpseHitInfo CorpseHit(CorpseStateManager corpseManager);

	bool Active
	{
		get;
	}
}

public class CorpseHitInfo
{
	public readonly CorpseStateManager.CorpseStateEnum newState;
	public readonly CorpseStateManager.PhysicsState newPhysicsState;
	public readonly bool corpseStateSet;
	public readonly bool physicsStateSet;

	public CorpseHitInfo(CorpseStateManager.CorpseStateEnum newState, CorpseStateManager.PhysicsState newPhysicsState)
	{
		this.newState = newState;
		this.newPhysicsState = newPhysicsState;
		corpseStateSet = true;
		physicsStateSet = true;
	}

	public CorpseHitInfo(CorpseStateManager.CorpseStateEnum newState)
	{
		this.newState = newState;
		this.newPhysicsState = CorpseStateManager.PhysicsState.Static;
		corpseStateSet = true;
		physicsStateSet = false;
	}

	public CorpseHitInfo(CorpseStateManager.PhysicsState newPhysicsState)
	{
		this.newState = CorpseStateManager.CorpseStateEnum.Normal;
		this.newPhysicsState = newPhysicsState;
		corpseStateSet = false;
		physicsStateSet = true;
	}

	public CorpseHitInfo()
	{
		this.newState = CorpseStateManager.CorpseStateEnum.Normal;
		this.newPhysicsState = CorpseStateManager.PhysicsState.Static;
		corpseStateSet = false;
		physicsStateSet = false;
	}
}