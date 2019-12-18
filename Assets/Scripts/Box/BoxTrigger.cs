using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
	private bool activeOnStart = false;
	private int colliderCount = 0;
	[SerializeField]
	private GameObject[] platforms = new GameObject[0];
	private Coroutine[] deactivatePlatformCoroutine;
	private AudioSource audioSource;


	private void Start()
	{
		deactivatePlatformCoroutine = new Coroutine[platforms.Length];
		audioSource = GetComponent<AudioSource>();

		for(int i = 0; i < platforms.Length; i++)
		{
			if(platforms[i].activeInHierarchy) // Set active animations to true in active platforms.
			{
				platforms[i].GetComponent<Animator>().SetBool("Active", true);
				activeOnStart = true;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.name.Equals("Player") || collider.name.StartsWith("Box") || collider.name.StartsWith("Enemy") || collider.name.StartsWith("DeathCollider"))
		{
			if(colliderCount == 0)
			{
				PressTrigger();
			}
			colliderCount++;
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.name.Equals("Player") || collider.name.StartsWith("Box") || collider.name.StartsWith("Enemy") || collider.name.StartsWith("DeathCollider"))
		{
			colliderCount--;
			if(colliderCount == 0)
			{
				UnpressTrigger();
			}
		}
	}

	private void PressTrigger()
	{
		transform.GetChild(0).position -= new Vector3(0, 0.1f);
		transform.GetChild(1).position -= new Vector3(0, 0.1f);
		audioSource.Play();
		for(int i = 0; i < platforms.Length; i++)
		{
			ActivePlatform(i, true);
		}
	}

	private void UnpressTrigger()
	{
		transform.GetChild(0).position += new Vector3(0, 0.1f);
		transform.GetChild(1).position += new Vector3(0, 0.1f);
		for(int i = 0; i < platforms.Length; i++)
		{
			ActivePlatform(i, false);
		}
	}

	private void ActivePlatform(int i, bool triggerPressed)
	{
		bool activate;
		if(triggerPressed)
		{
			activate = (!activeOnStart) ? true : false;
		}
		else
		{
			activate = (activeOnStart) ? true : false;
		}

		if(activate) // Activate platform.
		{
			if(deactivatePlatformCoroutine[i] != null)
			{
				StopCoroutine(deactivatePlatformCoroutine[i]);
			}
			platforms[i].SetActive(true);
			platforms[i].GetComponent<Animator>().SetBool("Active", true);
		}
		else // Deactivate platform.
		{
			platforms[i].GetComponent<Animator>().SetBool("Active", false);
			deactivatePlatformCoroutine[i] = StartCoroutine(DeactivatePlatform(platforms[i]));
		}
	}

	private IEnumerator DeactivatePlatform(GameObject go)
	{
		yield return new WaitForSeconds(0.5f);
		go.SetActive(false);
	}
}
