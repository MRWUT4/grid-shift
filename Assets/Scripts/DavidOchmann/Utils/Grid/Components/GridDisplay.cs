using UnityEngine;
using UnityEngine.Events;

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
			initGrid();
			initPosition();
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


		/**
		 * Private interface.
		 */

		/** Init ObjectGrid. */
		private void initGrid()
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
	}
}