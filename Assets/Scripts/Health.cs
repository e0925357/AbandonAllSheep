using System;
using UnityEngine;
using System.Collections;
using Prime31;

public class Health : MonoBehaviour {

	public delegate void lifeChange(GameObject go);
	public static event lifeChange onBirth;
	public static event lifeChange onDeath;

	private bool isMarkedForDeath;

	public bool IsMarkedForDeath
	{
		get { return isMarkedForDeath; }
	}

	// Use this for initialization
	void Start () {
		isMarkedForDeath = false;

		if(onBirth != null)
		{
			onBirth(gameObject);
		}
	}

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        Spike spike = collider2D.GetComponent<Spike>();
        if (spike != null)
        {
            spike.SheepHit(transform.position);
			KillSheep();
        }
    }

	void KillSheep()
	{
		isMarkedForDeath = true;

		if (onDeath != null)
		{
			onDeath(gameObject);
		}

		Destroy(gameObject);
	}
}
