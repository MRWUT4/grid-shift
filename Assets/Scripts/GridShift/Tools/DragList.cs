using DavidOchmann.Events;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using GridShift;

namespace DavidOchmann.Collections
{
	public enum Orientation{ Horizontal, Vertical }; 

	
	public class DragListUnityEvent : UnityEvent<DragList>{}

	public class DragListEvents
	{
		public DragListUnityEvent wrapBeginning = new DragListUnityEvent();
		public DragListUnityEvent wrapEnd = new DragListUnityEvent();
		public DragListUnityEvent update = new DragListUnityEvent();
	}


	public class DragList
	{
		public GameObject target;
		public List<object> list;
		public Orientation orientation;
		public Dictionary<GameObject, Vector3> positions;
		public Vector2 distance;
		public Vector2 disposition;
		public DragListEvents events = new DragListEvents();

		private float wrapIndex;
		private InputMessenger inputMessenger;


		public DragList(GameObject target, List<object> list, Orientation orientation, Vector2 distance)
		{
			this.target = target;
			this.list = list;
			this.orientation = orientation;
			this.distance = distance;

			init();
		}


		/**
		 * Public interface.
		 */

		public void Reset()
		{
			disposition = new Vector2();
			wrapIndex = 0;
		}

		public void Kill()
		{
			initInputMessenger( false );
		}

		public void Update()
		{
			if( positions != null )
			{
				updateListWrapHandling( disposition );
				updateListItemsDelta( disposition );
				updateEventDispatch();
			}
		}


		/**
		 * Private interface.
		 */

		private void init()
		{
			Reset();

			initVariables();
			initInputMessenger();
		}


		/** Init variables. */
		private void initVariables()
		{
			inputMessenger = target.GetComponent<InputMessenger>();
			// disposition = new Vector2();
		}


		/** InputMessenger functions. */
		private void initInputMessenger(bool boolean = true)
		{
			if( boolean )
			{
				inputMessenger.events.OnDrag.AddListener( targetOnDragHandler );
				// inputMessenger.events.OnDrop.AddListener( targetOnDropHandler );
			}
			else
			{
				inputMessenger.events.OnDrag.RemoveListener( targetOnDragHandler );
				// inputMessenger.events.OnDrop.RemoveListener( targetOnDropHandler );
			}
		}


		/** On Drag functions. */
		private void targetOnDragHandler(GameObject target, PointerEventData eventData)
		{
			calulateDisposition( eventData );
			setupPositionDictionary();
			// updateListItemsDelta( disposition );
			// updateListWrapHandling( disposition );
		}

		private void calulateDisposition(PointerEventData eventData)
		{
			Vector2 pressPosition = eventData.pressPosition;
			Vector2 position = eventData.position;

			if( orientation == Orientation.Horizontal )
				disposition.x = position.x - pressPosition.x;
			else
				disposition.y = position.y - pressPosition.y;
		}

		private void setupPositionDictionary()
		{
			if( positions == null )
			{
				positions = new Dictionary<GameObject, Vector3>();

				for( int i = 0; i < list.Count; ++i )
				{
				    GameObject item = (GameObject)list[ i ];
					positions.Add( item, item.transform.localPosition );	    
				}
			}
		}


		/** Update delta value of list items. */
		private void updateListItemsDelta(Vector2 disposition)
		{	
			for( int i = 0; i < list.Count; ++i )
			{
			    GameObject item = (GameObject)list[ i ];

			    Vector3 localPosition = positions[ item ];

			    localPosition.x += disposition.x;
			    localPosition.y += disposition.y;

			    item.transform.localPosition = localPosition;
			}
		}


		/** Wrap items in list. */
		private void updateListWrapHandling(Vector2 disposition)
		{
			if( orientation == Orientation.Horizontal )
			{
				changeWrapIndexValue( disposition.x, distance.x, list.Count - 1, 0 );
			}	
			else
			if( orientation == Orientation.Vertical )
			{
				changeWrapIndexValue( disposition.y, distance.y, 0, list.Count - 1 );
			}
		}	

		private void changeWrapIndexValue(float value, float distance, int aIndex, int bIndex)
		{
			float direction = ( value / distance ) + wrapIndex;				

			if( direction >  1 )
			{
				changeWrapPositionValues( orientation, direction, aIndex, bIndex );
				wrapIndex--;

				events.wrapBeginning.Invoke( this );
			}
			else
			if( direction < -1 )
			{
				changeWrapPositionValues( orientation, direction, bIndex, aIndex );
				wrapIndex++;

				events.wrapEnd.Invoke( this );
			}
		}

		private void changeWrapPositionValues(Orientation orientation, float direction, int removeIndex, int insertIndex)
		{
			float add = direction > 0 ? 1 : -1;

			GameObject lastItem = (GameObject)list[ removeIndex ];
			Vector3 lastVector = positions[ lastItem ];

			GameObject firstItem = (GameObject)list[ insertIndex ];
			Vector3 firstVector = positions[ firstItem ];

			if( orientation == Orientation.Horizontal )
				lastVector.x = firstVector.x - distance.x * add;
			else
				lastVector.y = firstVector.y - distance.y * add;
			
			positions[ lastItem ] = lastVector;

			moveListItem( removeIndex, insertIndex );
		}

		private GameObject moveListItem(int removeIndex, int insertIndex)
		{
			GameObject removeItem = (GameObject)list[ removeIndex ];

			list.RemoveAt( removeIndex );
			list.Insert( insertIndex, removeItem );

			return removeItem;
		}


		/** Dispatch event on every update. */
		private void updateEventDispatch()
		{
			events.update.Invoke( this );
		}

		/** OnDrop functions. */
		// private void targetOnDropHandler(GameObject target, PointerEventData eventData)
		// {
		// 	// Reset();
		// }
	}
}