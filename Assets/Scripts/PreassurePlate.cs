using UnityEngine;
using System.Collections;

public class PreassurePlate : MonoBehaviour, Trigger
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
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        trigggerCount = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (trigggerCount == 0)
        {
            spriteRenderer.sprite = EnabledStateSprite;
            Triggered = true;
        }

        trigggerCount++;
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        trigggerCount--;
        if (trigggerCount == 0)
        {
            Invoke("Disable", DisableDelay);
        }
    }

    private void Disable()
    {
        spriteRenderer.sprite = DisabledStateSprite;
        Triggered = false;
    }

    public bool Triggered { get; private set; }
}
