using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	public Animator titleAnimator;
	public Renderer titleRenderer;

	private bool activated = false;

	void OnEnable()
	{
		Health.onDeath += onSheepDeath;
	}

	void OnDisable()
	{
		Health.onDeath -= onSheepDeath;
	}

	void onSheepDeath(GameObject sheep)
	{
		if (activated) return;

		activated = true;
		titleRenderer.enabled = true;
		titleAnimator.SetTrigger("start");
	}
}
