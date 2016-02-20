using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
	private static readonly string RESPAWN_TAG_NAME = "Respawn";

	// Use this for initialization
	void Start()
	{
		MoveToRespawn();
	}

	public void MoveToRespawn()
	{
		GameObject respawn = GameObject.FindWithTag(RESPAWN_TAG_NAME);
		if (respawn != null)
		{
			transform.position = respawn.transform.position;
		}
		else
		{
			Debug.Log(string.Format("No respawn point found! Please add a gameobject with a \"{0}\" tag or the SpawnPointPrefab to the level", RESPAWN_TAG_NAME));
		}
	}
}
