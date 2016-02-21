using UnityEngine;
using System.Collections;
using System;

public class Flamable : MonoBehaviour, SheepKiller {

	public float heat = 100;
	public float maxHeat = 200;
	public float putOutRate = 10;
	public float diePercentage = 0.1f;
	public SpriteRenderer fireRenderer;
	public Animator fireAnimator;
	public Transform rotationPivot;
	public Animator corpseAnimator;
	public GameObject burntSheepPrefab;

	private float maxDieHeat;

	void Start()
	{
		maxDieHeat = maxHeat * diePercentage;
	}
	
	// Update is called once per frame
	void Update () {
		if (Heat <= 0) return;

		Heat -= putOutRate * Time.deltaTime;
		
		rotationPivot.rotation = Quaternion.identity;
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
		fireAnimator.SetBool("burning", burning);

		yield return new WaitForSeconds(1.0f);

		corpseAnimator.SetBool("burning", burning);
		fireRenderer.enabled = burning;
	}

	public bool Active
	{
		get
		{
			return Heat > 0;
		}
	}
}
