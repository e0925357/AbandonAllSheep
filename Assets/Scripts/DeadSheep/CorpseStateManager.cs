using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

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

	public SpriteRenderer staticRenderer;
	public Animator staticAnimator;

	public SpriteRenderer dynamicRenderer;
	public Animator dynamicAnimator;

	public PhysicsState currentPhysicsState = PhysicsState.Dynamic;

	public CorpseStateEnum currentState = CorpseStateEnum.Normal;
	public CorpseState[] possibleStates = new CorpseState[3];

	// Use this for initialization
	void Start () {
		rBody2D.isKinematic = currentPhysicsState == PhysicsState.Static;
		staticRenderer.enabled = currentPhysicsState == PhysicsState.Static;
		staticAnimator.enabled = currentPhysicsState == PhysicsState.Static;

		dynamicRenderer.enabled = currentPhysicsState == PhysicsState.Dynamic;
		dynamicAnimator.enabled = currentPhysicsState == PhysicsState.Dynamic;

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

	public void setDynamicRendererSprite(Sprite sprite)
	{
		dynamicRenderer.sprite = sprite;
		dynamicRenderer.enabled = currentPhysicsState == PhysicsState.Dynamic && dynamicRenderer.sprite != null;
	}

	public void setStaticRendererSprite(Sprite sprite)
	{
		staticRenderer.sprite = sprite;
		staticRenderer.enabled = currentPhysicsState == PhysicsState.Static && staticRenderer.sprite != null;
	}

	public void setDynamicAnimatorController(AnimatorController controller)
	{
		dynamicAnimator.runtimeAnimatorController = controller;
		dynamicAnimator.enabled = currentPhysicsState == PhysicsState.Dynamic && dynamicAnimator.runtimeAnimatorController != null;
	}

	public void setStaticAnimatorController(AnimatorController controller)
	{
		staticAnimator.runtimeAnimatorController = controller;
		staticAnimator.enabled = currentPhysicsState == PhysicsState.Static && staticAnimator.runtimeAnimatorController != null;
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

	public SpriteRenderer getCurrentSpriteRenderer()
	{
		switch(currentPhysicsState)
		{
			case PhysicsState.Dynamic:
				return dynamicRenderer;
			case PhysicsState.Static:
				return staticRenderer;
			default:
				return null;
		}
	}

	public Animator getCurrentAnimator()
	{
		switch (currentPhysicsState)
		{
			case PhysicsState.Dynamic:
				return dynamicAnimator;
			case PhysicsState.Static:
				return staticAnimator;
			default:
				return null;
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
			staticRenderer.enabled = currentPhysicsState == PhysicsState.Static && staticRenderer.sprite != null;
			staticAnimator.enabled = currentPhysicsState == PhysicsState.Static && staticAnimator.runtimeAnimatorController != null;

			dynamicRenderer.enabled = currentPhysicsState == PhysicsState.Dynamic && dynamicRenderer.sprite != null;
			dynamicAnimator.enabled = currentPhysicsState == PhysicsState.Dynamic && dynamicAnimator.runtimeAnimatorController != null;
			possibleStates[(int)currentState].physicsChanged(this);
		}
	}

	void OnEnable()
	{
		LevelChanger.nextLevelEvent += levelChanged;
	}

	void OnDisable()
	{
		LevelChanger.nextLevelEvent -= levelChanged;
	}

	void levelChanged()
	{
		Destroy(gameObject);
	}
}
