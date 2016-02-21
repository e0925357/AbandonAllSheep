using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {
	void Start () {
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}
}
