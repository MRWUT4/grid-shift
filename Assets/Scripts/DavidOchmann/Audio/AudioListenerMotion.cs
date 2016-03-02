using UnityEngine;
using DavidOchmann.Animation;
    	
namespace DavidOchmann
{
	[ RequireComponent ( typeof( AudioListener ) ) ]

	public class AudioListenerMotion : MonoBehaviour
	{
		public float duration = 1;
		public float initalVolume = 0;

		private DTween dTween;

		
		public float volume
		{
			get 
		    { 
		        return AudioListener.volume; 
		    }
		
		    set
		    { 
		        AudioListener.volume = value; 
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
			dTween.Update();	
		}

		public void FadeVolume(float volume)
		{
			dTween.To( this, duration, new { volume = volume }, Linear.EaseNone );	
		}

		

		/**
		 * Private interface.
		 */

		private void initVariables()
		{
			dTween = new DTween( true );
			volume = initalVolume;
		}
	}
}