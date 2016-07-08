using UnityEngine;
using System.Collections;
using System;

public class SpeepKillerAdapter : MonoBehaviour, SheepKiller
{
	public GameObject receiver;

	public bool Active
	{
		get
		{
			return receiver != null && receiver.GetComponent<SheepKiller>() != null &&
				receiver.GetComponent<SheepKiller>().Active;
		}
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpseManager)
	{
		if(receiver != null && receiver.GetComponent<SheepKiller>() != null)
		{
			return receiver.GetComponent<SheepKiller>().CorpseHit(corpseManager);
		}

		return new CorpseHitInfo();
	}

	public AudioClip SheepHit(GameObject sheep)
	{
		if(receiver != null && receiver.GetComponent<SheepKiller>() != null)
		{
			return receiver.GetComponent<SheepKiller>().SheepHit(sheep);
		}

		return null;
	}
}
