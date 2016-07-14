using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using PersistentGameObjects_1 = PersistentGameObjects;

/// <summary>
/// Describes the section the game is in in this scene.
/// 
/// IMPORTANT: Only add elements at the end of the enumeration and do not remove any of the entries! Leave the "None" value at the beginning of the enumeration.
///            If you do not follow these simple rules dragons will hunt you down and make you correct the mess caused by not following these rules.
/// </summary>
public enum GameSection
{
	None,
	GameLevel,
	MainMenu
}

/// <summary>
/// Elements/GameObjects which are persistent for the whole game. They are only created once and then needed throughout the whole game.
/// </summary>
public class PersistentGameObjects : MonoBehaviour
{
	private static readonly string PERSISTENT_TAG = "PersistentGameObjects";

	/// <summary>
	/// The section this scene belongs to.
	/// </summary>
	public GameSection gameSection = GameSection.None;

	/// <summary>
	/// Persistent objects throughout the whole game. Only created once when this PersistetGameElements script is created for the first time.
	/// </summary>
	public GameObject gamePersistentObjects = null;

	/// <summary>
	/// The persistentObjects for each of the section
	/// </summary>
	public List<GameObject> sectionPersistentObjects = new List<GameObject>();


	private GameObject currentPersistentSectionObjects = null;
	private GameObject currentGamePersistentObjects = null;

	void Awake()
	{
		GameObject[] persistentGameObjects = GameObject.FindGameObjectsWithTag(PERSISTENT_TAG);

		if (persistentGameObjects.Length == 2)
		{
			foreach (GameObject persistentObject in persistentGameObjects)
			{
				// Only check the other object
				if (persistentObject != gameObject)
				{
					// Get the other script and check whether we are still in the same section
					PersistentGameObjects other = persistentObject.GetComponent<PersistentGameObjects>();
					if (other == null)
					{
						Debug.LogError("Objects with the tag \"PersistentGameObjects\" also needs to have the \"PersistentGameObjects\" script added!");
					}
					else if (other.gameSection != gameSection)
					{
						// We are not in the same section --> load the GameObjects for the new section
						other.SpawnSectionObjects(gameSection);
					}

					// Destroy yourself, because there can only be one PersistentGameObjects-object
					Destroy(gameObject);
				}
			}

			return;
		}
		else if (persistentGameObjects.Length > 2)
		{
			Debug.LogError("More than one Object with the tag \"PersistentGameObjects\" is already in the scene. Please add only one of these per scene!");
			Destroy(gameObject);
			return;
		}

		// If this script was created the first time, then it is the one use throughout the whole application
		DontDestroyOnLoad(gameObject);
		SpawnGamePersistentObjects();
		SpawnSectionObjects(gameSection);
	}

	/// <summary>
	/// Spawns the persistent objects for the given section and destroys the old persistent objects if there.
	/// </summary>
	/// <param name="section">The section to load the persistent objects for.</param>
	private void SpawnSectionObjects(GameSection section)
	{
		if (currentPersistentSectionObjects != null)
		{
			Destroy(currentPersistentSectionObjects);
			currentPersistentSectionObjects = null;
		}

		// Update the current section and load it
		gameSection = section;
		if (section != GameSection.None)
		{
			currentPersistentSectionObjects = (GameObject)Instantiate(sectionPersistentObjects[(int)section - 1], Vector3.zero, Quaternion.identity);
			currentPersistentSectionObjects.transform.SetParent(transform, false);
		}
	}

	/// <summary>
	/// Spawns the persistent objects for the whole game.
	/// </summary>
	private void SpawnGamePersistentObjects()
	{
		if (gamePersistentObjects != null)
		{
			currentGamePersistentObjects = (GameObject) Instantiate(gamePersistentObjects, Vector3.zero, Quaternion.identity);
			currentGamePersistentObjects.transform.SetParent(transform, false);
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(PersistentGameObjects))]
public class PersistentGameObjectsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		
		SerializedProperty gameSectionProp = serializedObject.FindProperty("gameSection");
		SerializedProperty gamePersistentObjectsProp = serializedObject.FindProperty("gamePersistentObjects");

		EditorGUILayout.PropertyField(gameSectionProp);
		gamePersistentObjectsProp.objectReferenceValue = EditorGUILayout.ObjectField("Game Persistent Objects", gamePersistentObjectsProp.objectReferenceValue, typeof(GameObject), false);

		// Serialize the list with custom captions
		EditorGUILayout.Space();

		string[] enumNames = Enum.GetNames(typeof(GameSection));
		int enumLength = Math.Max(0, enumNames.Length - 1);
		SerializedProperty listProp = serializedObject.FindProperty("sectionPersistentObjects");
		
		if (enumLength != listProp.arraySize)
		{
			while (enumLength < listProp.arraySize)
			{
				listProp.DeleteArrayElementAtIndex(listProp.arraySize - 1);
			}

			while (enumLength > listProp.arraySize)
			{
				listProp.InsertArrayElementAtIndex(listProp.arraySize);
			}
		}

		for (int i = 0; i < listProp.arraySize; ++i)
		{
			SerializedProperty listElementProp = listProp.GetArrayElementAtIndex(i);

			listElementProp.objectReferenceValue = EditorGUILayout.ObjectField(string.Format("{0} Persistent Objects", enumNames[i + 1]), listElementProp.objectReferenceValue,
				typeof(GameObject), false);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif
