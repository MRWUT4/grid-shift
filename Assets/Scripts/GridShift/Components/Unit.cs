using UnityEngine;
using UnityEngine.UI;

namespace GridShift
{
	public class Unit : MonoBehaviour
	{
		public Text text;
		
		private int _value;


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

		        text.text = _value.ToString();
		    }
		}	


		/**
		 * Public interface.
		 */

		public void Start()
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
			
		}
	}
}