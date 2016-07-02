using UnityEngine;
using System.Collections;

public class PlayerSensor : MonoBehaviour {

	public Transform sensorStart;
	public Vector2 sensorExtend;
	public float sensorRange;
	public LayerMask sensorMask;

	public SheepSquasher Listener { get; set; }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Listener == null) return;

		RaycastHit2D hit = Physics2D.BoxCast(
			sensorStart.position, sensorExtend, 0.0f, Vector2.down, sensorRange, sensorMask);

		if(hit)
		{
			GameObject go = hit.collider.gameObject;

			if(go.layer == LayerMask.NameToLayer("Player"))
			{
				Listener.playerUnderneath(go);
			}
			else
			{
				Listener.objectUnderneath(go);
			}
		}
	}
}
