using UnityEngine;
using System.Collections;

/// <summary>
/// Each spike of the spike door gets this script. They play together and only kill the sheep when it is trapped between them and they are closing.
/// </summary>
public class SpikeDoorSpike : MonoBehaviour, SheepKiller
{
	private bool playerTouching = false;

	/// <summary>
	/// The parent script which controls the whole door.
	/// </summary>
	private SpikeDoor door;

	[SerializeField]
	[Tooltip("The other spike of the spike door. Used to determine whether the player can be killed.")]
	private SpikeDoorSpike otherSpike;

	/// <summary>
	/// Layer where the player is on. Will be initialized in Awake
	/// </summary>
	private static int PLAYER_LAYER;
	
	void Awake()
	{
		PLAYER_LAYER = LayerMask.NameToLayer("Player");
		door = transform.parent.GetComponent<SpikeDoor>();
	}

	void OnEnable()
	{
		Health.onDeath += SheepDeath;
	}

	void OnDisable()
	{
		Health.onDeath -= SheepDeath;
	}

	void SheepDeath(GameObject sheep)
	{
		playerTouching = false;
	}

	void OnCollisionEnter2D(Collision2D collision2D)
	{
		GameObject other = collision2D.gameObject;
		if (other.layer == PLAYER_LAYER)
		{
			playerTouching = true;
		}
	}

	void OnCollisionExit2D(Collision2D collision2D)
	{
		GameObject other = collision2D.gameObject;
		if (other.layer == PLAYER_LAYER)
		{
			playerTouching = false;
		}
	}

	public AudioClip SheepHit(GameObject sheep)
	{
		return door.doorDeathSound;
	}

	public CorpseHitInfo CorpseHit(CorpseStateManager corpseManager)
	{
		return new CorpseHitInfo();
	}

	public bool Active
	{
		get
		{
			return door.IsClosing && otherSpike.playerTouching;
		}
	}
}
