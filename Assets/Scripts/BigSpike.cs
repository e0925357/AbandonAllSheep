using UnityEngine;
using System.Collections;

public class BigSpike : MonoBehaviour, SheepKiller
{
    public Sprite BloodySprite;
    public GameObject DeadSheep;
    public GameObject SpikeSprite;
    public Animator SpikeAnimator;
    public float HiddenTime;
    public bool StartEnabled;

    private SpriteRenderer spriteRenderer;
	void Start ()
	{
	    Active = true;
	    spriteRenderer = SpikeSprite.GetComponent<SpriteRenderer>();
	    if (StartEnabled)
	    {
	        InvokeRepeating("CycleAnimation", 0, HiddenTime);
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CycleAnimation()
    {
        SpikeAnimator.SetTrigger("StartMove");
    }

    public AudioClip SheepHit(GameObject sheep)
    {
        spriteRenderer.sprite = BloodySprite;

        GameObject deadSheep = Instantiate(DeadSheep);

        deadSheep.transform.position = sheep.transform.position;
        deadSheep.transform.parent = transform;

        Vector3 deadPosition = new Vector3(Mathf.Clamp(deadSheep.transform.localPosition.x, -0.2f, 0.5f),
            Mathf.Clamp(deadSheep.transform.localPosition.y, 0.8f, 1.3f), 0.0f);
        deadSheep.transform.localPosition = deadPosition;
        Active = false;
      

		return null;
    }

    public bool Active { get; private set; }


    void OnParticleCollision(GameObject other)
    {
        spriteRenderer.sprite = BloodySprite;
    }

    public void Enable()
    {
        InvokeRepeating("CycleAnimation", 0, HiddenTime);
    }

    public void Disable()
    {
        CancelInvoke("CycleAnimation");
    }
}
