using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MobileAxisSetter : MonoBehaviour {

	public string axisName;
	public float valueWhenPressed;

	public void axisPressed()
	{
		CrossPlatformInputManager.SetAxis(axisName, valueWhenPressed);
	}

	public void axisReleased()
	{
		CrossPlatformInputManager.SetAxis(axisName, 0);
	}
}
