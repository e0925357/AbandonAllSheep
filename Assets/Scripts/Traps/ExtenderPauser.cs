using UnityEngine;
using System.Collections.Generic;

public class ExtenderPauser : MonoBehaviour {
	public Extender controlledExtender;

	private HashSet<Collider2D> colliders = new HashSet<Collider2D>();
	
	void OnTriggerEnter2D(Collider2D collider2D)
	{
		colliders.Add(collider2D);
		controlledExtender.paused = true;
	}

	void OnTriggerExit2D(Collider2D collider2D)
	{
		colliders.Remove(collider2D);
		controlledExtender.paused = colliders.Count > 0;
	}
}
