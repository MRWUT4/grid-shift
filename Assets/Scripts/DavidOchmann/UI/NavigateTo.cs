using UnityEngine;
using UnityEngine.SceneManagement;
    	
namespace UI.DavidOchmann
{
	public class NavigateTo : MonoBehaviour
	{

		/**
		 * Public interface.
		 */

		public void URL(string url)
		{
			Application.OpenURL( url );
		}

		public void Scene(string id)
		{
			SceneManager.LoadScene( id );
		}
	}
}