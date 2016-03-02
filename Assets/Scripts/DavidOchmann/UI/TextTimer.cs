using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
    	
namespace DavidOchmann.UI
{
	public enum Format{ minutesAndSeconds, seconds };

	[ System.Serializable ]
	public class TextTimerEvents
	{
		public UnityEvent onStep;
		public UnityEvent onComplete;
	}

	[ RequireComponent ( typeof (UnityEngine.UI.Text) ) ]
	public class TextTimer : MonoBehaviour
	{
		public float seconds = 80;
		public Format format;
		public TextTimerEvents events;


		private FrameTimer frameTimer;
		private Text text;


		/**
		 * Public interface.
		 */

		public void Start()
		{
			initVariables();
			initFrameTimer();

			UpdateTextDisplay();
		}

		public void FixedUpdate()
		{
			frameTimer.Update();
		}

		public void OnValidate()
		{
			Start();
		}


		/**
		 * Private interface.
		 */

		/** Variables. */
		private void initVariables()
		{
			text = GetComponent<Text>();	
		}


		/** FrameTimer functions. */
		private void initFrameTimer()
		{
			frameTimer = new FrameTimer( 1, seconds );
			frameTimer.OnStep += frameTimerOnStepHandler;
			frameTimer.OnComplete += frameTimerOnCompleteHandler;
			frameTimer.Start();
		}

		private void frameTimerOnStepHandler(FrameTimer frameTimer)
		{
			seconds--;
			UpdateTextDisplay();

			events.onStep.Invoke();
		}

		private void frameTimerOnCompleteHandler(FrameTimer frameTimer)
		{
			events.onComplete.Invoke();
		}


		/** Update functions. */

		private void UpdateTextDisplay()
		{
			switch( format )
			{
				case Format.minutesAndSeconds:
					UpdateTextDisplayInMinutes ();
					break;

				case Format.seconds:
					UpdateTextDisplayInSeconds ();
					break;
			}
		}

		private void UpdateTextDisplayInMinutes()
		{
			string minutesString = ( "0" + ( Mathf.Floor( seconds / 60 ) ).ToString() );
			string secondsString = ( "0" + ( seconds % 60 ).ToString() );
			string displayText = minutesString.Substring( minutesString.Length - 2, 2 ) + ":" + secondsString.Substring( secondsString.Length - 2, 2 );

			text.text = displayText;
		}

		private void UpdateTextDisplayInSeconds()
		{
			string displayText = seconds.ToString();
			text.text = displayText;
		}
	}
}