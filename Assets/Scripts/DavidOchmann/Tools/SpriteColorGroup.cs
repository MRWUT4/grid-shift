using UnityEngine;
    	
namespace DavidOchmann.Tools
{
	public class SpriteColorGroup : MonoBehaviour
	{
		public Color color;
		// private SpriteRenderer[] spriteRederers;


		/**
		 * Public interface.
		 */

		public void Start()
		{
			// changeSpriteRenderersColor();
		}

		public void OnValidate()
		{
			changeSpriteRenderersColor();
		}


		/**
		 * Private interface.
		 */

		private void changeSpriteRenderersColor()
		{
			SpriteRenderer[] spriteRederers = GetComponentsInChildren<SpriteRenderer>( true );

			for( int i = 0; i < spriteRederers.Length; ++i )
			{
			    SpriteRenderer spriteRenderer = spriteRederers[ i ];
			    spriteRenderer.color = color;
			}
		}
	}
}