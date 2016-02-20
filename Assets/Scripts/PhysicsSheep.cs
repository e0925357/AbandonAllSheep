using UnityEngine;
using System.Collections;

public class PhysicsSheep : MonoBehaviour
{

    private bool isDying;

	// Use this for initialization
	void Start ()
	{
	    isDying = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (!isDying)
        {
            Acid acid = collider2D.gameObject.GetComponent<Acid>();
            if (acid != null)
            {
                Destroy(gameObject, 15.0f);
                isDying = true;
            }
        }
    }
}
