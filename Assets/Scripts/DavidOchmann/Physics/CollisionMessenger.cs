using System;
using UnityEngine;
using UnityEngine.Events;


namespace DavidOchmann
{
	[ System.Serializable ]
	public class CollisionUnityEvent : UnityEvent<Collider2D>{}

	public class CollisionMessenger : MonoBehaviour
	{ 
		public bool logCollision = false;
		public GameObject[] filterList;
		public CollisionUnityEvent onEnter;
		public CollisionUnityEvent onExit;


		/**
		 * Event functions.
		 */

		// public delegate void OnTriggerEnterEventHandler( Collider2D collider2D );
		// public event OnTriggerEnterEventHandler OnTriggerEnter;
		
		// protected virtual void InvokeTriggerEnter(Collider2D collider2D) 
		// {
		// 	if( OnTriggerEnter != null ) OnTriggerEnter( collider2D );
		// }

		// public delegate void OnTriggerExitEventHandler( Collider2D collider2D );
		// public event OnTriggerExitEventHandler OnTriggerExit;
		
		// protected virtual void InvokeTriggerExit(Collider2D collider2D) 
		// {
		// 	if( OnTriggerExit != null ) OnTriggerExit( collider2D );
		// }


		/**
		 * Public interface.
		 */

		public bool GetActive(Collider2D collider2D)
		{
			if( !enabled )
				return false;


			if( filterList.Length > 0 )
			{
				// GameObject colliderGameObject = collider2D.gameObject;
				// Type type0 = colliderGameObject.GetType();

				for( int i = 0; i < filterList.Length; ++i )
				{
				    GameObject gameObject = filterList[ i ];
				    
				    if( gameObject.name == collider2D.name )
				    	return false;
				}
			}


			return true;
		}

		public void Start(){}

		// private void OnCollisionEnter2D(Collision2D collision2D)
		// {
		// 	Debug.Log( "OnCollisionEnter2D");
		// }

		public void OnTriggerEnter2D(Collider2D collider2D)
		{
			if( GetActive( collider2D ) )
			{
				// InvokeTriggerEnter( collider2D );
				onEnter.Invoke( collider2D );
				initCollisionLog( "OnTriggerEnter2D", collider2D );
			}
		}

		public void OnTriggerExit2D(Collider2D collider2D)
		{
			if( GetActive( collider2D ) )
			{
				// InvokeTriggerExit( collider2D );
				onExit.Invoke( collider2D );
				initCollisionLog( "OnTriggerExit2D", collider2D );
			}
		}

		private void initCollisionLog(string text, Collider2D collider2D)
		{
			if( GetActive( collider2D ) && logCollision )
				Debug.Log( text + ": " + collider2D );
		}
	}
}