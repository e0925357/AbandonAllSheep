using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public Transform target;
	public float speed = 1;
	public float deadZone = 1;
	public AnimationCurve respawnAccelerationCurve;

	private Vector3 targetPosition;
	private float accelerationTimer;

	void Start()
	{
		accelerationTimer = 0.0f;
		if (respawnAccelerationCurve.length > 0)
		{
			int lastKey = respawnAccelerationCurve.length - 1;
			accelerationTimer = respawnAccelerationCurve.keys[lastKey].time;
		}
	}

	// Update is called once per frame
	void Update () {
		accelerationTimer += Time.deltaTime;

		if (target == null && targetPosition == transform.position) return;
		
		if (target != null)
		{
			targetPosition = target.position;
		}

		Vector3 delta = targetPosition - transform.position;
		Vector2 delta2 = new Vector2(delta.x, delta.y);

		if(delta2.sqrMagnitude > Mathf.Pow(deadZone, 2))
		{
			Vector2 offset = delta2.normalized * speed * respawnAccelerationCurve.Evaluate(accelerationTimer) * delta2.sqrMagnitude * Time.deltaTime;

			if(offset.sqrMagnitude > delta2.sqrMagnitude)
			{
				offset = delta2;
				delta = Vector3.zero;
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
		accelerationTimer = 0.0f;
	}

	public void stop(GameObject go)
	{
		target = null;
	}
}
