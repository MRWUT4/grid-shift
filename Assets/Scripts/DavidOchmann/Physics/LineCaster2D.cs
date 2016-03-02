using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;
    	
namespace DavidOchmann.Physics
{
	[ System.Serializable ]
	public class Linecaster2DUnityEvent : UnityEvent<List<RaycastHit2D>> {}

	[ System.Serializable ]
	public class LineCaster2DEvents
	{
		public Linecaster2DUnityEvent onCollision;
	}

	public class LineCaster2D : MonoBehaviour
	{
		public Transform lineStart;
		public Transform lineEnd;
		public Color color = new Color( 0, 1, 0 );
		public string layer = "Obstacle";
		public bool logCollision;

		public LineCaster2DEvents events;

		private bool collisionState;
		private List<RaycastHit2D> collisionList = new List<RaycastHit2D>();


		/**
		 * Public interface.
		 */

		public void Start(){}

		public void Update()
		{
			updateDebuDraw();
			updateCollisionHandling();
		}


		/**
		 * Private interface.
		 */

		/** Draw debug layer to scene view. */
		private void updateDebuDraw()
		{
			Debug.DrawLine( lineStart.position, lineEnd.position, color );
		}


		/** Send collision event if Lincaster hits LayerMask collider. */
		private void updateCollisionHandling()
		{
			RaycastHit2D[] raycastHit2Ds = Physics2D.LinecastAll( lineStart.position, lineEnd.position, 1 << LayerMask.NameToLayer( layer ) );

			if( !getListIsIdentical( raycastHit2Ds ) )
			{
				collisionList = raycastHit2Ds.ToList();
				
				events.onCollision.Invoke( collisionList );
				log( "onCollision" );
			}
		}

		private bool getListIsIdentical(RaycastHit2D[] list)
		{
			if( list.Length != collisionList.Count )
				return false;
			else
			{
				for( int i = 0; i < list.Length; ++i )
				{
				    RaycastHit2D raycastHit2D0 = list[ i ];
				    RaycastHit2D raycastHit2D1 = collisionList[ i ];

				    if( raycastHit2D0 != raycastHit2D1 )
				    	return false;
				}
			}


			return true;
		}

		private void log(string message)
		{
			if( logCollision )
				Debug.Log( message );
		}
	}
}