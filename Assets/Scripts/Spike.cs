using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour, SheepKiller
{
	public Sprite[] SpikeSprites;
	public Sprite[] BloodySpikeSprites;
	public GameObject DeadSheep;
	public bool EnableMirroring;
	public AudioClip[] spikeDeathAudioClips;

	private int spikeIndex;

	private SpriteRenderer spriteRenderer;

	public bool Active
	{
		get
		{
			return true;
		}
	}

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spikeIndex = Random.Range(0, SpikeSprites.Length);

		spriteRenderer.sprite = SpikeSprites[spikeIndex];

		if (EnableMirroring && Random.Range(0, 2) == 1)
		{
			Vector3 scale = transform.localScale;
			scale.x = -scale.x;
			transform.localScale = scale;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public AudioClip SheepHit(GameObject sheep)
	{
		spriteRenderer.sprite = BloodySpikeSprites[spikeIndex];
		GameObject deadSheep = Instantiate(DeadSheep);
		deadSheep.transform.position = sheep.transform.position;
		deadSheep.transform.parent = transform;
		deadSheep.transform.localPosition -= new Vector3(0.0f, 0.5f, 0.0f);

		return spikeDeathAudioClips[UnityEngine.Random.Range(0, spikeDeathAudioClips.Length - 1)];
	}

	void OnParticleCollision(GameObject other)
	{
		spriteRenderer.sprite = BloodySpikeSprites[spikeIndex];
	}
}
