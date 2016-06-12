using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//TODO: 

public class Spline_Catmull_Rom
{

	public static List<Vector3> GetPoints (Vector3 [] points, int numPoints)
	{
		//if (points.Length < 4)
			//throw new ArgumentException("CatmullRomSpline requires at least 4 points", "points");
		
		List<Vector3> splinePoints = new List<Vector3>();
		
		for (int i = 0; i < points.Length - 3; i++)
		{
			for (int j = 0; j < numPoints; j++)
			{
				splinePoints.Add(PointOnCurve(points[i], points[i + 1], points[i + 2], points[i + 3], (1f / numPoints) * j));
			}
		}
		
		splinePoints.Add(points[points.Length - 2]);
		
		return splinePoints;//.ToArray();
	}

	public static Vector3 PointOnCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		Vector3 ret = new Vector3();
		
		float t2 = t * t;
		float t3 = t2 * t;
		
		ret.x = 0.5f * ((2.0f * p1.x) +
		                (-p0.x + p2.x) * t +
		                (2.0f * p0.x - 5.0f * p1.x + 4 * p2.x - p3.x) * t2 +
		                (-p0.x + 3.0f * p1.x - 3.0f * p2.x + p3.x) * t3);
		
		ret.y = 0.5f * ((2.0f * p1.y) +
		                (-p0.y + p2.y) * t +
		                (2.0f * p0.y - 5.0f * p1.y + 4 * p2.y - p3.y) * t2 +
		                (-p0.y + 3.0f * p1.y - 3.0f * p2.y + p3.y) * t3);

		ret.z = 0.5f * ((2.0f * p1.z) +
		                (-p0.z + p2.z) * t +
		                (2.0f * p0.z - 5.0f * p1.z + 4 * p2.z - p3.z) * t2 +
		                (-p0.z + 3.0f * p1.z - 3.0f * p2.z + p3.z) * t3);		
		return ret;
	}

	public static Vector3 TangentOfCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		Vector3 Point;
		if(t+0.1f<1)
			Point = PointOnCurve(p0, p1, p2, p3, t+0.1f)-PointOnCurve(p0, p1, p2, p3, t);
		else
			Point = PointOnCurve(p0, p1, p2, p3, t)-PointOnCurve(p0, p1, p2, p3, t-0.1f);

		return Point.normalized;
	}

	public static float LengthofCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3,int samples)
	{
		float dt=1.0f/samples;
		float t=0;
		float distance=0;
		for(int i=0;i<samples;i++)
		{
			Vector3 pt1=PointOnCurve(p0, p1, p2, p3, t);
			t+=dt;
			if(t>1)
				t=1;
			Vector3 pt2=PointOnCurve(p0,p1,p2,p3,t);
			distance+=Vector3.Distance(pt1,pt2);
		}
		return distance;
	}
}

