using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    public Sprite BloodySpike;

    private SpriteRenderer spriteRenderer;
    
    // Use this for initialization
	void Start ()
	{
	    spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SheepHit()
    {
        spriteRenderer.sprite = BloodySpike;
    }
}
