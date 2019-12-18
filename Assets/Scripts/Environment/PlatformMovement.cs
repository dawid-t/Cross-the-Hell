using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
	private bool goBack = false;
	[SerializeField]
	private Vector2 destinationPosition = new Vector3(0, 0);
	private Vector2 originalPosition;


	private void Awake()
	{
		originalPosition = transform.position;
	}

	private void OnEnable()
	{
		StartCoroutine(Move());
	}

	private IEnumerator Move()
	{
		Vector3 destination = (!goBack) ? destinationPosition : originalPosition;
		while(transform.position != destination)
		{
			Vector3 newPosition = Vector3.Lerp(transform.position, destination, 0.125f);
			float magnitude = (newPosition - transform.position).magnitude;
			if(magnitude > 0.05f)
			{
				transform.position += (newPosition - transform.position).normalized/20; // Max position shift is 0.05f.
			}
			else
			{
				transform.position = newPosition; // Position shift less than 0.05f.
			}
			yield return new WaitForSeconds(0.02f);
		}

		goBack = !goBack;
		StartCoroutine(Move());
	}
}
