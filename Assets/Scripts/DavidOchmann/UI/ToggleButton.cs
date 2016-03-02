using UnityEngine;
using UnityEngine.UI;

namespace DavidOchmann.Components.UI
{
	public class ToggleButton : MonoBehaviour
	{
		public bool active = true;
		public GameObject[] gameObjectList;


		/**
		 * Public interface.
		 */

		public void Start()
		{
		}

		public void OnValidate()
		{
			changeButtonStates( active );
		}

		public void Toggle()
		{
			changeButtonStates( !active );
		}

		public void SetActive(bool boolean)
		{
			changeButtonStates( boolean );
		}


		/**
		 * Private interface.
		 */

		private void changeButtonStates(bool boolean)
		{
			active = boolean;

			if( gameObjectList != null )
			{
				for( int i = 0; i < gameObjectList.Length; ++i )
				{
				    GameObject gameObject = gameObjectList[ i ];
				    
				    if( gameObject != null )
				    {
					   	Button button = gameObject.GetComponent<Button>();
					   	button.interactable = active;
				   	}
				}
			}
		}
	}
}