using UnityEngine;
using System.Collections;

public class Acid : MonoBehaviour, SheepKiller {

    public GameObject DeadSheep;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SheepHit(GameObject sheep)
    {
        GameObject deadSheep = Instantiate(DeadSheep);
        deadSheep.transform.position = sheep.transform.position;
        deadSheep.transform.parent = transform;
        deadSheep.transform.localPosition -= new Vector3(0.0f, 0.75f, -1.0f);
        Destroy(deadSheep, 15.0f);
    }
}
