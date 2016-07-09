using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof (Impaler), typeof(Extender))]
public class BigSpike : MonoBehaviour, SheepKiller, SheepSquasher
{
	public Sprite BloodySprite;
	public GameObject DeadSheep;
	public GameObject SpikeSprite;

	public AudioSource spikeOutAudio;
	public AudioClip spikeDeathClip;

	private SpriteRenderer spriteRenderer;
	private bool spikeDeadly;
	private Extender extender;

	void Start ()
	{
		spikeDeadly = true;
		spriteRenderer = SpikeSprite.GetComponent<SpriteRenderer>();

		extender = GetComponent<Extender>();
		Impaler impaler = GetComponent<Impaler>();
		impaler.init(new Vector2(-0.2f,-0.2f), new Vector2(0.5f, 0.3f), extender.moveablePart);
	}

	void OnEnable()
	{
		GetComponent<Extender>().extendingEvent += onExtending;
	}

	void OnDisable()
	{
		GetComponent<Extender>().extendingEvent -= onExtending;
	}
	
	private void onExtending()
	{
		spikeOutAudio.PlayDelayed(0.1f);
	}

	public AudioClip SheepHit(GameObject sheep)
	{
		spriteRenderer.sprite = BloodySprite;

		GameObject deadSheep = Instantiate(DeadSheep);

		deadSheep.transform.position = sheep.transform.position;

		CorpseStateManager corpseManager = deadSheep.GetComponent<CorpseStateManager>();

		if(corpseManager != null)
		{
			corpseManager.currentState = CorpseStateManager.CorpseStateEnum.Normal;
			corpseManager.CurrentPhysicsState = CorpseStateManager.PhysicsState.Static;
		}

		GetComponent<Impaler>().impale(deadSheep);
		
		spikeDeadly = false;

		PlayerSensor sensor = deadSheep.GetComponent<PlayerSensor>();
		if (sensor != null)
		{
			sensor.Listener = this;
		}

		return spikeDeathClip;
	}

	public bool Active {
		get {
			return spikeDeadly;
		}
	}


	void OnParticleCollision(GameObject other)
	{
		spriteRenderer.sprite = BloodySprite;
	}

	public void Enable()
	{
		extender.activated = true;
	}

	public void Disable()
	{
		extender.activated = false;
	}

	public void playerUnderneath(GameObject player)
	{
		handleSomthingsUnderneath();
	}

	public void objectUnderneath(GameObject o)
	{
		//handleSomthingsUnderneath();
		//Do nothing
	}

	private void handleSomthingsUnderneath()
	{
		if(extender.CurrentState == Extender.State.RETRACTING)
		{
			extender.toggleExtending();
		}
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpseManager)
	{
		if (corpseManager.currentPhysicsState == CorpseStateManager.PhysicsState.Static) return new CorpseHitInfo();

		GetComponent<Impaler>().impale(corpseManager.gameObject);
		spikeDeadly = false;
		return new CorpseHitInfo(CorpseStateManager.PhysicsState.Static);
	}
}
