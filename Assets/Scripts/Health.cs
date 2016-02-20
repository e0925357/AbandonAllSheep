using System;
using UnityEngine;
using System.Collections;
using Prime31;

public class Health : MonoBehaviour {

	public delegate void lifeChange(GameObject go);
	public static event lifeChange onBirth;
	public static event lifeChange onDeath;
    
	// Use this for initialization
	void Start () {
		if(onBirth != null)
		{
			onBirth(gameObject);
		}
	}

    void OnDestroy()
	{
		if (onDeath != null)
		{
			onDeath(gameObject);
		}
	}

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.GetComponent<Spike>() != null)
        {
            Destroy(gameObject);
        }
    }
}
