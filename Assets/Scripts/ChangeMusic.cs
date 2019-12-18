using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
	private AudioSource audioSource;
	[SerializeField]
	private AudioClip finalClip;


	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayFinalMusic()
	{
		StartCoroutine(Change());
	}

	private IEnumerator Change()
	{
		while(audioSource.volume > 0)
		{
			yield return new WaitForSeconds(0.1f);
			audioSource.volume -= 0.01f;
		}
		audioSource.clip = finalClip;
		audioSource.Play();

		while(audioSource.volume < 0.05f)
		{
			yield return new WaitForSeconds(0.1f);
			audioSource.volume += 0.01f;
		}
		audioSource.volume = 0.05f;
	}
}
