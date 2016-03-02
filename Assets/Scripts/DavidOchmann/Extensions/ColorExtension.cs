using UnityEngine;

namespace DavidOchmann.Extensions
{
	public static class ColorExtension
	{
		public static Color HSB(this Color color, float hue, float saturation, float brightness, float alpha = 1 ) 
		{
			float r = 0;
			float g = 0; 
			float b = 0;


			if( saturation == 0 ) 
				r = g = b = (int) ( brightness * 255.0f + 0.5f );
			else 
			{
				float h = ( hue - (float) Mathf.Floor( hue ) ) * 6.0f;
				float f = h - (float) Mathf.Floor( h );
				float p = brightness * ( 1.0f - saturation );
				float q = brightness * ( 1.0f - saturation * f );
				float t = brightness * ( 1.0f - ( saturation * ( 1.0f - f ) ) );

				switch ((int) h) 
				{
					case 0:
						r = (int) (brightness * 255.0f + 0.5f);
						g = (int) (t * 255.0f + 0.5f);
						b = (int) (p * 255.0f + 0.5f);
						break;

					case 1:
						r = (int) (q * 255.0f + 0.5f);
						g = (int) (brightness * 255.0f + 0.5f);
						b = (int) (p * 255.0f + 0.5f);
						break;

					case 2:
						r = (int) (p * 255.0f + 0.5f);
						g = (int) (brightness * 255.0f + 0.5f);
						b = (int) (t * 255.0f + 0.5f);
						break;

					case 3:
						r = (int) (p * 255.0f + 0.5f);
						g = (int) (q * 255.0f + 0.5f);
						b = (int) (brightness * 255.0f + 0.5f);
						break;

					case 4:
						r = (int) (t * 255.0f + 0.5f);
						g = (int) (p * 255.0f + 0.5f);
						b = (int) (brightness * 255.0f + 0.5f);
						break;

					case 5:
						r = (int) (brightness * 255.0f + 0.5f);
						g = (int) (p * 255.0f + 0.5f);
						b = (int) (q * 255.0f + 0.5f);
						break;
				}
			}

			return new Color( r / 255, g / 255, b / 255, alpha );
		}
	}
}