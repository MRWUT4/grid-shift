using UnityEngine;
using DavidOchmann;

namespace DavidOchmann.UI
{
	[ RequireComponent( typeof( Interaction ) ) ]
	public class DragAndDrop : MonoBehaviour
	{
		[ HideInInspector ]
		public float rotation;
		
		[ HideInInspector ]
		public Vector3 vPosition;
		
		[ HideInInspector ]
		public Vector3 vMouse;

		[ Header( "Setup" ) ]
		public bool startOnMouseDown;
		public BoxCollider bounds;

		[ Header( "Position" ) ]
		public bool logMousePosition = false;
		public float positionSpeed = .06f;
		public Vector2 positionDisposition;

		[ Header( "Rotation" ) ]
		public float rotationSpeed = 1;
		public float rotationDisposition = 180;
		public float rotationMultiplier = 1;

		private Vector3 mousePosition;
		private Interaction interaction;
		private bool interactionMouseDown;
		// private bool mouseDown;


		/**
		 * Getter / Setter.
		 */

		public Vector3 inputMousePosition
		{
			get 
		    { 
				Vector3 vector3 = Camera.main.ScreenToWorldPoint( Input.mousePosition );
				
				if( bounds != null )
				{
					Vector2 size = bounds.size;
					Vector3 boundsCenter = Camera.main.ScreenToWorldPoint( bounds.bounds.center );
			        
					if( vector3.x < boundsCenter.x )
						vector3.x = boundsCenter.x;

					if( vector3.x > -boundsCenter.x )
						vector3.x = -boundsCenter.x;


					Vector3 center = bounds.center;

					float top = center.y + size.y * .5f;
					float bottom = center.y + -size.y * .5f;

					if( vector3.y > top )
						vector3.y = top;

					if( vector3.y < bottom )
						vector3.y = bottom;
				}

		        return vector3; 
		    }
		}

		public bool mouseDown
		{
			get 
		    { 
				bool _mouseDown = false;
				bool inputMouseDown = Input.GetMouseButton( 0 );

				if( inputMouseDown )
				{
					if( startOnMouseDown )
					{
						if( interactionMouseDown )
							_mouseDown = true;
					}
					else
						_mouseDown = true;
				}
				else
					interactionMouseDown = false;

		        return _mouseDown; 
		    }
		}
		

		/**
		 * Public interface.
		 */

		public void Start()
		{
			initInteractionComponent();
		}

		public void FixedUpdate()
		{
		    updateVariables();
			updatePositions();
			updateRotation();
		}


		/** Init Variables. */
		private void initInteractionComponent()
		{
			interaction = GetComponent<Interaction>();
			interaction.OnMouseDown += interactionOnMouseDownHandler;
		}

		private void interactionOnMouseDownHandler(MonoBehaviour monoBehaviour)
		{
			interactionMouseDown = true;
		}


		/** Update functions. */
		private void updateVariables()
		{
			if( logMousePosition )
				Debug.Log( inputMousePosition );

			if( mouseDown )
			{
			    vPosition = gameObject.transform.position;
			    
			    vMouse = inputMousePosition;
			    vMouse.z = vPosition.z;
			}
		}

		private void updatePositions()
		{
			if( vPosition != default( Vector3 ) )
			{
			    vPosition = (Vector3)Assist.FollowProperty( vPosition, "x", vMouse.x + positionDisposition.x, positionSpeed );
			    vPosition = (Vector3)Assist.FollowProperty( vPosition, "y", vMouse.y + positionDisposition.y, positionSpeed );

			    gameObject.transform.position = vPosition;
		    }
		}

		private void updateRotation()
		{
			if( vPosition != default( Vector3 ) )
			{
				float value = ( Calculator.DegreeBetweenTwoVectors( vPosition, vMouse ) + rotationDisposition ) * rotationMultiplier;
			    rotation = ( (DragAndDrop)Assist.FollowProperty( this, "rotation", value, rotationSpeed ) ).rotation;
			    
				Vector3 angles = gameObject.transform.rotation.eulerAngles;
				Quaternion quaternion = Quaternion.Euler( angles.x, angles.y, rotation );

			    gameObject.transform.rotation = quaternion;
			}
		}
	}
}