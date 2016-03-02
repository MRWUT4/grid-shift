using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SwitchCollection : MonoBehaviour
{
	public bool active = false;
	public bool addGameObject = false;
	public List<string> monoBehaviourList;


	/**
	 * Static interface.
	 */

	public static Type FindTypeInLoadedAssemblies(string typeName)
	{
		Type _type = null;

		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			_type = assembly.GetType(typeName);
			
			if (_type != null)
				break;
		}

		return _type;
	}

	public static void EnableAll(bool boolean = true)
	{
		SwitchCollection[] switchCollections = ( SwitchCollection[] )FindObjectsOfType( typeof( SwitchCollection ) );

		for( int i = 0; i < switchCollections.Length; ++i )
		{
		    SwitchCollection switchCollection = switchCollections[ i ];
		 	switchCollection.Enable( boolean );   
		}
	}



	/**
	 * Public interface.
	 */

	public void Start()
	{
		Enable( active );
	}	

	public void OnValidate()
	{
		Start();
	}


	public void Enable(bool boolean)
	{
		EnableMonoBehaviourList( boolean );
		EnableGameObject( boolean );
	}

	public void All(bool boolean)
	{
		SwitchCollection.EnableAll( boolean );
	}

	private Type GetType(string name)
	{
		Type type = Type.GetType( name );

		if( type == null )
			type = FindTypeInLoadedAssemblies( name );

		return type;
	}

	private void EnableMonoBehaviourList(bool boolean)
	{
		for( int i = 0; i < monoBehaviourList.Count; ++i )
		{
		    string item = monoBehaviourList[ i ];
		    Type type = GetType( item );

		    if( type != null )
		    {
			    Component[] list = gameObject.GetComponents( type );

			    for( int j = 0; j < list.Length; ++j )
			    {
			        Behaviour monoBehaviour = (Behaviour)list[ j ];
			    	monoBehaviour.enabled = boolean;
			    }
		    }
		    else
		    	Debug.LogWarning( gameObject.name + " has no component of type " + item + "." );
		}
	}

	private void EnableGameObject(bool boolean)
	{
		if( addGameObject )
			gameObject.SetActive( boolean );
	}
}