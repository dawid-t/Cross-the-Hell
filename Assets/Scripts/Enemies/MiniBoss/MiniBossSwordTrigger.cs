using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossSwordTrigger : MonoBehaviour
{
	[SerializeField]
	private GameObject bloodEffectPrefab;
	private AudioSource audioSource;


	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		GameObject obj = collider.gameObject;
		if(obj.tag.Equals("Player"))
		{
			PlayerHealth health = obj.GetComponent<PlayerHealth>();
			if(health.Hp > 0)
			{
				health.SubtractHp(10000);
				obj.GetComponents<AudioSource>()[1].Play();
				//CameraShake.Instance.StartShake();
			}

			GameObject effect = Instantiate(bloodEffectPrefab, obj.transform.position, Quaternion.identity);
			Destroy(effect, 5);
		}
	}
}
