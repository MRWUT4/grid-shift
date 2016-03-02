using UnityEngine;
using System.Collections.Generic;

namespace DavidOchmann.Grid
{
	public class ObjectGrid
	{
		public delegate void PositionDelegate(int x, int y, object value);

		public int width;
		public int height;
		public object defaultValue;

		public List<object> column;


		public ObjectGrid(int width, int height, object defaultValue = null)
		{
			this.width = width;
			this.height = height;
			this.defaultValue = defaultValue;

			initWithWidthAndHeight();
		}


		/**
		 * Public functions.
		 */

		public void ForEveryElementCall(PositionDelegate callback)
		{
			for( int y = 0; y < column.Count; ++y )
			{
			    List<object> row = column[ y ] as List<object>;
			    	
			    for( int x = 0; x < row.Count; ++x )
			    {
			        object value = row[ x ] as object;
					callback( x, y, value );
			    }
			}
		}

		public void Set(int x, int y, object value)
		{
			List<object> row = column[ y ] as List<object>;
			row[ x ] = value; 
		}



		/**
		 * Private functions.
		 */

		/** Instantiate nested List. */
		private void initWithWidthAndHeight()
		{
			column = new List<object>();

			for( int y = 0; y < height; ++y )
			{
				List<object> row = new List<object>();
				column.Add( row );
			    
			    for( int x = 0; x < width; ++x )
			        row.Add( defaultValue );
			}
		}
	}
}