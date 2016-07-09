using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Impaler))]
public class Spike : MonoBehaviour, SheepKiller
{
	public Sprite[] SpikeSprites;
	public Sprite[] BloodySpikeSprites;
	public GameObject DeadSheep;
	public bool EnableMirroring;
	public AudioClip[] spikeDeathAudioClips;

	private int spikeIndex;

	private SpriteRenderer spriteRenderer;

	public bool Active { get; private set; }

	// Use this for initialization
	void Start ()
	{
		Active = true;
		spriteRenderer = GetComponent<SpriteRenderer>();
		spikeIndex = Random.Range(0, SpikeSprites.Length);

		spriteRenderer.sprite = SpikeSprites[spikeIndex];

		if (EnableMirroring && Random.Range(0, 2) == 1)
		{
			Quaternion localRotation = Quaternion.AngleAxis(180, Vector3.up);
			transform.localRotation = transform.localRotation*localRotation;
		}

		Impaler impaler = GetComponent<Impaler>();
		impaler.init(new Vector2(-0.35f, 0.0f), new Vector2(0.35f, 0.65f), transform);
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

		CorpseStateManager corpseManager = deadSheep.GetComponent<CorpseStateManager>();

		if (corpseManager != null)
		{
			corpseManager.currentState = CorpseStateManager.CorpseStateEnum.Normal;
			corpseManager.CurrentPhysicsState = CorpseStateManager.PhysicsState.Static;
		}

		GetComponent<Impaler>().impale(deadSheep);

		Active = false;
		
		return spikeDeathAudioClips[UnityEngine.Random.Range(0, spikeDeathAudioClips.Length - 1)];
	}

	void OnParticleCollision(GameObject other)
	{
		spriteRenderer.sprite = BloodySpikeSprites[spikeIndex];
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpseManager)
	{
		if (corpseManager.currentPhysicsState == CorpseStateManager.PhysicsState.Static) return new CorpseHitInfo();

		GetComponent<Impaler>().impale(corpseManager.gameObject);
		Active = false;
		return new CorpseHitInfo(CorpseStateManager.PhysicsState.Static);
	}
}
