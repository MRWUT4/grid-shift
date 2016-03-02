// using System;
// using System.Collections.Generic;
// using UnityEngine;



// [Serializable]
// public struct StateVO 
// {
// 	public string name;
// 	public GameObject gameObject;
// }


// public class StateInfo : MonoBehaviour
// {
// 	public State state;
// }



// public class GameObjectFactory
// {
// 	private StateVO[] stateList;
// 	private Dictionary<string, GameObject> _states;


// 	public GameObjectFactory(StateVO[] stateList)
// 	{
// 		this.stateList = stateList;
// 	}


// 	/**
// 	 * Getter / Setter
// 	 */

// 	public Dictionary<string, GameObject> states
// 	{
// 		get 
// 	    { 
// 	        if( _states == null )
// 	        {
// 	        	_states = new Dictionary<string, GameObject>();

// 	        	for( int i = 0; i < stateList.Length; ++i )
// 	        	{
// 	        	    StateVO stateVO = stateList[ i ];
// 	        	    _states[ stateVO.name ] = stateVO.gameObject;
// 	        	}
// 	        }

// 	        return _states; 
// 	    }
// 	}

// 	public GameObject GetState(GameObjectState state)
// 	{
// 		GameObject original = states[ state.id ];
// 		GameObject gameObject = (GameObject) GameObject.Instantiate( original, original.transform.position, Quaternion.identity );
		
// 		StateInfo stateInfo = gameObject.AddComponent<StateInfo>();
// 		stateInfo.state = state;

// 		return gameObject;
// 	}
// }