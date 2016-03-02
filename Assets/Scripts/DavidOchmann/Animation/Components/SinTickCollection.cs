using UnityEngine;
    	
namespace DavidOchmann.Animation
{
	[ System.Serializable ]
	public class SinTickCollection : MonoBehaviour
	{
		[ Range( 0, 1 ) ]
		public float height = 1;
		
		[ Range( 0, 1 ) ]
		public float amplitude = 1;
		
		[ Range( 0, 1 ) ]
		public float phase = 1;

		[ Range( 0, 1 ) ]
		public float disposition = 1;

		private SinTickAnimation[] sinTickAnimationList;


		/**
		 * Public interface.
		 */

		public void Start()
		{
			UpdatePropertyValues();
		}

		public void FixedUpdate()
		{
			Start();
		}

		public void UpdatePropertyValues()
		{
			sinTickAnimationList = GetComponents<SinTickAnimation>();

			for( int i = 0; i < sinTickAnimationList.Length; ++i )
			{
			    SinTickAnimation sinTickAnimation = sinTickAnimationList[ i ];
			    
			    Properties sourceProperties = sinTickAnimation.sourceProperties;
			    Properties properties = sinTickAnimation.properties;

			    properties.height = sourceProperties.height * height;
			    properties.amplitude = sourceProperties.amplitude * amplitude;
			    properties.phase = sourceProperties.phase * phase;
			    properties.disposition = sourceProperties.disposition * disposition;
			}
		}
	}
}