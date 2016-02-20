using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour {

	public GameObject prefab2Spawn;
	public Vector3 projectileOffset;

	public float waitTime = 1.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine(spawnProjectile());
	}

	IEnumerator spawnProjectile()
	{
		while(true)
		{
			yield return new WaitForSeconds(waitTime);

			Instantiate(prefab2Spawn, transform.position + (transform.rotation * projectileOffset), transform.rotation);
		}
	}
}
