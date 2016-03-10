using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using DavidOchmann.Collections;
using GridShift;

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
		public GameObject template;
		public Vector2 size = new Vector2( 3, 3 );		
		public Vector2 distance = new Vector2( 100, 100 );		
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
			initGrids();
			initPosition();
		}


		/** Clone the template object and position it at cell position. */
		public GameObject GetCloneAtPosition(int x, int y)
		{
			GameObject clone = Object.Instantiate( template );

			float posX = Mathf.Floor( (float)x * distance.x );
			float posY = Mathf.Floor( -(float)y * distance.y );

			clone.transform.SetParent( gameObject.transform, false );
			clone.transform.localPosition = new Vector3( posX, posY, 0 );

			return clone;
		}


		/** Create ItemVO object with distance to nearest grid cell. */ 
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


		/** Match list items to objectGrid positions. */
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

			    if( i >= size )
			    {
			    	itemVOs.RemoveAt( i );
			    	Object.Destroy( itemVO.item );
			    }
				else
					objectGrid.Set( (int)itemVO.x, (int)itemVO.y, itemVO.item );
			}
		}

		public List<ItemVO> SortItemVOsByDistance(List<ItemVO> list)
		{
			List<ItemVO> sortedList = list.OrderBy( o => o.distance ).ToList();

			return sortedList;
		}


		/** Change alpha value of list items. */
		public void ChangeListAlpha(List<object> list, float alpha)
		{
			for( int i = 0; i < list.Count; ++i )
			{
			    GameObject item = (GameObject)list[ i ];
			    setItemAlpha( item, alpha );
			}	
		}

		public void ChangeListPositionAlpha(List<object> list, Orientation orientation)
		{
			for( int i = 0; i < list.Count; ++i )
			{
			    GameObject item = (GameObject)list[ i ];
			    ChangeItemPositionAlpha( item, orientation );
			}	
		}

		private void ChangeItemPositionAlpha(GameObject item, Orientation orientation)
		{
			Vector3 localPosition = item.transform.localPosition;
			float alpha = 1;

			if( orientation == Orientation.Horizontal )
			{
				if( localPosition.x < 0 )
					alpha = 1 - ( Mathf.Abs( localPosition.x ) / distance.x );
				else
				if( localPosition.x + distance.x > width )
					alpha = -1 * ( ( localPosition.x - width ) / distance.x );
			}
			else
			if( orientation == Orientation.Vertical )
			{
				if( localPosition.y - distance.y < -height )
					alpha = ( localPosition.y + height ) / distance.y;
				else
				if( localPosition.y > 0 )
					alpha = 1 - ( localPosition.y / distance.y );
			}	

			setItemAlpha( item, alpha );
		}

		private void setItemAlpha(GameObject item, float value)
		{
			float alpha = Mathf.Max( 0, Mathf.Min( 1, value ) );

			CanvasGroup canvasGroup = item.GetComponent<CanvasGroup>();
			canvasGroup.alpha = alpha;
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