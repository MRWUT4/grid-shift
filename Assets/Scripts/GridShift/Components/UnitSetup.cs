using UnityEngine;
    	
namespace GridShift
{
	public class UnitSetup : MonoBehaviour
	{
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

		public void Setup(int x, int y, object value)
		{
			Debug.Log( x + " " + y + " " + value );
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