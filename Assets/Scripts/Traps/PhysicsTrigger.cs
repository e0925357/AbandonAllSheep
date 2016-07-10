using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PhysicsTrigger : MonoBehaviour {

	private HashSet<Collider2D> colliders = new HashSet<Collider2D>();

	void OnTriggerEnter2D(Collider2D collider2D)
	{
		if(!isImportant(collider2D))
		{
			//Ignore
			return;
		}

		if (colliders.Add(collider2D) && colliders.Count == 1)
			triggerActivated();
	}

	void OnTriggerExit2D(Collider2D collider2D)
	{
		if (isImportant(collider2D) && colliders.Remove(collider2D) && colliders.Count <= 0)
			triggerDeactivated();
	}

	protected abstract bool isImportant(Collider2D collider);

	protected abstract void triggerActivated();

	protected abstract void triggerDeactivated();
}
