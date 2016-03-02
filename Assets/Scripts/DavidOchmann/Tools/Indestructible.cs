using UnityEngine;
    	
namespace DavidOchmann.Tools
{
	public class Indestructible : MonoBehaviour
	{
		public static Indestructible instance;


		/**
		 * Public interface.
		 */

		public void Awake () 
		{
			if( instance == null ) 
			{
				instance = this;
				DontDestroyOnLoad( gameObject );
			}
			else 
			{
				Object.Destroy( gameObject );
			}
		}
	}
}