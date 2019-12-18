using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject box;
	private Rigidbody2D boxRb;
	private BoxSpawnEffect boxSpawnEffect;


	private void Start()
	{
		boxSpawnEffect = box.GetComponent<BoxSpawnEffect>();
		boxRb = box.GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject == box)
		{
			/*if(!IsInvoking())
			{
				Invoke("TeleportBox", 2);
			}*/
			StopAllCoroutines();
			StartCoroutine(TeleportBox());
		}
	}

	private IEnumerator TeleportBox()
	{
		yield return new WaitForSeconds(2);
		boxSpawnEffect.RespawnEffect(); // Scale down box animation.

		yield return new WaitForSeconds(0.15f); // This is the animation time.
		boxRb.velocity = Vector2.zero;
		box.transform.position = transform.position; // Teleport the box.

		yield return new WaitForSeconds(0.15f); // This is the animation time.
		boxSpawnEffect.SpawnEffect(); // Scale up box animation.
	}
}
