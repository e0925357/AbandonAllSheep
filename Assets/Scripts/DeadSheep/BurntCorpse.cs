using UnityEngine;
using System.Collections;
using System;

public class BurntCorpse : CorpseState, Burnable {
	public Flamable flamable;

	public void addHeat(float heat)
	{
		flamable.Heat += heat;
	}

	public override void physicsChanged(CorpseStateManager manager)
	{
		flamable.corpseAnimator = manager.getCurrentAnimator();
	}

	/// <summary>
	/// Will be called before the state has been entered.
	/// </summary>
	protected override void beforeEnterState(CorpseStateManager manager)
	{
		flamable.corpseAnimator = manager.getCurrentAnimator();
	}

	/// <summary>
	/// Will be called after the state has been entered.
	/// </summary>
	protected override void onEnterState(CorpseStateManager manager)
	{
		manager.DynamicSheepKillerAdapter.receiver = flamable.gameObject;
		manager.StaticSheepKillerAdapter.receiver = flamable.gameObject;
	}

	/// <summary>
	/// Will be called before all involved components get deacivated.
	/// </summary>
	protected override void onLeaveState(CorpseStateManager manager)
	{
		manager.DynamicSheepKillerAdapter.receiver = null;
		manager.StaticSheepKillerAdapter.receiver = null;
	}
}
