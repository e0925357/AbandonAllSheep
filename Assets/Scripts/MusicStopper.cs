using UnityEngine;
using System.Collections;

public class MusicStopper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject music = GameObject.Find("Music");

		if(music != null)
		{
			MusicManager mm = music.GetComponent<MusicManager>();

			if(mm != null)
			{
				mm.Stop();
			}
		}
	}
}
