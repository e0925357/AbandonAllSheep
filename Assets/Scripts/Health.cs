using System;
using UnityEngine;
using System.Collections;
using Prime31;

public class Health : MonoBehaviour {

	public delegate void lifeChange(GameObject go);
	public static event lifeChange onBirth;
	public static event lifeChange onDeath;

    public GameObject BloodParticleSystem;
    
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
        SheepKiller killer = collider2D.GetComponent<SheepKiller>();
        if (killer != null)
        {
            killer.SheepHit(gameObject);
            SpawnBlood();
            Destroy(gameObject);
        }
    }

    private void SpawnBlood()
    {
        GameObject particle = (GameObject) Instantiate(BloodParticleSystem, transform.position, Quaternion.Euler(-89.99f, 179.99f, 0.0f));
        Destroy(particle, 2.0f);
    }
}
