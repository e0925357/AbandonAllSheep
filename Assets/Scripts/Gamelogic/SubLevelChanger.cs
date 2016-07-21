using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Level changer which can only work properly with a realy LevelChanger instance.
/// This is needed, because only one LevelChanger can live in a scene at one time.
/// However, this System will change anyways when we implement a new LevelChanger System as soon as the different LevelThemes are implemented.
/// TODO: Rewrite the LevelChanger, because this is old GameJam Code and we will need a new one anyways when the different LevelThemes are implemented.
/// </summary>
public class SubLevelChanger : MonoBehaviour
{
	[Tooltip("The LevelChanger instance in the level. Used to perform the levelchange")]
	public LevelChanger levelChanger;

	[Tooltip("The name of the scene to be loaded. Can either be only the scene name or the full path (shown in the \"Build Settings\"), both without the .unity extension.")]
	public string sceneName;

	/// <summary>
	/// Layer where the player is on. Will be initialized in Awake
	/// </summary>
	private static int PLAYER_LAYER;

	// Use this for initialization
	void Awake()
	{
		PLAYER_LAYER = LayerMask.NameToLayer("Player");
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.layer == PLAYER_LAYER)
		{
			// Player triggered the SubLevelChanger
			levelChanger.JumpToScene(sceneName);
		}
	}
}
