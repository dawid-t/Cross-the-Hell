using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Traps : MonoBehaviour
{
	[SerializeField]
	private GameObject bloodEffectPrefab;


	private void OnTriggerStay2D(Collider2D collider)
	{
		GameObject obj = collider.gameObject;
		if(obj.tag.Equals("Player"))
		{
			PlayerHealth health = obj.GetComponent<PlayerHealth>();
			if(!health.Immunity)
			{
				health.SetTemporaryImmunity(true);
				health.SubtractHp(200);
				
				obj.GetComponents<AudioSource>()[1].Play();
				GameObject effect = Instantiate(bloodEffectPrefab, obj.transform.position, Quaternion.identity);
				Destroy(effect, 5);
			}
		}
	}
}
