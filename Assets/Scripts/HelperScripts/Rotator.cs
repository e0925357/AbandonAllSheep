using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public float degreesPerSecond = 10;
	
	// Update is called once per frame
	void Update () {

		transform.Rotate(Vector3.forward, degreesPerSecond * Time.deltaTime);
	}
}
