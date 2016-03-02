using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine 
{	
	public State currentState = null;

	private List<State> stateList = new List<State>(); 
	private List<State> previousStateList = new List<State>();
	

	/**
	 * Event interface.
	 */

	public delegate void OnExitEventHandler( State state, string message );
	public event OnExitEventHandler OnExit;
	
	protected virtual void InvokeExit(State state, string message = null ) 
	{
		if( OnExit != null ) OnExit( state, message );
	}

	public delegate void OnMessageEventHandler( State state, string message );
	public event OnMessageEventHandler OnMessage;
	
	protected virtual void InvokeMessage(State state, string message = null ) 
	{
		if( OnMessage != null ) OnMessage( state, message );
	}

	public delegate void OnEnterEventHandler( State enter );
	public event OnEnterEventHandler OnEnter;
	
	protected virtual void InvokeEnter(State state) 
	{
		if( OnEnter != null ) OnEnter( state );
	}

	public delegate void OnKillEventHandler( State state );
	public event OnKillEventHandler OnKill;
	
	protected virtual void InvokeKill(State state) 
	{
		if( OnKill != null ) OnKill( state );
	}


	/**
	 * Getter / Setter.
	 */

	public string PreviousStateID
	{		
		get { return PreviousState.id; }
	}

	public State PreviousState
	{
		get { return previousStateList[ previousStateList.Count - 1 ] as State; }
	}

	public State GetState(string id)
	{
		for( int i = 0; i < stateList.Count; ++i )
		{
			State state = stateList[ i ] as State;

			if( state.id == id )
				return state;
		}
		
		return null;
	}

	public void SetState(string id)
	{
		if( currentState != null )
		{
			currentState.Exit();
			previousStateList.Add( currentState );
		}

		currentState = GetState( id );
		currentState.Enter();
		// currentState.InvokeEnter();

		InvokeEnter( currentState );
	}


	/**
	 * Public interface.
	 */

	public void Update()
	{
		if( currentState != null )
			currentState.Update();
	}

	public void FixedUpdate()
	{
		if( currentState != null )
			currentState.FixedUpdate();
	}

	public void AddState(string id, State state) 
	{
		state.id = id;
		state.Init();

		state.OnExit += stateOnExitHandler;
		state.OnMessage += stateOnMessageHandler;

		stateList.Add( state );
	}

	public State KillPreviousState()
	{
		if( previousStateList.Count > 1 )
		{
			KillState( PreviousState );
			previousStateList.RemoveAt( previousStateList.Count - 1 );

			return PreviousState;
		}
		else
			return null;
	}

	public void KillPreviousStates()
	{
		while( previousStateList.Count > 0 )
			KillPreviousState();
	}

	public void KillState(State state)
	{
		state.Kill();
		OnKill( state );
	}


	/**
	 * Private interface.
	 */

	private void stateOnExitHandler(State state, string message)
	{
		InvokeExit( state, message );	
	}

	private void stateOnMessageHandler(State state, string message)
	{
		InvokeMessage( state, message );	
	}
}