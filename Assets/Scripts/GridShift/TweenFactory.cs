using UnityEngine;
using DavidOchmann.Animation;
using DavidOchmann.Collections;

namespace GridShift
{
	public class TweenFactory
	{
		public static float duration = .6f;

		public static Tween DragListDisposition(DragList dragList, Vector2 vector2)
		{
			Tween dragListTween = new DTween().To( dragList.disposition, TweenFactory.duration * .5f, new 
			{ 
				x = vector2.x, 
				y = vector2.y 
			},
			Quad.EaseInOut );

			dragListTween.OnUpdate += delegate( Tween tween ){ dragList.disposition = (Vector2)tween.target; };

			return dragListTween;
		}
	}
}