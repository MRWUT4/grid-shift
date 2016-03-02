using System;
using System.Reflection;
using UnityEngine;

using Random = UnityEngine.Random;

namespace DavidOchmann.Animation
{
	public enum Presets{ none, shake }
	public enum Name{ localPosition, localRotation, localScale }
	public enum Direction{ x, y, z }

	[System.Serializable]
	public class Properties
	{
		public Name name;
		public Direction direction;
		public float height = 0;
		public float amplitude = 10f;
		public float phase = .001f;
		public float disposition = 0;
		public bool randomDisposition = false;

		public Properties Clone()
		{
			return (Properties)MemberwiseClone();
		}
	}

	[System.Serializable]
	public class Fade
	{
		public float duration = 0;
		public float delay = 0;

		[ HideInInspector ]
		public float value = 0;
	}

	public class SinTickAnimation : MonoBehaviour
	{
		public Presets presets;
		public Properties properties = new Properties();
		public Fade fade = new Fade();

		[ HideInInspector ]
		public float disposition;

		[ HideInInspector ]
		public Properties sourceProperties;

		private object propertyObject;
		private string propertyString;
		private string directionString;
		private int tick;

		private float _vector3Value = float.NaN;
		private float _quaternionValue = float.NaN;
		private DTween dTween;
		private Tween tween;


		/**
		 * Getter / Setter.
		 */

		public float vector3Value
		{
			get 
		    { 
				_vector3Value = !float.IsNaN( _vector3Value ) ? _vector3Value : (float)Tween.GetObjectValue( propertyObject, directionString );
		        return _vector3Value; 
		    }
		}

		
		public float quaternionValue
		{
			get 
		    { 
		        if( float.IsNaN( _quaternionValue ) )
		        {
		        	Vector3 vector3 = ( (Quaternion)propertyObject ).eulerAngles;
		        	_quaternionValue = (float)Tween.GetObjectValue( vector3, directionString );
		        }

		        return _quaternionValue; 
		    }
		}


		public float normal
		{
			get 
		    { 
		        return Mathf.Sin( 2 * Mathf.PI * disposition + tick * properties.phase * fade.value ); 
		    }
		}

		public float position
		{
			get 
		    { 
		        return properties.height + normal * properties.amplitude;
		    }
		}


		/**
		 * Public interface.
		 */

		public void Awake()
		{
			sourceProperties = properties.Clone();
		}

		public void Start()
		{
			initVariables();
			initFadeIn();

			updateObjectValue();
		}

		public void OnValidate()
		{
			changePresetValues();
		}

		public void FixedUpdate()
		{
			updateObjectValue();
			dTween.Update();
		}


		/**
		 * Private interface.
		 */

		/** Variables. */
		private void initVariables()
		{
			dTween = new DTween();

			propertyString = Enum.GetName( typeof( Name ), properties.name );
			directionString = Enum.GetName( typeof( Direction ), properties.direction );
			propertyObject = Tween.GetObjectValue( transform, propertyString );
			disposition = properties.randomDisposition ? Random.value : properties.disposition;
		}


		/** Change preset values. */
		private void changePresetValues()
		{
			// string name = Enum.GetName( typeof( Presets ), presets );

			switch( presets )
			{
				case Presets.shake:
					presets = Presets.none;
					properties.amplitude = .1f;
					properties.phase = 1 + Random.value * .1f;
					properties.randomDisposition = true;
					break;
			}
		}


		/** Init Fade in. */
		private void initFadeIn()
		{
			tween = dTween.To( fade, fade.duration, new { delay = fade.delay, value = 1 }, Quad.EaseInOut );
		}


		/** Update functions. */
		private void updateObjectValue()
		{
			// float normal = Mathf.Sin( 2 * Mathf.PI * disposition + tick * phase * fade );
			// float position = height + normal * amplitude;

			switch( properties.name )
			{
				case Name.localPosition:
				case Name.localScale:
					updateVector3();
					break;

				case Name.localRotation:
					updateQuaternion();
					break;
			}

			if( tween.start )
				tick++;
		}

		private void updateVector3()
		{
			if( transform != null )
			{
				float value = vector3Value + position;

				Vector3 vector3 = (Vector3)Tween.GetObjectValue( transform, propertyString );
				Vector3 vectorObject = (Vector3)propertyObject;

				vectorObject.x = vector3.x;
				vectorObject.y = vector3.y;
				vectorObject.z = vector3.z;

				propertyObject = Tween.SetObjectValue( vectorObject, directionString, value );
				Tween.SetObjectValue( transform, propertyString, propertyObject );
			}
		}

		private void updateQuaternion()
		{
			float value = quaternionValue + position;

			Vector3 angles = ( (Quaternion)propertyObject ).eulerAngles;
			Quaternion quaternion = Quaternion.Euler( angles.x, angles.y, value );

			Tween.SetObjectValue( transform, propertyString, quaternion );
		}
	}
}