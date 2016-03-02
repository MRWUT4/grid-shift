// using UnityEngine;
// using UnityEditor;
// using DavidOchmann;

// [ CustomEditor( typeof(DavidOchmann.GameObjectSpawner) ) ]
// public class GameObjectSpawnerEditor : Editor
// {
// 	// public GameObjectSpawnerEvents events;

// 	private GameObjectSpawner gameObjectSpawner;

// 	public override void OnInspectorGUI()
// 	{
// 		gameObjectSpawner = (GameObjectSpawner)target;

// 		DrawPropertiesExcluding( serializedObject, new string[]
// 		{ 
// 			"m_Script",
// 			"minTimeout",
// 			"maxTimeout",
// 		});

// 		initTimeoutFields();
// 	}


// 	/**
// 	 * Private interface.
// 	 */

// 	private void initTimeoutFields()
// 	{
// 		// EditorGUILayout.Space();
// 		EditorGUILayout.BeginHorizontal();

// 		GUILayout.Label("Range", GUILayout.MinWidth(115.0f) );
// 		GUILayout.Label("min");
// 		gameObjectSpawner.minTimeout = EditorGUILayout.FloatField( gameObjectSpawner.minTimeout );
// 		GUILayout.Label("max");
// 		gameObjectSpawner.maxTimeout = EditorGUILayout.FloatField( gameObjectSpawner.maxTimeout );

// 		EditorGUILayout.EndHorizontal();
// 	}
// }