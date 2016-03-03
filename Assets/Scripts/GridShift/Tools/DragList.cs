using DavidOchmann.Events;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace DavidOchmann.Collections
{
	public enum Direction{ Horizontal, Vertical }; 

	public class DragList
	{
		public GameObject target;
		public List<object> list;
		public Direction direction;

		private Vector2 disposition;
		private InputMessenger inputMessenger;
		public Dictionary<GameObject, Vector3> positions;


		public DragList(GameObject target, List<object> list, Direction direction)
		{
			this.target = target;
			this.list = list;
			this.direction = direction;

			init();
		}


		/**
		 * Public interface.
		 */

		private void reset()
		{
			disposition = new Vector2();
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
		}

		private void calulateDisposition(PointerEventData eventData)
		{
			Vector2 pressPosition = eventData.pressPosition;
			Vector2 position = eventData.position;

			if( direction == Direction.Horizontal )
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

		private void moveListItemsOverDelta(Vector2 delta)
		{
			for( int i = 0; i < list.Count; ++i )
			{
			    GameObject item = (GameObject)list[ i ];
			    Vector3 localPosition = positions[ item ];

			    localPosition.x += delta.x;
			    localPosition.y += delta.y;

			    item.transform.localPosition = localPosition;
			}
		}

		/** OnDrop functions. */
		private void targetOnDropHandler(GameObject target, PointerEventData eventData)
		{
			reset();
		}
	}
}