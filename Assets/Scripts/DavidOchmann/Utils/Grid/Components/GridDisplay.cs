using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace DavidOchmann.Grid
{
	[ System.Serializable ]
	public class GridDisplayUnityEvent : UnityEvent<int, int, object>{}

	[ System.Serializable ]
	public class GridDisplayEvents
	{
		public GridDisplayUnityEvent setup;
	}

	public class GridDisplay : MonoBehaviour
	{
		public Vector2 size = new Vector2( 3, 3 );		
		public Vector2 distance = new Vector2( 100, 100 );		
		public GameObject template;
		public GridDisplayEvents events;

		public ObjectGrid positionGrid;
		public ObjectGrid objectGrid;


		/**
		 * Getter / Setter.
		 */
		
		public float width
		{
			get { return size.x * distance.x; }
		}

		public float height
		{
			get { return size.y * distance.y; }
		}


		/**
		 * Public interface.
		 */

		public void Start()
		{
			initObjectGrid();
			initPosition();
			initPositionGrid();
		}

		public GameObject GetCloneAtPosition(int x, int y)
		{
			GameObject clone = Object.Instantiate( template );

			float posX = (float)x * distance.x;
			float posY = -(float)y * distance.y;

			clone.transform.SetParent( gameObject.transform );
			clone.transform.localPosition = new Vector3( posX, posY, 0 );

			return clone;
		}

		private Vector2 GetClosestPoint(Vector3 aVector3)
		{
			float closestDistance = float.NaN;
			Vector2 point = new Vector2();

			positionGrid.ForEveryElementCall( delegate(int x, int y, object item )
			{
				Vector3 bVector3 = (Vector3)item;
				float distance = Vector3.Distance( aVector3, bVector3 );

				if( float.IsNaN( closestDistance ) || distance < closestDistance )
				{
					closestDistance = distance;

					point.x = x;
					point.y = y;
				}
			} );

			return point;
		}

		public void MapListToObjectGrid(List<object> list)
		{
			for( int i = 0; i < list.Count; ++i )
			{
			    GameObject item = (GameObject)list[ i ];
			    Vector2 closetPoint = GetClosestPoint( item.transform.position );

			    GameObject closestItem = (GameObject)objectGrid.Get( (int)closetPoint.x, (int)closetPoint.y );

			    Object.Destroy( closestItem );
			}
		}


		/**
		 * Private interface.
		 */

		/** Init ObjectGrid. */
		private void initObjectGrid()
		{
			objectGrid = new ObjectGrid( (int)size.x, (int)size.y );
			objectGrid.ForEveryElementCall( populateList );
		}

		private void populateList(int x, int y, object value)
		{
			GameObject clone = GetCloneAtPosition( x, y );
			objectGrid.Set( x, y, clone );

			events.setup.Invoke( x, y, clone );
		}


		/** Center grid on canvas. */
		private void initPosition()
		{
			float x = -distance.x * ( size.x - 1 ) * .5f;
			float y = distance.y * ( size.y - 1 ) * .5f;

			transform.localPosition = new Vector3( x, y, 0 );
		}


		/** Init PositionGrid. */
		private void initPositionGrid()
		{
			positionGrid = new ObjectGrid( (int)size.x, (int)size.y );
			positionGrid.Set( x, y, clone.transform.position );
		}
	}
}