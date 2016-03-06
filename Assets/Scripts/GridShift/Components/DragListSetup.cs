using DavidOchmann.Collections;
using DavidOchmann.Events;
using DavidOchmann.Grid;
using DavidOchmann.Animation;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace GridShift
{
	public class DragListSetup : MonoBehaviour 
	{
		private int index = 0;
		private GridDisplay gridDisplay;
		private ObjectGrid objectGrid;
		private DragList dragList;
		private DTween dTween;


		/**
		 * Public interface.
		 */

		public void Start()
		{
			initVariables();
		}

		public void Setup(int x, int y, object value)
		{
			Debug.Log( "Setup " + x + " " + y);

			GameObject item = (GameObject)value;
			
			setupItemValue( item );
			setupItemInputMessenger( item );
		}

		public void Update()
		{
			dTween.Update();
			updateDragList();
		}


		/**
		 * Private interface.
		 */

		/** Init variables. */
		private void initVariables()
		{
			dTween = new DTween();
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
			// TODO: Coninue with next state.

			dragList.Kill();
			animateDragListPositionToRoundValue();

			// dragList.Reset();
			// dragList = null;
		}


		/** Aniamtion functions. */
		private void animateDragListPositionToRoundValue()
		{
			Vector2 vector2 = dragList.disposition;

			vector2.x = Mathf.Round( vector2.x / dragList.distance.x ) * dragList.distance.x;
			vector2.y = Mathf.Round( vector2.y / dragList.distance.y ) * dragList.distance.y;

			Tween dragListTween = dTween.Add( TweenFactory.DragListDisposition( dragList, vector2 ) );
			dragListTween.OnComplete += tweenOnCompleteHandler;
		}

		private void tweenOnCompleteHandler(Tween tween)
		{
			gridDisplay.MapListToObjectGrid( dragList.list );
			// mapDragListGameObjectsToGrid();
		}


		/** Map list GameObjects to Grid .*/
		// private void mapDragListGameObjectsToGrid()
		// {
			
		// }


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
				dragList.events.wrapBeginning.AddListener( dragListWrapBeginningHandler );
				dragList.events.wrapEnd.AddListener( dragListWrapEndHandler );
			}
		}

		private void dragListWrapBeginningHandler(DragList dragList)
		{
			if( dragList.orientation == Orientation.Horizontal )
				copyUnityValues( dragList.list, dragList.list.Count - 2, 0 );
			else
				copyUnityValues( dragList.list, 1, dragList.list.Count - 1 );
		}

		private void dragListWrapEndHandler(DragList dragList)
		{
			if( dragList.orientation == Orientation.Horizontal )
				copyUnityValues( dragList.list, 1, dragList.list.Count - 1 );
			else
				copyUnityValues( dragList.list, dragList.list.Count - 2, 0 );
		}

		private void copyUnityValues(List<object> list, int aIndex, int bIndex)
		{
			// List<object> list = dragList.list;

			Unit copyElement = ( (GameObject)list[ aIndex] ).GetComponent<Unit>();
			Unit pasteElement = ( (GameObject)list[ bIndex ] ).GetComponent<Unit>();

			pasteElement.value = copyElement.value;	
		}

		// private void logDragListValues(DragList dragList)
		// {
		// 	List<object> list = dragList.list;

		// 	for( int i = 0; i < list.Count; ++i )
		// 	{
		// 	    GameObject item = (GameObject)list[ i ];
			    
		// 	    Debug.Log( item.GetComponent<Unit>().value );
		// 	}
		// }


		private List<object> getOrientationList(Point point, Orientation orientation, Vector2 delta)
		{
			List<object> list = orientation == Orientation.Horizontal ? objectGrid.GetRow( point.y ) : objectGrid.GetColumn( point.x );

			if( orientation == Orientation.Horizontal )
			{
				extendListWithCloneAt( list, -1, point.y, list.Count - 1, list.Count - 1 );
				extendListWithCloneAt( list, list.Count - 1, point.y, 0, 1 );
			}	
			else
			if( orientation == Orientation.Vertical )
			{
				extendListWithCloneAt( list, point.x, -1, list.Count - 1, list.Count - 1 );
				extendListWithCloneAt( list, point.x, list.Count - 1, 0, 1 );
			}

			return list;
		}

		private void extendListWithCloneAt(List<object> list, int x, int y, int position, int value)
		{
			GameObject item = gridDisplay.GetCloneAtPosition( x, y );
			setupItemInputMessenger( item );


			GameObject opposition = (GameObject)list[ value ];
			Unit unit = opposition.GetComponent<Unit>();

			setupItemValue( item, unit.value );


			int insert = position == 0 ? list.Count : 0;
			list.Insert( insert, item );
		}


		/** Update funcitons. */
		private void updateDragList()
		{
			if( dragList != null )
				dragList.Update();	
		}
	}
}