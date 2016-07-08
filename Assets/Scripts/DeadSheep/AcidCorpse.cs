using UnityEngine;
using System.Collections;

public class AcidCorpse : CorpseState {
	public float timeUntilDissolve = 30.0f;
	public GameObject rootObject;
	public Animator sheepAnimator;

	private bool waitForDeath = false;

	public override void physicsChanged(CorpseStateManager manager)
	{
		sheepAnimator = manager.getCurrentAnimator();
	}

	/// <summary>
	/// Will be called after the state has been entered.
	/// </summary>
	protected override void onEnterState(CorpseStateManager manager)
	{
		sheepAnimator = manager.getCurrentAnimator();
	}

	// Update is called once per frame
	void Update () {
		if (waitForDeath) return;

		timeUntilDissolve -= Time.deltaTime;

		if(timeUntilDissolve <= 0)
		{
			waitForDeath = true;
			sheepAnimator.SetTrigger("Dissolve");
			Destroy(rootObject, 2.0f);
		}
	}
}
