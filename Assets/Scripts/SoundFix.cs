using UnityEngine;
using System.Collections;

public class SoundFix : MonoBehaviour {

	// Moves the sound to the same z-coordinate as the camera
	void Start () {
		Vector3 pos = transform.position;
		pos.z = Camera.main.transform.position.z;
		transform.position = pos;
	}
}
