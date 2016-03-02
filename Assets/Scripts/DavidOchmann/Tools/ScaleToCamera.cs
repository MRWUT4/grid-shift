using UnityEngine;
using System.Collections;

public enum Scale
{
   None,
   AutoUp,
   AutoDown,
   ToFit,
   ToHeight,
   ToWidth
}


[ RequireComponent (typeof (BoxCollider)) ]

public class ScaleToCamera : MonoBehaviour
{
	public Scale scale;

	private Scale scaleCompare;
	private Vector3 _colliderSize;
	private Vector3 _cameraSize;
	private Vector3 cameraSizeCompare;
	


	/**
	 * Getter / Setter.
	 */

	public Vector3 colliderSize
	{
		get 
	    { 
	    	if( _colliderSize == default( Vector3 ) )
	    	{
		    	BoxCollider boxCollider = GetComponent<BoxCollider>();
		        _colliderSize = boxCollider.bounds.size;

		        _colliderSize.x /= transform.localScale.x;
		        _colliderSize.y /= transform.localScale.y;
	        }

	        return _colliderSize; 
	    }
	}

	public Vector3 cameraSize
	{
		get 
	    { 
	    	float height = Camera.main.orthographicSize * 2f;
	    	float width = height * ( (float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight );

			_cameraSize.x = width;
			_cameraSize.y = height;
			_cameraSize.z = 1;

	        return _cameraSize; 
	    }
	}
	
	public Vector3 scaleToFit
	{
		get 
	    { 
	    	float x = cameraSize.x / colliderSize.x;
	    	float y = cameraSize.y / colliderSize.y;

	        return new Vector3( x, y, 1 ); 
	    }
	}

	public Vector3 scaleToHeight
	{
		get 
	    { 
	    	float y = cameraSize.y / colliderSize.y;
	        return new Vector3( y, y, 1 ); 
	    }
	}

	public Vector3 scaleToWidth
	{
		get 
	    { 
	    	float x = cameraSize.x / colliderSize.x;
	        return new Vector3( x, x, 1 ); 
	    }
	}

	public Vector3 scaleToAutoUp
	{
		get 
	    { 
	    	float ratio = cameraSize.x / cameraSize.y;
	        return ratio >= 1 ? scaleToWidth : scaleToHeight; 
	    }
	}
	
	public Vector3 scaleToAutoDown
	{
		get 
	    { 
	    	float ratio = cameraSize.x / cameraSize.y;
	        return ratio <= 1 ? scaleToWidth : scaleToHeight; 
	    }
	}



	/**
	 * Public interface.
	 */

	public void Start()
	{
		updateSizeChange();
	}

	public void OnValidate()
	{
		Start();
	}


	/**
	 * Private interface.
	 */

	/** Update functions. */
	private void updateSizeChange()
	{
		bool cameraChanged = cameraSize.x != cameraSizeCompare.x || cameraSize.y != cameraSizeCompare.y;
		bool scaleChanged = scale != scaleCompare;

		if( cameraChanged || scaleChanged )
		{
			scaleCompare = scale;
			cameraSizeCompare = cameraSize;	
			
			initScaleTansform();
		}
	}


	/** Transform functions. */
	private void initScaleTansform()
	{
		switch( scale )
		{
			case Scale.None:
				transform.localScale = new Vector3( 1, 1, 1 );
				break;

			case Scale.AutoUp:
				transform.localScale = scaleToAutoUp;
				break;

			case Scale.AutoDown:
				transform.localScale = scaleToAutoDown;
				break;

			case Scale.ToFit:
				transform.localScale = scaleToFit;
				break;

			case Scale.ToWidth:
				transform.localScale = scaleToWidth;
				break;

			case Scale.ToHeight:
				transform.localScale = scaleToHeight;
				break;
		}
	}
}