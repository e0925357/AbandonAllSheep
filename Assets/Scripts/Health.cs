using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public delegate void lifeChange(GameObject go);
	public static event lifeChange onBirth;
	public static event lifeChange onDeath;

	// Use this for initialization
	void Start () {
		if(onBirth != null)
		{
			onBirth(gameObject);
		}
	}

	void OnDestroy()
	{
		if (onDeath != null)
		{
			onDeath(gameObject);
		}
	}
}
