using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	private Vector3 direction = Vector3.zero;
	[SerializeField]
	private AudioSource fireballHitAudio;


	public Vector3 Direction { set { direction = value; } }


	private void Start()
	{
		Invoke("DestroyFireball", 5);
	}

	private void FixedUpdate()
	{
		transform.position += direction;
		//transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.name.StartsWith("Player"))
		{
			direction = Vector3.zero;
			GetComponent<Collider2D>().enabled = false;
			transform.parent = collider.transform;
			fireballHitAudio.Play();

			CancelInvoke();
			Invoke("StopFire", 3);

			collider.GetComponent<PlayerHealth>().SubtractHp(100);
			collider.GetComponents<AudioSource>()[1].Play();
			//GameObject effect = Instantiate(bloodEffectPrefab, collider.transform.position, Quaternion.identity);
			//Destroy(effect, 5);
		}
	}

	private void DestroyFireball()
	{
		Destroy(gameObject);
	}

	private void StopFire()
	{
		ParticleSystem ps = GetComponent<ParticleSystem>();
		var emission = ps.emission;
		emission.rateOverTime = 0;
		emission.rateOverDistance = 0;

		Destroy(gameObject, 5);
	}
}
