using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public Transform target;
	public float speed = 1;
	public float deadZone = 1;
	
	// Update is called once per frame
	void Update () {
		if (target == null) return;

		Vector3 delta = target.position - transform.position;
		Vector2 delta2 = new Vector2(delta.x, delta.y);

		if(delta2.sqrMagnitude > Mathf.Pow(deadZone, 2))
		{
			Vector2 offset = delta2.normalized * speed * delta2.sqrMagnitude * Time.deltaTime;

			if(offset.sqrMagnitude > delta2.sqrMagnitude)
			{
				offset = delta2;
			}

			transform.position = transform.position + (new Vector3(offset.x, offset.y, 0));
		}
	}

	void OnEnable()
	{
		Health.onBirth += follow;
		Health.onDeath += stop;
	}

	void OnDisable()
	{
		Health.onBirth -= follow;
		Health.onDeath -= stop;
	}

	public void follow(GameObject go)
	{
		target = go.transform;
	}

	public void stop(GameObject go)
	{
		target = null;
	}
}
