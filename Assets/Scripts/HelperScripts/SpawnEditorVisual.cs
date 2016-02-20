using UnityEngine;
using System.Collections;

public class SpawnEditorVisual : MonoBehaviour
{
	public float circleRadius = 0.5f;

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Vector3 position = transform.position;
		Gizmos.DrawWireSphere(position, circleRadius);
	}
}
