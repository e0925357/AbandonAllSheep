using UnityEngine;
using System.Collections;

public class Impaler : MonoBehaviour {
	private Vector2 maxOffset;
	private Vector2 minOffset;
	private Transform deadSheepParent;

	public void init(Vector2 minOffset, Vector2 maxOffset, Transform deadSheepParent)
	{
		this.minOffset = minOffset;
		this.maxOffset = maxOffset;
		this.deadSheepParent = deadSheepParent;
	}

	public void impale(GameObject deadSheep)
	{
		deadSheep.transform.parent = deadSheepParent;
		Vector3 deadPosition = new Vector3(Mathf.Clamp(deadSheep.transform.localPosition.x, minOffset.x, maxOffset.x),
			Mathf.Clamp(deadSheep.transform.localPosition.y, minOffset.y, maxOffset.y), 0.0f);
		deadSheep.transform.localPosition = deadPosition;

		Rigidbody2D rBody = deadSheep.GetComponent<Rigidbody2D>();

		if(rBody != null)
		{
			rBody.isKinematic = true;
		}
	}
}
