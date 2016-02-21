using UnityEngine;
using System.Collections;

public class PhysicsSheep : MonoBehaviour
{
    public GameObject SplashParticles;
    public GameObject BubblesParticles;
	public Animator sheepAnimator;
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
                GameObject particle = (GameObject)Instantiate(BubblesParticles, new Vector3(0, 0), Quaternion.Euler(-89.99f, 179.99f, 0.0f));
			    particle.transform.parent = transform;
			    particle.transform.localPosition = new Vector3(0, -0.1f);
                particle.transform.localScale = new Vector3(1, 1, 1);


                particle = (GameObject)Instantiate(SplashParticles, transform.position, Quaternion.Euler(-89.99f, 179.99f, 0.0f));
                Destroy(particle, 2.0f);


                sheepAnimator.SetTrigger("acid");
				isDying = true;

				StartCoroutine(dissolve());
			}
		}
	}

	IEnumerator dissolve()
	{
		yield return new WaitForSeconds(28.0f);

		sheepAnimator.SetTrigger("dissolve");
		Destroy(gameObject, 2.0f);
	}
}
