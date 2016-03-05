using DavidOchmann.Collections;
using DavidOchmann.Events;
using DavidOchmann.Grid;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace GridShift
{
	public class UnitSetup : MonoBehaviour 
	{
		private int index = 0;
		private GridDisplay gridDisplay;
		private ObjectGrid objectGrid;
		private DragList dragList;


		/**
		 * Public interface.
		 */

		public void Start()
		{
			initVariables();
		}

		public void Setup(int x, int y, object value)
		{
			GameObject item = (GameObject)value;
			
			setupItemValue( item );
			setupItemInputMessenger( item );
		}


		/**
		 * Private interface.
		 */

		/** Init variables. */
		private void initVariables()
		{
			gridDisplay = GetComponent<GridDisplay>();
			objectGrid = gridDisplay.objectGrid;
		}


		/** Setup functions. */
		private void setupItemValue(GameObject item, int value = -1 )
		{
			Unit unit = item.GetComponent<Unit>();

			if( value == -1 )
			{
				unit.value = index + 1;
				index++;
			}
			else
				unit.value = value;
		}


		/** Setup InputMessenger handling. */
		private void setupItemInputMessenger(GameObject item)
		{
			InputMessenger inputMessenger = item.GetComponent<InputMessenger>();
 			inputMessenger.events.OnDrag.AddListener( itemOnDragHandler );
 			inputMessenger.events.OnDrop.AddListener( itemOnDropHandler );
		}

		private void itemOnDragHandler(GameObject target, PointerEventData eventData)
		{
			setupDragList( target, eventData );
		}

		private void itemOnDropHandler(GameObject target, PointerEventData eventData)
		{
			dragList.kill();
			// dragList = null;
		}


		/** Setup DragList. */
		private void setupDragList(GameObject target, PointerEventData eventData)
		{
			if( dragList == null )
			{
				Vector2 delta = eventData.delta;

				Orientation orientation = Mathf.Abs( delta.x ) > Mathf.Abs( delta.y ) ? Orientation.Horizontal : Orientation.Vertical;
				Point point = objectGrid.GetPosition( target );
				
				List<object> list = getOrientationList( point, orientation, delta );

				dragList = new DragList( target, list, orientation, gridDisplay.distance );
			}
		}

		private List<object> getOrientationList(Point point, Orientation orientation, Vector2 delta)
		{
			List<object> list = orientation == Orientation.Horizontal ? objectGrid.GetRow( point.y ) : objectGrid.GetColumn( point.x );

			if( orientation == Orientation.Horizontal && delta.x > 0 )
				extendListWithCloneAt( list, -1, point.y, list.Count - 1 );
			else
			if( orientation == Orientation.Horizontal )
				extendListWithCloneAt( list, list.Count, point.y, 0 );
			else
			if( orientation == Orientation.Vertical && delta.y < 0 )
				extendListWithCloneAt( list, point.x, -1, list.Count - 1 );
			else
			if( orientation == Orientation.Vertical )
				extendListWithCloneAt( list, point.x, list.Count, 0 );

			return list;
		}

		private void extendListWithCloneAt(List<object> list, int x, int y, int position)
		{
			GameObject item = gridDisplay.GetCloneAtPosition( x, y );
			GameObject opposition = (GameObject)list[ position ];
			Unit unit = opposition.GetComponent<Unit>();

			setupItemValue( item, unit.value );

			int insert = position == 0 ? list.Count : 0;
			list.Insert( insert, item );
		}
	}
}