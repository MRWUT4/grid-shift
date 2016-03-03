using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;


namespace DavidOchmann
{
	[ System.Serializable ]
    public class InteractionUnityEvent : UnityEvent<GameObject>{}

	[ System.Serializable ]
	public class InteractionEvent
	{
		public InteractionUnityEvent mouseDown;
		public InteractionUnityEvent mouseUp;
		public InteractionUnityEvent mouseOver;
		public InteractionUnityEvent mouseOut;
	}

	public class Interaction : MonoBehaviour 
	{
		new public Collider2D collider2D;
		public InteractionEvent events;

		private bool isOver = false;
		private bool isDown = false;


		/**
		 * Event interface.
		 */

		public delegate void OnMouseDownEventHandler( MonoBehaviour monoBehaviour );
		public event OnMouseDownEventHandler OnMouseDown;

		protected virtual void InvokeMouseDown() 
	    {
	        if( OnMouseDown != null ) OnMouseDown( this );
	    }

		public delegate void OnMouseUpEventHandler( MonoBehaviour monoBehaviour );
		public event OnMouseUpEventHandler OnMouseUp;

	   	protected virtual void InvokeMouseUp() 
	    {
	        if( OnMouseUp != null ) OnMouseUp( this );
	    }

		public delegate void OnMouseOverEventHandler( MonoBehaviour monoBehaviour );
		public event OnMouseOverEventHandler OnMouseOver;

	   	protected virtual void InvokeMouseOver() 
	    {
	        if( OnMouseOver != null ) OnMouseOver( this );
	    }

		public delegate void OnMouseOutEventHandler( MonoBehaviour monoBehaviour );
		public event OnMouseOutEventHandler OnMouseOut;

	   	protected virtual void InvokeMouseOut() 
	    {
	        if( OnMouseOut != null ) OnMouseOut( this );
	    }


	    /**
	     * Public interface.
	     */

		void Start()
		{
			initVariables();
		}

		void Update() 
		{
			updateMouseInteraction();
		}


		public void MouseDown()
		{
			isDown = true;
			isOver = true;

			InvokeMouseDown();
			events.mouseDown.Invoke( gameObject );
		}

		public void MouseUp()
		{
			isDown = false;
			isOver = false;

			InvokeMouseUp();
			events.mouseUp.Invoke( gameObject );
		}

		public void MouseOver()
		{
			isOver = true;
			InvokeMouseOver();
			events.mouseOver.Invoke( gameObject );
		}

		public void MouseOut()
		{
			isOver = false;
			InvokeMouseOut();
			events.mouseOut.Invoke( gameObject );
		}


		/**
		 * Private interface.
		 */

		private void initVariables()
		{
			if( collider2D == null )
				collider2D = GetComponent<Collider2D>();
		}

		private void updateMouseInteraction()
		{
			Vector3 position = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			RaycastHit2D hit = Physics2D.Raycast( position, Vector2.zero );

			if( hit && hit.collider == collider2D )
			{
				if( Input.GetMouseButtonDown( 0 ) )
					MouseDown();
				else
				if( Input.GetMouseButtonUp( 0 ) )
					MouseUp();
				
				if( Input.GetMouseButton( 0 ) && !isOver && !isDown  )
					MouseOver();
			}
			else
			if( isOver )
				MouseOut();
		}
	}
}