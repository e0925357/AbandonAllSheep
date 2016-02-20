using UnityEngine;
using System.Collections;

public class SpawnEditorVisual : MonoBehaviour
{
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Vector3 position = transform.position;
		Gizmos.DrawWireSphere(position, 0.5f);
	}
}
