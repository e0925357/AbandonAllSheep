using UnityEngine;
using System.Collections;
using System;

public class ObstacleStomper : PhysicsTrigger
{
	public Extender stomperExtender;
	protected override bool isImportant(Collider2D collider)
	{
		return collider.gameObject.layer != LayerMask.NameToLayer("Player");
	}

	protected override void triggerActivated()
	{
		if(stomperExtender.CurrentState == Extender.State.EXTENDING)
			stomperExtender.stopHere();
	}

	protected override void triggerDeactivated()
	{
		//Do nothing
	}
}
