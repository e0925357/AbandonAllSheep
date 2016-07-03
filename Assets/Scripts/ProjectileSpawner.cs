using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour {

	public GameObject prefab2Spawn;
	public Vector3 projectileOffset;

	public Animator spawnerAnimator;
	public string spawnAnimatorTrigger;
	public AudioSource spawnAudio;

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

			bool mirror = transform.localScale.x < 0;
			GameObject spawnedGO;

			if(transform.localScale.x < 0)
			{
				//mirror shot
				Quaternion mirrorRotation = transform.rotation * Quaternion.AngleAxis(180, Vector3.up);
				Vector3 posOffset = (mirrorRotation * projectileOffset);
				//posOffset.x *= -1;


				spawnedGO = (GameObject)Instantiate(prefab2Spawn, transform.position + posOffset, mirrorRotation);
			}
			else
			{
				spawnedGO = (GameObject)Instantiate(prefab2Spawn, transform.position + (transform.rotation * projectileOffset), transform.rotation);
			}
			
			if(spawnedGO.transform.childCount > 0)
			{
				Fireball fb = spawnedGO.transform.GetChild(0).GetComponent<Fireball>();
				if(fb)
				{
					fb.spawner = gameObject;
				}
			}
			spawnerAnimator.SetTrigger(spawnAnimatorTrigger);

			if(spawnAudio != null)
			{
				spawnAudio.Play();
			}
		}
	}
}
