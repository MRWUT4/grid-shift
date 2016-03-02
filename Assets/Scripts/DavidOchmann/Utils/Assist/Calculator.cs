using UnityEngine;

public class Calculator
{
	public static float DegreeBetweenTwoVectors(Vector3 vector0, Vector3 vector1)
	{
		float dx = vector0.x - vector1.x;
		float dy = vector0.y - vector1.y;
	
		return Mathf.Atan2( dy, dx ) *  ( 180 / Mathf.PI );
	}

	public static float DistanceBetweenTwoVectos(Vector3 vector0, Vector3 vector1)
	{
		float distance = 0;

		float distanceX = Mathf.Pow( vector1.x - vector0.x, 2 );
		float distanceY = Mathf.Pow( vector1.y - vector0.y, 2 );
		
		distance = Mathf.Sqrt( distanceX + distanceY );

		return distance;
	}
}