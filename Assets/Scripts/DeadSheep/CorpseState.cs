using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

public class CorpseState : MonoBehaviour {
	public Sprite staticSprite;
	public AnimatorController staticAnimationController;
	public Sprite dynamicSprite;
	public AnimatorController dynamicAnimationController;

	public void enterState(CorpseStateManager manager)
	{
		beforeEnterState(manager);

		manager.setStaticRendererSprite(staticSprite);
		manager.setStaticAnimatorController(staticAnimationController);
		manager.setDynamicRendererSprite(dynamicSprite);
		manager.setDynamicAnimatorController(dynamicAnimationController);

		setEnabled(true);
		onEnterState(manager);
	}

	public virtual void physicsChanged(CorpseStateManager manager)
	{
		//Do nothing
	}

	/// <summary>
	/// Will be called after the state has been entered.
	/// </summary>
	protected virtual void onEnterState(CorpseStateManager manager)
	{
		//Do nothing
	}

	/// <summary>
	/// Will be called before the state has been entered.
	/// </summary>
	protected virtual void beforeEnterState(CorpseStateManager manager)
	{
		//Do nothing
	}

	public void leaveState(CorpseStateManager manager)
	{
		onLeaveState(manager);
		setEnabled(false);
	}

	/// <summary>
	/// Will be called before all involved components get deacivated.
	/// </summary>
	protected virtual void onLeaveState(CorpseStateManager manager)
	{
		//Do nothing
	}

	private void setEnabled(bool isEnabled, Transform t = null)
	{
		if(t == null)
		{
			t = transform;
		}

		Behaviour[] components = t.GetComponents<Behaviour>();
		for(int i = 0; i < components.Length; i++)
		{
			components[i].enabled = isEnabled;
		}

		Renderer[] renderers = t.GetComponents<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = isEnabled;
		}

		for (int i = 0; i <  t.childCount; i++)
		{
			setEnabled(isEnabled, t.GetChild(i));
		}
	}
}
