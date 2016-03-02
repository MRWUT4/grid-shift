using System;
using System.Collections.Generic;
using UnityEngine;



/**
 * State
 */

public abstract class State
{
	public string id;
	public object proxy;


	public State(object proxy = null)
	{
		this.proxy = proxy;
	}

	/**
	 * Event interface.
	 */

	// public delegate void OnEnterEventHandler( State state );
	// public event OnEnterEventHandler OnEnter;
	
	// public virtual void InvokeEnter() 
	// {
	// 	if( OnEnter != null ) OnEnter( this );
	// }

	public event OnExitEventHandler OnExit;
	public delegate void OnExitEventHandler( State state, string message );
	
	public virtual void InvokeExit(string message = null) 
	{
		if( OnExit != null ) OnExit( this, message );
	}

	public event OnMessageEventHandler OnMessage;
	public delegate void OnMessageEventHandler( State state, string message );
	
	public virtual void InvokeMessage(string message = null) 
	{
		if( OnMessage != null ) OnMessage( this, message );
	}


	/**
	 * Virtual interface.
	 */

	public virtual void Init(){}

	public virtual void Enter(){}
	
	public virtual void Exit(){}
	
	public virtual void Kill(){}

	public virtual void Update(){}

	public virtual void FixedUpdate(){}
}



/**
 * GameObjectProxy.
 */

// public class GameObjectProxy
// {
// 	public StateVO[] stateList;
// 	public GameObject container;

// 	private GameObjectFactory _gameObjectFactory;

// 	public GameObjectFactory gameObjectFactory
// 	{
// 		get
// 		{
// 			_gameObjectFactory = _gameObjectFactory != null ? _gameObjectFactory : new GameObjectFactory( stateList );
// 			return _gameObjectFactory;
// 		}
// 	}
// }



/**
 * SceneStateProxy
 */

// [Serializable]
// public struct StateVO 
// {
// 	public string name;
// 	// public Scene gameObject;
// }

// public class SceneStateProxy : ScriptableObject
// {
// 	public StateVO[] stateList;
// }



/**
 * GameObjectState.
 */

// public abstract class GameObjectState : State
// {
// 	private new GameObjectProxy proxy;

// 	public GameObject gameObject;


// 	/**
// 	 * Constructor.
// 	 */

// 	public GameObjectState(string id, GameObjectProxy proxy) : base(proxy)
// 	{
// 		this.id = id;
// 		this.proxy = proxy;
// 	}


// 	/**
// 	 * Public interface.
// 	 */

// 	public void initGameObject()
// 	{
// 		gameObject = proxy.gameObjectFactory.GetState( this );
// 		gameObject.transform.SetParent( proxy.container.transform );
// 	}
// }