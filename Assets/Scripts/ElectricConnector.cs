using UnityEngine;
using System.Collections;

public class ElectricConnector : MonoBehaviour, SheepKiller, Trigger
{

    public GameObject SheepRoot;
    public GameObject ElectricParticles;
    public GameObject DeadSheep;

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
        particles.transform.parent = SheepRoot.transform;
        Destroy(particles, 2.0f);

        GameObject deadSheep = Instantiate(DeadSheep);
        deadSheep.transform.position = SheepRoot.transform.position;
        deadSheep.transform.parent = transform;
        Active = false;
    }

    public bool Active { get; private set; }
    public bool Triggered {
        get { return !Active;  }
    }
}
