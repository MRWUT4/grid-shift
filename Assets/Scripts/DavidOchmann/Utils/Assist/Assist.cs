using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;


public class Assist
{
	/** List functions. */
	public static object ListContainsValue(List<object> list, object item)
	{
		for( int i = 0; i < list.Count; ++i )
		{
		    object value = list[ i ];
		    
		    if( value == item )
		    	return value;
		}
		
		return null;
	}
	

	/** GameObject functions. */
	public static GameObject GetGameObjectClone(GameObject gameObject)
	{
		GameObject clone = (GameObject) GameObject.Instantiate( gameObject, gameObject.transform.position, Quaternion.identity );
		clone.name = gameObject.name;

		return clone;
	}


	/** Array handling. */
	public static List<GameObject> GetRandomGameObjectsFromList(GameObject[] list, int numElements)
	{
		List<GameObject> value = new List<GameObject>();

		for( int i = 0; i < numElements; ++i )
		{
		    GameObject gameObject = list[ (int)Mathf.Floor( UnityEngine.Random.value * list.Length ) ];
		    value.Add( gameObject );
		}

		return value;
	}

	public static List<object> GetRandomElementsFromList(object[] list, int numElements = 1)
	{
		List<object> value = new List<object>();

		if( list.Length > 0 )
		{
			for( int i = 0; i < numElements; ++i )
			{
			    object gameObject = list[ (int)Mathf.Floor( UnityEngine.Random.value * list.Length ) ];
			    value.Add( gameObject );
			}
		}

		return value;
	}

	/** Value handling. */
	public static object FollowProperty(object target, string property, float value, float speed = .06f, float direction = -1)
	{
		float objectValue = (float)Assist.GetObjectValue( target, property );
		float destination = value + direction * objectValue;
		float destinationSpeed = destination * speed;

		Assist.SetObjectValue( target, property, objectValue + destinationSpeed );

		return target;
	}

	public static object GetObjectValue(object target, string property)
	{
		object value = null;
		Type type = target.GetType();

		PropertyInfo propertyInfo = type.GetProperty( property );

		if( propertyInfo != null )
		{
			value = propertyInfo.GetValue( target, null );
		}
		else
		{
			FieldInfo fieldInfo = type.GetField( property );
			value = fieldInfo.GetValue( target );
		}

		return value;
	}

 	public static object SetObjectValue(object target, string property, object value)
 	{
 		Type type = target.GetType();
		PropertyInfo propertyInfo = type.GetProperty( property );

		if( propertyInfo != null ) 
		{
			propertyInfo.SetValue( target, value, null );
		}
		else
		{
			FieldInfo fieldInfo = type.GetField( property );
			fieldInfo.SetValue( target, value );
		}

		return target;
 	}
}