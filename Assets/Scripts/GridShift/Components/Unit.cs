using UnityEngine;
using UnityEngine.UI;
using DavidOchmann.Events;

namespace GridShift
{
	public class Unit : MonoBehaviour
	{
		public Text[] texts;
		
		private int _value;
		private bool _active = true;
		private CanvasGroup canvasGroup;
		// private InputMessenger inputMessenger;


		/**
		 * Getter / Setter.
		 */

		public int value
		{
			get 
		    { 
		        return _value; 
		    }
		
		    set
		    { 
		        _value = value; 

		        for( int i = 0; i < texts.Length; ++i )
		        {
		            Text text = texts[ i ];
		        	text.text = _value.ToString();
		        }
		    }
		}	


		public bool active
		{
			get 
		    { 
		        return _active; 
		    }
		
		    set
		    { 
		        _active = value;
	        	// inputMessenger.enabled = _active;
		    }
		}


		/**
		 * Public.
		 */

		/** MonoBahaviour implementation. */
		public void Awake()
		{
			initVariables();
		}

		public void FixedUpdate()
		{
			
		}


		/**
		 * Private interface.
		 */

		/** Init variables. */
		private void initVariables()
		{
			// inputMessenger = gameObject.GetComponent<InputMessenger>();
		}
	}
}