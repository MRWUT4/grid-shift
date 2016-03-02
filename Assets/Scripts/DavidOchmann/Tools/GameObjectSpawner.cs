using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
    	
namespace DavidOchmann
{
	[ System.Serializable ]
    public class GameObjectSpawnerUnityEvent : UnityEvent<GameObject>{}

	[ System.Serializable ]
	public class GameObjectSpawnerEvents
	{	
		public GameObjectSpawnerUnityEvent spawn;
	}

	public class GameObjectSpawnerVO 
	{
		public GameObject gameObject;
		public FrameTimer frameTimer;
		public float lifetime;


		/**
		 * Event Interface.
		 */

		public event OnLifetimeEndEventHandler OnLifetimeEnd;
		public delegate void OnLifetimeEndEventHandler( GameObjectSpawnerVO gameObjectSpawnerVO );
		
		protected virtual void InvokeLifetimeEnd() 
		{
			if( OnLifetimeEnd != null ) OnLifetimeEnd( this );
		}


		/**
		 * Public Interface.
		 */

		public GameObjectSpawnerVO(GameObject gameObject, float lifetime)
		{
			this.gameObject = gameObject;
			this.lifetime = lifetime;

			initFrameTimer();
		}

		public void Update()
		{
			if( frameTimer != null )
				frameTimer.Update();
		}


		/**
		 * Private interface.
		 */

		private void initFrameTimer()
		{
			if( lifetime != 0 )
			{
				frameTimer = new FrameTimer( lifetime );
				frameTimer.OnComplete += frameTimerOnCompleteHandler;
				frameTimer.Start();
			}
		}

		private void frameTimerOnCompleteHandler(FrameTimer frameTimer)
		{
			InvokeLifetimeEnd();
		}
	}


	public class GameObjectSpawner : MonoBehaviour
	{
		new public GameObject gameObject;
		public GameObject container;
		public bool inheritTransform = true;
		public bool updateSpawn = true;
		public int repeat = 5;
		public float lifetime = 0;
		public Vector2 timeoutRange = new Vector2( 1, 1 );
		public GameObjectSpawnerEvents events;
		// public float minTimeout = 0; 
		// public float maxTimeout = 0; 

		[ HideInInspector ]
		private float timeout;

		public List<GameObjectSpawnerVO> voList = new List<GameObjectSpawnerVO>();
		
		private FrameTimer frameTimer;


		/**
		 * Event interface.
		 */

		public event OnSpawnEventHandler OnSpawn;
		public delegate void OnSpawnEventHandler( GameObject gameObject );
		
		protected virtual void InvokeSpawn(GameObject gameObject) 
		{
			if( OnSpawn != null ) OnSpawn( gameObject );
		}


		/**
		 * Public interface.
		 */

		public void Start()
		{
			initFrameTimer();
		}

		public void SetUpdateSpawn(bool boolean)
		{
			updateSpawn = boolean;
		}

		public void FixedUpdate()
		{
			if( updateSpawn )
				frameTimer.Update();

			UpdateVOList();
		}


		/**
		 * Private interface.
		 */

		private void initFrameTimer()
		{
			frameTimer = new FrameTimer( timeoutRange.x, repeat == 0 ? float.NaN : repeat );
			frameTimer.OnStep += frameTimerOnStepHandler;

			initFrameTimerTimeout();
			
			frameTimer.Start();
		}

		private void initFrameTimerTimeout()
		{
			frameTimer.seconds = Random.Range( timeoutRange.x, timeoutRange.y );
		}

		private void frameTimerOnStepHandler(FrameTimer frameTimer)
		{
			initFrameTimerTimeout();
			
			GameObject clone = createGameObjectClone();

			InvokeSpawn( clone );
			events.spawn.Invoke( clone );
		}


		/** VOList functions. */
		private void UpdateVOList()
		{
			for( int i = 0; i < voList.Count; ++i )
			{
			    GameObjectSpawnerVO vo = voList[ i ];
			    vo.Update();
			}
		}

		private void RemoveVOFromList(GameObjectSpawnerVO vo)
		{
			for( int i = voList.Count - 1; i >= 0; --i )
			{
			    GameObjectSpawnerVO item = voList[ i ];
			    
			    if( item == vo )
			    {
			    	voList.RemoveAt( i );
			    	Object.Destroy( item.gameObject );
			    }
			}
		}


		/** GameObjectClone functions. */
		private GameObject createGameObjectClone()
		{
			GameObject clone = Assist.GetGameObjectClone( gameObject );
			
			clone.transform.parent = container.transform;
			
			if( inheritTransform )
				clone.transform.position = transform.position;

			GameObjectSpawnerVO vo = new GameObjectSpawnerVO( clone, lifetime );
			vo.OnLifetimeEnd += voOnLifetimeEndHandler;

			voList.Add( vo );

			return clone;
		}

		private void voOnLifetimeEndHandler(GameObjectSpawnerVO vo)
		{
			RemoveVOFromList( vo );
		}
	}
}