using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Impaler))]
public class StomperSpike : MonoBehaviour, SheepSquasher {
	public Extender stomperExtender;

	void sheepImpaled(GameObject deadSheep)
	{
		if(deadSheep != null)
		{
			PlayerSensor playerSensor = deadSheep.GetComponent<PlayerSensor>();

			if(playerSensor != null)
			{
				playerSensor.Listener = this;
			}
		}
	}
	
	void OnEnable()
	{
		GetComponent<Impaler>().impaleEvent += sheepImpaled;
	}

	void OnDisable()
	{
		GetComponent<Impaler>().impaleEvent -= sheepImpaled;
	}

	public void playerUnderneath(GameObject player)
	{
		if(stomperExtender.CurrentState == Extender.State.EXTENDING)
		{
			stomperExtender.stopHere();
		}
	}

	public void objectUnderneath(GameObject o)
	{
		if (stomperExtender.CurrentState == Extender.State.EXTENDING)
		{
			stomperExtender.stopHere();
		}
	}
}
