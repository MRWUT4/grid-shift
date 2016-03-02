using System;
using UnityEngine;


public class FrameTimer
{
	public static int FPS = (int)Math.Floor( 1 / Time.fixedDeltaTime );
	public delegate void OnCompleteDelegate(FrameTimer frameTimer);

	public float seconds;
	public float repeat;
	public OnCompleteDelegate onCompleteDelegate;

	public int currentStep;
	public int currentRepeat;
	public int totalSteps;
	public int currentTicks;
	public int totalTicks;
	public bool hasCompleted = true;


	public FrameTimer(float seconds, float repeat = 0, OnCompleteDelegate onCompleteDelegate = null)
	{
		this.seconds = seconds;
		this.repeat = repeat;
		this.onCompleteDelegate = onCompleteDelegate;
	}


	/**
	 * Event interface.
	 */

	public event OnChangeEventHandler OnChange;
	public delegate void OnChangeEventHandler( FrameTimer frameTimer );
	
	protected virtual void InvokeChange() 
	{
		if( OnChange != null ) OnChange( this );
	}


	public event OnStepEventHandler OnStep;
	public delegate void OnStepEventHandler( FrameTimer frameTimer );
	
	protected virtual void InvokeStep() 
	{
		if( OnStep != null ) OnStep( this );
	}


	public event OnCompleteEventHandler OnComplete;
	public delegate void OnCompleteEventHandler( FrameTimer frameTimer );
	
	protected virtual void InvokeComplete() 
	{
		if( OnComplete != null ) OnComplete( this );
	}


	/**
	 * Getter / Setter.
	 */

	public float currentTime
	{		
		get 
		{ 
			return ( seconds > 0 ? (float)currentTicks : (float)currentTicks * -1 ) / FPS; 
		}
	}

	public bool hasStarted
	{
		get 
		{ 
			return currentTicks > 0;
		}
	}

	public float progress
	{
		get 
	    { 
	        return (float)currentTicks / (float)totalTicks; 
	    }
	}


	/**
	 * Public interface.
	 */

	public void Start()
	{
		Reset();
	}

	public void Stop()
	{
		hasCompleted = true;
	}

	public void Reset(bool ignoreRepeat = false)
	{
		hasCompleted = false;
		totalTicks = (int)Math.Floor( ( float.IsNaN( seconds ) ? 0 : seconds ) * FPS );
		currentTicks = totalTicks;

		if( !ignoreRepeat )
		{
			totalSteps = 0;
			currentRepeat = (int)( repeat > 0 ? repeat - 1 : 0 );
		}
	}

	public void Update()
	{
		if( !hasCompleted )
		{
			currentTicks--;
			InvokeChange();

			if( !float.IsNaN( seconds ) )
			{
				if( currentTicks <= 0 )
				{
					hasCompleted = true;
					totalSteps++;

					InvokeStep();

					if( float.IsNaN( repeat ) || currentRepeat > 0 )
					{
						currentRepeat--;
						Reset( true );
					}
					else
					{
						InvokeComplete();
						
						if( onCompleteDelegate != null )
							onCompleteDelegate( this );
					}
				}
			}
		}
	}
}