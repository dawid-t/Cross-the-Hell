using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSound : MonoBehaviour
{
	private AudioSource audioSource;


	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.relativeVelocity.magnitude >= 2 && !audioSource.isPlaying)
		{
			if(collision.relativeVelocity.magnitude >= 10) // 100% volume.
			{
				audioSource.volume = 0.5f;
				audioSource.pitch = 1;
			}
			else // 2% - 99.9% volume.
			{
				audioSource.volume = collision.relativeVelocity.magnitude * 0.05f;
				audioSource.pitch = 2.0f - collision.relativeVelocity.magnitude / 10f;
			}
			audioSource.Play();
		}
	}
}
