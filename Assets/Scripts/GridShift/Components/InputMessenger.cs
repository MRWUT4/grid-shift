using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DavidOchmann.Events
{
	[ System.Serializable ]
	public class InputMessengerUnityEvent : UnityEvent<GameObject, PointerEventData>{}

	[ System.Serializable ]
	public class InputMessengerEvents
	{
		public InputMessengerUnityEvent OnPointerDown;
		public InputMessengerUnityEvent OnDrag;
		public InputMessengerUnityEvent OnDrop;
	}

	public class InputMessenger : MonoBehaviour, IPointerDownHandler, IDragHandler, IDropHandler
	{
		public InputMessengerEvents events;


		/**
		 * Interface methods.
		 */

		public void OnPointerDown(PointerEventData eventData)
		{
			events.OnPointerDown.Invoke( gameObject, eventData );
		}

		public void OnDrag(PointerEventData eventData)
		{
			events.OnDrag.Invoke( gameObject, eventData );
		}

		public void OnDrop(PointerEventData eventData)
		{
			events.OnDrop.Invoke( gameObject, eventData );
		}
	}
}