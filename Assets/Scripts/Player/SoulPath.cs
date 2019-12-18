using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPath : MonoBehaviour
{
	[SerializeField]
	private Transform[] curves;


	private void OnEnable()
	{
		StartCoroutine(SpawnSoul());
	}

	private IEnumerator SpawnSoul()
	{
		#region Add all points to the list:
		List<Vector3> points = new List<Vector3>();
		for(int i = 0; i < curves.Length; i++)
		{
			for(int j = 0; j < curves[i].childCount; j++)
			{
				points.Add(curves[i].GetChild(j).position);
			}
		}
		#endregion  Add all points to the list.

		#region Soul flies through the points:
		Vector3 p1, p2;
		for(int i = 0; i < points.Count/2; i++)
		{
			p1 = points[i*2];
			p2 = points[i*2 + 1];
			for(int j = 0; j < 40; j++)
			{
				Vector3 curvePoint = GetCurvePoint((j+1)/40f, transform.position, p1, p2);
				transform.position = Vector3.Lerp(transform.position, curvePoint, 0.125f);
				yield return new WaitForSeconds(0.005f);
			}
		}
		

		p1 = points[points.Count-1];
		p2 = p1;
		for(int i = 0; i < 20; i++)
		{
			Vector3 curvePoint = GetCurvePoint((i+1)/20f, transform.position, p1, p2);
			transform.position = Vector3.Lerp(transform.position, curvePoint, 0.125f);
			yield return new WaitForSeconds(0.005f);
		}
		transform.position = p2;
		#endregion Soul flies through the points.

		#region Soul is disappearing
		for(int i = 0; i < 20; i++)
		{
			transform.localScale -= new Vector3(0.01f, 0.01f, 0.011f);
			yield return new WaitForSeconds(0.02f);
		}
		Destroy(gameObject, 5);
		#endregion Soul is disappearing.
	}

	private Vector3 GetCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) // Quadratic Bezier curve.
	{
		return (1 - t)*(1 - t)*p0 + 2*(1 - t)*t*p1 + t*t*p2;
	}
}
