using UnityEngine;
using System.Collections;

public class LinearMover : MonoBehaviour {

	public Vector3 unitsPerSecond;
	
	// Update is called once per frame
	void Update () {
		transform.localPosition += unitsPerSecond * Time.deltaTime;
	}
}
