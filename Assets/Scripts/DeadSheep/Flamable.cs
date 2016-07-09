using UnityEngine;
using System.Collections;
using System;

public class Flamable : MonoBehaviour, SheepKiller {

	public float heat = 100;
	public float maxHeat = 200;
	public float putOutRate = 10;
	public SpriteRenderer fireRenderer;
	public Animator fireAnimator;
	public Transform rotationPivot;
	public Animator corpseAnimator;
	public GameObject burntSheepPrefab;
	public LoopSoundController burningSoundController;
	
	// Update is called once per frame
	void Update () {
		if(fireRenderer.enabled)
		{
			rotationPivot.rotation = Quaternion.identity;
		}

		if (Heat <= 0) return;

		Heat -= putOutRate * Time.deltaTime;
	}

	public AudioClip SheepHit(GameObject sheep)
	{
		Instantiate(burntSheepPrefab, sheep.transform.position, sheep.transform.rotation);

		return null;
	}

	public float Heat
	{
		get
		{
			return heat;
		}

		set
		{
			if (heat <= 0 && value > 0)
			{
				StartCoroutine(changeFireState(true));
			}
			else if (heat > 0 && value <= 0)
			{
				StartCoroutine(changeFireState(false));
			}

			if(value < 0)
			{
				value = 0;
			}
			else if(value > maxHeat)
			{
				value = maxHeat;
			}

			heat = value;
		}
	}

	IEnumerator changeFireState(bool burning)
	{
		if (burning)
			fireRenderer.enabled = true;

		fireAnimator.SetBool("burning", burning);

		if (burningSoundController != null)
			burningSoundController.ShouldPlay = burning;

		yield return new WaitForSeconds(1.0f);

		corpseAnimator.SetBool("burning", burning);
		if(!burning)
			fireRenderer.enabled = false;
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpse)
	{
		return new CorpseHitInfo();
	}

	public bool Active
	{
		get
		{
			return Heat > 0;
		}
	}
}
