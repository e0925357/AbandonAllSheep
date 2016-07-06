using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileInputVisibility : MonoBehaviour {
	public Joystick joystick;
	public Image image;
	public EventTrigger eventTrigger;

	void OnEnable()
	{
		InputSwitch.inputToggledEvent += inputToggled;
	}

	void OnDisable()
	{
		InputSwitch.inputToggledEvent -= inputToggled;
	}

	void inputToggled(bool useJoystick)
	{
		if(joystick != null)
		{
			joystick.enabled = image.enabled = useJoystick;
		}

		if(eventTrigger != null)
		{
			eventTrigger.enabled = image.enabled = !useJoystick;
		}
	}
}
