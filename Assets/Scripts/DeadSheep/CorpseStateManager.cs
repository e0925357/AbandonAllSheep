using UnityEngine;
using System.Collections;

public class CorpseStateManager : MonoBehaviour {

	public enum PhysicsState
	{
		Static, Dynamic
	}

	public enum CorpseStateEnum
	{
		Normal = 0, Acid = 1, Burnt = 2
	}

	public Rigidbody2D rBody2D;

	public SpeepKillerAdapter sheepKillerAdapter;
	public Collider2D staticCollider;
	public Collider2D dynamicCollider;

	public PhysicsState currentPhysicsState = PhysicsState.Dynamic;

	public CorpseStateEnum currentState = CorpseStateEnum.Normal;
	public CorpseState[] possibleStates = new CorpseState[3];

	// Use this for initialization
	void Start () {
		rBody2D.isKinematic = currentPhysicsState == PhysicsState.Static;
		staticCollider.enabled = currentPhysicsState == PhysicsState.Static;
		dynamicCollider.enabled = currentPhysicsState == PhysicsState.Dynamic;

		for (int i = 0; i < 3; i++)
		{
			if(i == (int)currentState)
			{
				possibleStates[(int)i].enterState(this);
			}
			else
			{
				possibleStates[(int)i].leaveState(this);
			}
		}
	}
	
	public void addHeat(float heat)
	{
		if(CurrentState == CorpseStateEnum.Burnt)
		{
			Burnable b = null;

			if (possibleStates[(int)currentState] is Burnable)
				b = possibleStates[(int)currentState] as Burnable;

			if (b == null)
				b = possibleStates[(int)currentState].GetComponent<Burnable>();

			if (b != null)
			{
				b.addHeat(heat);
			}
		}
	}

	public SpeepKillerAdapter SheepKillerAdapter
	{
		get
		{
			return sheepKillerAdapter;
		}
	}

	public CorpseStateEnum CurrentState
	{
		get
		{
			return currentState;
		}

		set
		{
			if (value == currentState || !canSwitch(value)) return;

			//do state transition
			possibleStates[(int)currentState].leaveState(this);

			currentState = value;
			possibleStates[(int)currentState].enterState(this);
		}
	}

	private bool canSwitch(CorpseStateEnum newState)
	{
		if(CurrentState == CorpseStateEnum.Normal)
		{
			return true;
		}
		else if(CurrentState == CorpseStateEnum.Burnt)
		{
			return newState == CorpseStateEnum.Acid;
		}
		else
		{
			return false;
		}
	}

	public PhysicsState CurrentPhysicsState
	{
		get
		{
			return currentPhysicsState;
		}

		set
		{
			if (value == currentPhysicsState) return;

			currentPhysicsState = value;

			rBody2D.isKinematic = currentPhysicsState == PhysicsState.Static;
			staticCollider.enabled = currentPhysicsState == PhysicsState.Static;
			dynamicCollider.enabled = currentPhysicsState == PhysicsState.Dynamic;

			possibleStates[(int)currentState].physicsChanged(this);
		}
	}

	void OnEnable()
	{
		LevelChanger.LevelFinishedEvent += levelFinishedChanged;
	}

	void OnDisable()
	{
		LevelChanger.LevelFinishedEvent -= levelFinishedChanged;
	}

	void levelFinishedChanged()
	{
		Destroy(gameObject);
	}
}
