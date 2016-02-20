using UnityEngine;
using System.Collections;

public class BigSpike : MonoBehaviour, SheepKiller
{
    public Sprite BloodySprite;
    public GameObject DeadSheep;
    public GameObject SpikeSprite;
    public Animator SpikeAnimator;
    public float HiddenTime;

    private SpriteRenderer spriteRenderer;
	void Start ()
	{
	    spriteRenderer = SpikeSprite.GetComponent<SpriteRenderer>();
        InvokeRepeating("CycleAnimation", 0, HiddenTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CycleAnimation()
    {
        SpikeAnimator.SetTrigger("StartMove");
    }

    public void SheepHit(GameObject sheep)
    {
        spriteRenderer.sprite = BloodySprite;

        GameObject deadSheep = Instantiate(DeadSheep);
        deadSheep.transform.position = sheep.transform.position;
        deadSheep.transform.parent = transform.parent;
    }



    void OnParticleCollision(GameObject other)
    {
        spriteRenderer.sprite = BloodySprite;
    }
}
