using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace DavidOchmann.Grid
{
	[ System.Serializable ]
	public class GridDisplayUnityEvent : UnityEvent<int, int, object>{}

	[ System.Serializable ]
	public class GridDisplayEvents
	{
		public GridDisplayUnityEvent setup;
	}

	public class ItemVO
	{
		public int x;
		public int y;
		public float distance;
		public GameObject item;
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
		
		// public float width
		// {
		// 	get { return size.x * distance.x; }
		// }

		// public float height
		// {
		// 	get { return size.y * distance.y; }
		// }


		/**
		 * Public interface.
		 */

		public void Start()
		{
			initGrids();
			initPosition();
		}

		public GameObject GetCloneAtPosition(int x, int y)
		{
			GameObject clone = Object.Instantiate( template );

			float posX = Mathf.Floor( (float)x * distance.x );
			float posY = Mathf.Floor( -(float)y * distance.y );

			clone.transform.SetParent( gameObject.transform );
			clone.transform.localPosition = new Vector3( posX, posY, 0 );

			return clone;
		}

		private ItemVO GetDistanceItemVO(Vector3 aVector3)
		{
			float closestDistance = float.NaN;
			ItemVO itemVO = new ItemVO();

			positionGrid.ForEveryElementCall( delegate(int x, int y, object item )
			{
				Vector3 bVector3 = (Vector3)item;
				float distance = Vector3.Distance( aVector3, bVector3 );

				if( float.IsNaN( closestDistance ) || distance < closestDistance )
				{
					closestDistance = distance;

					itemVO.x = x;
					itemVO.y = y;
					itemVO.distance = distance;
				}
			} );

			return itemVO;
		}

		public void MapListToObjectGrid(List<object> list, int size)
		{
			GameObject item = null;
			List<ItemVO> itemVOs = new List<ItemVO>();

			for( int i = 0; i < list.Count; ++i )
			{
			    item = (GameObject)list[ i ];
			    
			    ItemVO itemVO = GetDistanceItemVO( item.transform.localPosition );
			    itemVO.item = item;

			    itemVOs.Add( itemVO );
			}


			itemVOs = SortItemVOsByDistance( itemVOs );

			for( int i = itemVOs.Count - 1; i >= 0; --i )
			{
			    ItemVO itemVO = itemVOs[ i ];
			    // item = (GameObject)objectGrid.Get( itemVO.x, itemVO.y );

			    if( i >= size )
			    {
			    	itemVOs.RemoveAt( i );
			    	Object.Destroy( itemVO.item );
			    }
				else
				{
					objectGrid.Set( (int)itemVO.x, (int)itemVO.y, itemVO.item );
				}
			}
		}

		public List<ItemVO> SortItemVOsByDistance(List<ItemVO> list)
		{
			List<ItemVO> sortedList = list.OrderBy( o => o.distance ).ToList();

			return sortedList;
		}


		/**
		 * Private interface.
		 */

		/** Init ObjectGrid. */
		private void initGrids()
		{
			positionGrid = new ObjectGrid( (int)size.x, (int)size.y );

			objectGrid = new ObjectGrid( (int)size.x, (int)size.y );
			objectGrid.ForEveryElementCall( populateList );
		}

		private void populateList(int x, int y, object value)
		{
			GameObject clone = GetCloneAtPosition( x, y );
			
			objectGrid.Set( x, y, clone );
			positionGrid.Set( x, y, clone.transform.localPosition );

			events.setup.Invoke( x, y, clone );
		}


		/** Center grid on canvas. */
		private void initPosition()
		{
			float x = Mathf.Floor( -distance.x * ( size.x - 1 ) * .5f );
			float y = Mathf.Floor( distance.y * ( size.y - 1 ) * .5f );

			transform.localPosition = new Vector3( x, y, 0 );
		}
	}
}