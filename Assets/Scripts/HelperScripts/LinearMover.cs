using UnityEngine;
using System.Collections;

public class LinearMover : MonoBehaviour {

	public Vector3 unitsPerSecond;
	public bool accountForLocalRotation = false;
	// Update is called once per frame
	void Update () {
		if(accountForLocalRotation)
			transform.localPosition += transform.rotation*(unitsPerSecond * Time.deltaTime);
		else
			transform.localPosition += unitsPerSecond * Time.deltaTime;
	}
}
