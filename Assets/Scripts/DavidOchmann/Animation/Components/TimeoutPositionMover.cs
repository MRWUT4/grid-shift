
using UnityEngine;
    	
namespace DavidOchmann.Animation
{
	public class TimeoutPositionMover : MonoBehaviour
	{
		public BoxCollider2D boxCollider2D;
		public Vector2 scaleRange = new Vector2( .4f, 1 );
		public Vector2 timeoutRangeX = new Vector2( 1, 1 );
		public Vector2 timeoutRangeY = new Vector2( 1, 1 );
		public Vector2 minDistance = new Vector2( 0, 0 );
		public float lerpSpeedRotation = .6f;
		public float addRotation = 0;

		private FrameTimer frameTimer;
		private Vector3 positon;
		private Vector3 movePosition;

		private Vector2 _colliderOffset;
		private Vector3 _cameraSize;
		private DTween dTween;


		/**
		 * Getter / Setter.
		 */

		public Vector2 colliderSize
		{
			get 
		    { 
		        return boxCollider2D.bounds.size; 
		    }
		}

		public Vector2 colliderOffset
		{
			get 
		    { 
		        return boxCollider2D.offset; 
		    }
		}
		
		public float scale
		{
			get 
		    { 
		        return Random.Range( scaleRange.x, scaleRange.y ); 
		    }
		}

		public float xPosition
		{
			get 
		    { 
		        positon.x = colliderOffset.x + ( Random.value * colliderSize.x ) - ( Random.value * colliderSize.x );
		        float distance = Mathf.Abs( Calculator.DistanceBetweenTwoVectos( positon, gameObject.transform.localPosition ) );

		        return distance > minDistance.x ? positon.x : xPosition; 
		    }
		}

		public float yPosition
		{
			get 
		    { 
				positon.y = colliderOffset.y + ( Random.value * colliderSize.y ) - ( Random.value * colliderSize.y );
		        float distance = Mathf.Abs( Calculator.DistanceBetweenTwoVectos( positon, gameObject.transform.localPosition ) );
		        
		        return distance > minDistance.y ? positon.y : yPosition; 
		    }
		}


		/**
		 * Public interface.
		 */

		public void Start()
		{
			initVariables();
			startTweenX();
			startTweenY();
			// initFrameTimer();
		}

		public void FixedUpdate()
		{
			dTween.Update();
			// frameTimer.Update();
			// updateGameObjectPosition();
			updateGameObjectRotation();
		}


		private float GetTimeout(Vector2 vector2)
		{
			return Random.Range( vector2.x, vector2.y );
		}


		/**
		 * Private interface.
		 */

		/** Variables. */
		private void initVariables()
		{
			dTween = new DTween();
			positon = gameObject.transform.localPosition;
		}


		/** Position. */
		// private void updateGameObjectPosition()
		// {
		// 	Vector3 localPositionX = Vector3.Lerp( gameObject.transform.localPosition, positon, lerpSpeedPositionX );
		// 	Vector3 localPositionY = Vector3.Lerp( gameObject.transform.localPosition, positon, lerpSpeedPositionY );

		// 	movePosition.x = localPositionX.x;
		// 	movePosition.y = localPositionY.y;

		// 	gameObject.transform.localPosition = movePosition;
		// }

		private void updateGameObjectRotation()
		{
			Vector3 localPosition = gameObject.transform.localPosition;
			
			float value = Calculator.DegreeBetweenTwoVectors( localPosition, positon );

			Vector3 angles = gameObject.transform.rotation.eulerAngles;
			Quaternion quaternion = Quaternion.Euler( angles.x, angles.y, value + addRotation );

			gameObject.transform.localRotation = Quaternion.Lerp( gameObject.transform.localRotation, quaternion, lerpSpeedRotation );
		}


		/** FrameTimer. */
		// private void initFrameTimer()
		// {
		// 	frameTimer = new FrameTimer( timeout, float.NaN );
		// 	frameTimer.OnStep += frameTimerOnStepHandler;
		// 	frameTimer.Start();

		// 	frameTimerOnStepHandler( frameTimer );
		// }

		// private void frameTimerOnStepHandler(FrameTimer frameTimer)
		// {
		// 	frameTimer.seconds = timeout;
		// 	changeCurrentPosition();
		// }


		private void startTweenX(Tween value = null)
		{
			Tween tween = dTween.To( GetComponent<Mutate>(), GetTimeout( timeoutRangeX ), new { x = xPosition }, Sine.EaseInOut );
			tween.OnComplete += startTweenX;
		}

		private void startTweenY(Tween value = null)
		{
			float mutateScale = scale;

			Tween tween = dTween.To( GetComponent<Mutate>(), GetTimeout( timeoutRangeY ), new { y = yPosition, scaleX = mutateScale, scaleY = mutateScale }, Sine.EaseInOut );
			tween.OnComplete += startTweenY;
		}


		/** Position change functions. */
		// private void changeCurrentPosition()
		// {
		// 	positon.x = ( Random.value * cameraSize.x ) - ( Random.value * cameraSize.x );
		// 	positon.y = ( Random.value * cameraSize.y ) - ( Random.value * cameraSize.y );
		// }
	}
}