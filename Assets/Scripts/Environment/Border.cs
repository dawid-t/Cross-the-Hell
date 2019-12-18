using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Border : MonoBehaviour
{
	[SerializeField]
	private bool scream = false;
	private AudioSource audioSource;


	private void Awake()
	{
		if(scream)
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.volume = 0.2f;
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(scream) // Scream border.
		{
			if(collider.tag.Equals("Player"))
			{
				audioSource.Play();
				StartCoroutine(ScreamQuieter());
			}
		}
		else // Fail border.
		{
			if(collider.tag.Equals("Player"))
			{
				SceneChanger.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);
			}
			else if(collider.tag.Equals("Box") || collider.tag.Equals("Enemy"))
			{
				Destroy(collider.gameObject);
			}
		}
	}

	private IEnumerator ScreamQuieter()
	{
		int seconds = (int)(audioSource.clip.length * 50);
		for(int i = 0; i < seconds; i++)
		{
			audioSource.volume -= (float)1/(seconds*5);
			yield return new WaitForSecondsRealtime(0.02f);
		}
	}
}
