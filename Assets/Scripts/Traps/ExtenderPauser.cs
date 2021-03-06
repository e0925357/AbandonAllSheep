﻿using UnityEngine;

public class ExtenderPauser : PhysicsTrigger
{
	public Extender controlledExtender;

	protected override bool isImportant(Collider2D collider)
	{
		return true;
	}

	protected override void triggerActivated()
	{
		controlledExtender.Paused = true;
	}

	protected override void triggerDeactivated()
	{
		controlledExtender.Paused = false;
	}
}
