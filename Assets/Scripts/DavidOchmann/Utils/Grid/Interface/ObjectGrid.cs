using UnityEngine;
using System.Collections.Generic;

namespace DavidOchmann.Grid
{
	public class Point
	{
		public int x;
		public int y;

		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return "(" + x.ToString() + ", " + y.ToString() + ")";
		}
	}

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

		public Point GetPosition(object value)
		{
			Point point = null;;

			ForEveryElementCall( delegate(int x, int y, object item)
			{
				if( value == item )
				{
					point = new Point( x, y );
					return;
				}
			});

			return point;
		}

		public List<object> GetColumn(int posX)
		{
			List<object> list = new List<object>(); 

			for( int y = 0; y < column.Count; ++y )
			{
			    List<object> row = column[ y ] as List<object>;
			    	
				object value = row[ posX ] as object;
				list.Add( value );
			}

			return list;
		}

		public List<object> GetRow(int posY)
		{
			List<object> list = new List<object>( (List<object>)column[ posY ] ); 
			return list;
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