using System;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace DavidOchmann.Animation
{
	[Serializable]
	public class Mutate : MonoBehaviour
	{	
		public static string COLOR_NAME = "color";

		private Vector3 localPosition;
		private Vector3 localScale;
		private Quaternion localRotation;
		
		private Component _colorComponent;
		private Color _color;


		/**
		 * Override interface.
		 */

		public void Start()
		{
			initVariables();
		}


		/**
		 * Getter / Setter.
		 */

		/** localPosition */
		public float x
		{
			set 
			{ 
				localPosition.x = value;
				localPosition.y = y;
				localPosition.z = z;
				gameObject.transform.localPosition = localPosition;
			}
			
			get 
			{ 
				return gameObject.transform.localPosition.x; 
			}
		}

		public float y
		{
			set 
			{ 
				localPosition.x = x;
				localPosition.y = value;
				localPosition.z = z;
				gameObject.transform.localPosition = localPosition;
			}
			
			get 
			{ 
				return gameObject.transform.localPosition.y; 
			}
		}

		public float z
		{
			set 
			{ 
				localPosition.x = x;
				localPosition.y = y;
				localPosition.z = value;
				gameObject.transform.localPosition = localPosition;
			}
			
			get 
			{ 
				return gameObject.transform.localPosition.z; 
			}
		}


		/** localScale */
		public float scaleX
		{
			set 
			{ 
				localScale.x = value;
				localScale.y = scaleY;
				localScale.z = scaleZ;
				gameObject.transform.localScale = localScale;
			}
			
			get 
			{ 
				return gameObject.transform.localScale.x; 
			}
		}

		public float scaleY
		{
			set 
			{ 
				localScale.x = scaleX;
				localScale.y = value;
				localScale.z = scaleZ;
				gameObject.transform.localScale = localScale;
			}
			
			get 
			{ 
				return gameObject.transform.localScale.y; 
			}
		}

		public float scaleZ
		{
			set 
			{ 
				localScale.x = scaleX;
				localScale.y = scaleY;
				localScale.z = value;
				gameObject.transform.localScale = localScale;
			}
			
			get 
			{ 
				return gameObject.transform.localScale.z; 
			}
		}
		

		/** localRotation */
		public float rotationX
		{
			get 
		    { 
		        return localRotation.eulerAngles.x; 
		    }
		
		    set
		    { 	
				Vector3 angles = ( transform.localRotation ).eulerAngles;
				Quaternion quaternion = Quaternion.Euler( value, angles.y, angles.z );

				transform.localRotation = quaternion;
		    }
		}

		public float rotationY
		{
			get 
		    { 
		        return localRotation.eulerAngles.y;
		    }
		
		    set
		    { 	
				Vector3 angles = ( transform.localRotation ).eulerAngles;
				Quaternion quaternion = Quaternion.Euler( angles.x, value, angles.z );

				transform.localRotation = quaternion;
		    }
		}

		public float rotationZ
		{
			get 
		    { 
		        return localRotation.eulerAngles.y; 
		    }
		
		    set
		    { 	
				Vector3 angles = ( transform.localRotation ).eulerAngles;
				Quaternion quaternion = Quaternion.Euler( angles.x, angles.y, value );

				transform.localRotation = quaternion;
		    }
		}


		/** color */	
		public Component colorComponent
		{
			get 
		    { 
		    	if( _colorComponent == null )
		    	{
		        	_colorComponent = (Component)GetComponent<SpriteRenderer>();

		        	if( _colorComponent == null )
		        		_colorComponent = (Component)GetComponent<Image>();
		        }

		        return _colorComponent;
		    }
		}

		public Color color
		{
			get 
		    { 
		        if(  colorComponent != null )
		        	_color = (Color)Tween.GetObjectValue( colorComponent, COLOR_NAME );

		        return _color; 
		    }

			set
			{ 
				Assist.SetObjectValue( colorComponent, "color", value );
			}
		}

		public float alpha
		{
			get 
		    { 
		        return color.a; 
		    }
		
		    set
		    { 
		    	Color componentColor = color;

		    	componentColor.a = value;
		    	color = componentColor;
		    }
		}


		/**
		 * Private interface.
		 */

		private void initVariables()
		{
			localPosition = gameObject.transform.localPosition;
			localScale = gameObject.transform.localScale;
			localRotation = gameObject.transform.localRotation;
			localRotation = gameObject.transform.localRotation;
		}
	}
}