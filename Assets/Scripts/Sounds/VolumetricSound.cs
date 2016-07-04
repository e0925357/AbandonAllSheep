using UnityEngine;
using System.Collections;

public class VolumetricSound : MonoBehaviour {
	public Transform areaCorner1;
	public Transform areaCorner2;
	public Transform soundTransform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main == null) return;

		float leftBorder = Mathf.Min(areaCorner1.position.x, areaCorner2.position.x);
		float rightBorder = Mathf.Max(areaCorner1.position.x, areaCorner2.position.x);
		float topBorder = Mathf.Max(areaCorner1.position.y, areaCorner2.position.y);
		float bottomBorder = Mathf.Min(areaCorner1.position.y, areaCorner2.position.y);

		Vector3 newPos = Camera.main.transform.position;

		newPos.x = Mathf.Clamp(newPos.x, leftBorder, rightBorder);
		newPos.y = Mathf.Clamp(newPos.y, bottomBorder, topBorder);

		soundTransform.position = newPos;
	}
}
