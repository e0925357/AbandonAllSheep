using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{
    public Sprite[] SpikeSprites;
    public Sprite[] BloodySpikeSprites;
    public GameObject DeadSheep;

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

    public void SheepHit(Vector3 sheep_position)
    {
        spriteRenderer.sprite = BloodySpikeSprites[spikeIndex];
        GameObject deadSheep = Instantiate(DeadSheep);
        sheep_position.y -= 0.75f;
        deadSheep.transform.position = sheep_position;
        deadSheep.transform.parent = transform;
    }
}
