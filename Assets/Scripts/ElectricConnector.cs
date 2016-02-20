using UnityEngine;
using System.Collections;

public class ElectricConnector : MonoBehaviour, SheepKiller
{

    public GameObject SheepRoot;
    public GameObject ElectricParticles;

	// Use this for initialization
	void Start ()
	{
	    Active = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SheepHit(GameObject sheep)
    {
        GameObject particles = Instantiate(ElectricParticles);
        particles.transform.position = SheepRoot.transform.position;
        Destroy(particles, 2.0f);
    }

    public bool Active { get; private set; }
}
