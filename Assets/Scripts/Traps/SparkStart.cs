using UnityEngine;
using System.Collections;

public class SparkStart : MonoBehaviour {
	public GameObject sparkPrefab;
	public float spawnerCooldown = 5.0f;
	public float sparkActivationTime = 2.0f;
	public Transform sparkPathStart;
	public float particleSpeed = 1;

	private float cooldownTimer = 0.0f;

	void Update()
	{
		if (cooldownTimer <= 0) return;

		cooldownTimer -= Time.deltaTime;

		if (cooldownTimer <= 0)
			cooldownTimer = 0;
	}

	public void startSpark()
	{
		if (cooldownTimer > 0) return;

		GameObject spark = Instantiate(sparkPrefab);
		spark.transform.position = sparkPathStart.position;

		spark.GetComponent<SparkMover>().init(sparkActivationTime, particleSpeed, sparkPathStart);

		cooldownTimer = spawnerCooldown;
	}
}
