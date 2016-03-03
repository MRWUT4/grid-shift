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
		private void setupItemValue(GameObject item)
		{
			Unit unit = item.GetComponent<Unit>();

			unit.value = index + 1;
			index++;
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
			dragList = null;
		}


		/** Setup DragList. */
		private void setupDragList(GameObject target, PointerEventData eventData)
		{
			if( dragList == null )
			{
				Vector2 delta = eventData.delta;

				Direction direction = Mathf.Abs( delta.x ) > Mathf.Abs( delta.y ) ? Direction.Horizontal : Direction.Vertical;
				Point point = objectGrid.GetPosition( target );
				
				Debug.Log( point );

				List<object> list = direction == Direction.Horizontal ? objectGrid.GetRow( point.y ) : objectGrid.GetColumn( point.x );

				dragList = new DragList( target, list, direction );
			}
		}
	}
}