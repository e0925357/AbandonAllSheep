using System;
using UnityEngine;
using System.Collections;

public class CorpseState : MonoBehaviour
{
	public Transform staticNode;
	public Transform dynamicNode;

	private CorpseStateManager manager;

	public void enterState(CorpseStateManager manager)
	{
		this.manager = manager;
		beforeEnterState(manager);

		setEnabled(staticNode == dynamicNode || manager.CurrentPhysicsState == CorpseStateManager.PhysicsState.Static, staticNode);

		if(staticNode != dynamicNode)
			setEnabled(manager.CurrentPhysicsState == CorpseStateManager.PhysicsState.Dynamic, dynamicNode);

		setEnabled(true);
		onEnterState(manager);
	}

	public virtual void physicsChanged(CorpseStateManager manager)
	{
		this.manager = manager;

		if (staticNode != dynamicNode)
		{
			setEnabled(manager.CurrentPhysicsState == CorpseStateManager.PhysicsState.Static, staticNode);
			setEnabled(manager.CurrentPhysicsState == CorpseStateManager.PhysicsState.Dynamic, dynamicNode);
		}
	}

	/// <summary>
	/// Retrieves the node associated with the current physics state of the owning CorpseStateManager.
	/// </summary>
	/// <returns>The node containing the physics-state dependend graphical representations.</returns>
	public Transform getActivePhysicsDependendNode()
	{
		switch (manager.CurrentPhysicsState)
		{
			case CorpseStateManager.PhysicsState.Static:
				return staticNode;
			case CorpseStateManager.PhysicsState.Dynamic:
				return dynamicNode;
			default:
				throw new ArgumentOutOfRangeException();
		}
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
		this.manager = manager;
		onLeaveState(manager);
		setEnabled(false);
		setEnabled(false, staticNode);

		if (staticNode != dynamicNode)
			setEnabled(false, dynamicNode);
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
		if (t == null)
		{
			t = transform;
		}

		Behaviour[] components = t.GetComponents<Behaviour>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].enabled = isEnabled;
		}

		Renderer[] renderers = t.GetComponents<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = isEnabled;
		}

		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			if (child != dynamicNode && child != staticNode)
				setEnabled(isEnabled, child);
		}
	}
}
