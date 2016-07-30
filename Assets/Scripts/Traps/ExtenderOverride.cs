using UnityEngine;

public class ExtenderOverride : PhysicsTrigger
{
	public Extender controlledExtender;

	protected override bool isImportant(Collider2D collider)
	{
		return true;
	}

	protected override void triggerActivated()
	{
		controlledExtender.ExtendOverride = true;
	}

	protected override void triggerDeactivated()
	{
		controlledExtender.ExtendOverride = false;
	}
}
