using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour, SheepKiller
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
    
    public void SheepHit(GameObject sheep)
    {
        spriteRenderer.sprite = BloodySpikeSprites[spikeIndex];
        GameObject deadSheep = Instantiate(DeadSheep);
        deadSheep.transform.position = sheep.transform.position;
        deadSheep.transform.parent = transform;
        deadSheep.transform.localPosition -= new Vector3(0.0f, 0.75f, 0.0f);
    }

    void OnParticleCollision(GameObject other)
    {
        spriteRenderer.sprite = BloodySpikeSprites[spikeIndex];
    }
}
