using UnityEngine;
using System.Collections;

public class SparkMover : MonoBehaviour {
	public ParticleSystem sparkParticles;
	public float deathDelay = 5f;

	private float activationTime;
	private int currentChild;
	private Transform sparkPath;
	private float speed;
	private bool alive;

	private float currentPathLength;
	private float positionParameter = 0;

	public void init(float activationTime, float speed, Transform sparkPath)
	{
		this.activationTime = activationTime;
		this.speed = speed;
		this.sparkPath = sparkPath;
		currentChild = -1;
		alive = true;

		nextWaypoint();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!alive) return;

		positionParameter += Time.deltaTime * speed;

		if(positionParameter >= currentPathLength)
		{
			nextWaypoint();
		}

		if (alive)
		{
			float param = positionParameter / currentPathLength;
			transform.position = sparkPath.GetChild(currentChild).position +
				(sparkPath.GetChild(currentChild + 1).position - sparkPath.GetChild(currentChild).position) * param;
		}
	}

	void nextWaypoint()
	{
		currentChild++;
		positionParameter = 0;

		Transform waypoint = sparkPath.GetChild(currentChild);

		for (int childId = 0; childId < waypoint.childCount; childId++)
		{
			//Create sub-sparks
			Transform subPath = waypoint.GetChild(childId);

			GameObject subParticle = Instantiate(gameObject);
			subParticle.GetComponent<SparkMover>().init(activationTime, speed, subPath);
		}

		SparkTrigger trigger = waypoint.GetComponent<SparkTrigger>();

		if(trigger != null)
		{
			trigger.trigger(activationTime);
		}

		if (currentChild + 1 >= sparkPath.childCount)
		{
			alive = false;
			sparkParticles.Stop();
			Destroy(gameObject, deathDelay);
			currentPathLength = 0;
		}
		else
		{
			currentPathLength = (waypoint.position - sparkPath.GetChild(currentChild + 1).position).magnitude;
		}
	}

	public bool Alive
	{
		get
		{
			return alive;
		}
	}
}
