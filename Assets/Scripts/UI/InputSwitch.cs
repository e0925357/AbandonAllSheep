using UnityEngine;
using System.Collections;

public class InputSwitch : MonoBehaviour {
	public delegate void InputToggled(bool useJoystick);
	public static event InputToggled inputToggledEvent;
	public bool useJoystick = true;

	void Start()
	{
		if (inputToggledEvent != null)
		{
			inputToggledEvent(useJoystick);
		}
	}

	public void toggle()
	{
		useJoystick = !useJoystick;
		if(inputToggledEvent != null)
		{
			inputToggledEvent(useJoystick);
		}
	}
}
