using UnityEngine;
using System.Collections;
using System;

public class Flamable : MonoBehaviour, SheepKiller {

	public float heat = 100;
	public float maxHeat = 200;
	public float putOutRate = 10;
	public float diePercentage = 0.1f;
	public SpriteRenderer[] fireVisuals;
	public float[] fireVisualsAlpha;
	public Animator corpseAnimator;
	public GameObject burntSheepPrefab;

	private float maxDieHeat;

	void Start()
	{
		maxDieHeat = maxHeat * diePercentage;

		fireVisualsAlpha = new float[fireVisuals.Length];

		for (int i = 0; i < fireVisuals.Length; i++)
		{
			SpriteRenderer r = fireVisuals[i];
			fireVisualsAlpha[i] = r.color.a;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Heat <= 0) return;

		Heat -= putOutRate * Time.deltaTime;

		if(Heat < maxDieHeat)
		{
			for (int i = 0; i < fireVisuals.Length; i++)
			{
				SpriteRenderer r = fireVisuals[i];
				r.color = new Color(r.color.r, r.color.g, r.color.b, fireVisualsAlpha[i]*Heat /maxDieHeat);
			}
		}
		else
		{
			for (int i = 0; i < fireVisuals.Length; i++)
			{
				SpriteRenderer r = fireVisuals[i];
				r.color = new Color(r.color.r, r.color.g, r.color.b, fireVisualsAlpha[i]);
			}
		}
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
				foreach (Renderer r in fireVisuals)
				{
					r.enabled = true;
				}

				corpseAnimator.SetBool("burning", true);
			}
			else if (heat > 0 && value <= 0)
			{
				foreach (Renderer r in fireVisuals)
				{
					r.enabled = false;
				}

				corpseAnimator.SetBool("burning", false);
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

	public bool Active
	{
		get
		{
			return Heat > 0;
		}
	}
}
