using UnityEngine;
using System.Collections;

public class AcidCorpse : CorpseState {
	public float timeUntilDissolve = 30.0f;
	public GameObject rootObject;
	public Animator sheepAnimator;
	public ParticleSystem splashParticles;
	public Transform paticleRoot;

	private bool waitForDeath = false;

	public override void physicsChanged(CorpseStateManager manager)
	{
		base.physicsChanged(manager);
		sheepAnimator = getActivePhysicsDependendNode().GetComponent<Animator>();
	}

	/// <summary>
	/// Will be called after the state has been entered.
	/// </summary>
	protected override void onEnterState(CorpseStateManager manager)
	{
		sheepAnimator = getActivePhysicsDependendNode().GetComponent<Animator>();
		splashParticles.Play();
	}

	// Update is called once per frame
	void Update () {
		paticleRoot.rotation = Quaternion.identity;

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
