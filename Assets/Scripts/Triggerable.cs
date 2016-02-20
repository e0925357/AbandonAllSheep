using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Triggerable : MonoBehaviour
{
    public GameObject[] Triggers;
    public UnityEvent OnTriggeredEvent;
    public UnityEvent OnResetEvent;

    private bool isTriggered;
    // Use this for initialization
    void Start ()
    {
        isTriggered = false;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    bool triggered = true;
	    foreach (GameObject trigger in Triggers)
	    {
	        Trigger trig = trigger.GetComponent<Trigger>();
	        if (trig != null && !trig.Triggered)
	        {
	            triggered = false;
	            break;
	        }
	    }

	    if (triggered && !isTriggered)
	    {
	        isTriggered = true;
            OnTriggeredEvent.Invoke();
	    }
        else if (!triggered && isTriggered)
        {
            isTriggered = false;
            OnResetEvent.Invoke();
        }
	}
}
