using DavidOchmann.Events;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace DavidOchmann.Collections
{
	public enum Orientation{ Horizontal, Vertical }; 

	public class DragList
	{
		public GameObject target;
		public List<object> list;
		public Orientation orientation;
		public Dictionary<GameObject, Vector3> positions;
		public Vector2 distance;

		private Vector2 disposition;
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

		private void reset()
		{
			disposition = new Vector2();
			wrapIndex = 0;
		}

		public void kill()
		{
			initInputMessenger( false );
		}


		/**
		 * Private interface.
		 */

		private void init()
		{
			reset();

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
				inputMessenger.events.OnDrop.AddListener( targetOnDropHandler );
			}
			else
			{
				inputMessenger.events.OnDrag.RemoveListener( targetOnDragHandler );
				inputMessenger.events.OnDrop.RemoveListener( targetOnDropHandler );
			}
		}


		/** On Drag functions. */
		private void targetOnDragHandler(GameObject target, PointerEventData eventData)
		{
			calulateDisposition( eventData );
			setupPositionDictionary();
			moveListItemsOverDelta( disposition );
			wrapListItems( disposition );
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

		private void moveListItemsOverDelta(Vector2 disposition)
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

		private void wrapListItems(Vector2 disposition)
		{
			float value = ( disposition.x / distance.x ) + wrapIndex;

			// Debug.Log( value );

			if( value >  1 )
			{
				Debug.Log( value );
				wrapIndex--;
				Debug.Log( "toLeft " + wrapIndex );
			}
			else
			if( value < -1 )
			{
				wrapIndex++;
				Debug.Log( "toRight");
				
			}
		}

		/** OnDrop functions. */
		private void targetOnDropHandler(GameObject target, PointerEventData eventData)
		{
			reset();
		}
	}
}