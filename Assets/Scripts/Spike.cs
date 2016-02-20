using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    public Sprite[] SpikeSprites;
    public Sprite[] BloodySpikeSprites;

    private int spikeIndex;

    private SpriteRenderer spriteRenderer;
    
    // Use this for initialization
	void Start ()
	{
	    spriteRenderer = GetComponent<SpriteRenderer>();
	    spikeIndex = Random.Range(0, SpikeSprites.Length);

	    spriteRenderer.sprite = SpikeSprites[spikeIndex];

	    if (Random.Range(0, 2) == 1)
	    {
	        transform.localScale = new Vector3(-1, 1, 1);
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SheepHit()
    {
        spriteRenderer.sprite = BloodySpikeSprites[spikeIndex];
    }
}
