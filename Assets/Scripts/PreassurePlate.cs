using UnityEngine;
using System.Collections;

public class PreassurePlate : MonoBehaviour
{
    public GameObject Trigger;
    public Sprite EnabledStateSprite;
    public Sprite DisabledStateSprite;
    public float DisableDelay;
    public bool Active
    {
        get { return trigggerCount > 0; }
    }


    private int trigggerCount;
    private Triggerable triggerInterface;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        trigggerCount = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (Trigger != null)
        {
            triggerInterface = Trigger.GetComponent<Triggerable>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (triggerInterface != null && trigggerCount == 0)
        {
            spriteRenderer.sprite = EnabledStateSprite;
            triggerInterface.OnEnable();
        }

        trigggerCount++;
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (triggerInterface != null && trigggerCount > 0)
        {
           Invoke("Disable", DisableDelay);
        }
        trigggerCount--;
    }

    private void Disable()
    {
        spriteRenderer.sprite = DisabledStateSprite;
        triggerInterface.OnDisable();
    }
}
