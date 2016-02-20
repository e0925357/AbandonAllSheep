using UnityEngine;
using System.Collections;
using System;

public class Acid : MonoBehaviour, SheepKiller {

	public GameObject DeadSheep;
	public bool TopLayer;

	public bool Active
	{
		get
		{
			return true;
		}
	}

	// Use this for initialization
	void Start () {
		GetComponent<Animator>().SetBool("IsTopLayer", TopLayer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public AudioClip SheepHit(GameObject sheep)
    {
        GameObject deadSheep = Instantiate(DeadSheep);
        deadSheep.transform.position = sheep.transform.position;
        deadSheep.transform.parent = transform;
        deadSheep.transform.localPosition -= new Vector3(0.0f, 0.0f, -1.0f);

		return null;
	}
}
