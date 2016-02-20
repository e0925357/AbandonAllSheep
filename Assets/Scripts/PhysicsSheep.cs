using UnityEngine;
using System.Collections;

public class PhysicsSheep : MonoBehaviour
{
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
