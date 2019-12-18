using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UndergroundTrigger : MonoBehaviour
{
	private static bool isDying;

	[SerializeField]
	private AudioSource audioSource;
	[SerializeField]
	private GameObject bloodEffectPrefab;
	private Coroutine dieCoroutine;


	private void Start()
	{
		isDying = false;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.name.Equals("Player"))
		{
			if(!isDying)
			{
				isDying = true;
				GameObject player = collider.gameObject;
				Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
				rb.isKinematic = true;
				rb.velocity = Vector2.zero;
				player.GetComponent<PlayerMovement>().enabled = false;
				player.transform.Find("Character").GetComponent<Animator>().SetBool("Run", false);

				dieCoroutine = StartCoroutine(Die(player));
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.name.Equals("Player"))
		{
			isDying = false;
			GameObject player = collider.gameObject;
			Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
			rb.isKinematic = false;
			player.GetComponent<PlayerMovement>().enabled = true;

			if(dieCoroutine != null)
			{
				StopCoroutine(dieCoroutine);
			}
		}
	}

	private IEnumerator Die(GameObject player)
	{
		if(Settings.Difficulty != Settings.GameDifficulty.Hard) // In hard mode die instantly.
		{
			yield return new WaitForSeconds(0.75f);
		}

		PlayerHealth health = player.GetComponent<PlayerHealth>();
		health.Dead = true;
		health.SubtractHp(health.Hp);
		player.SetActive(false);

		if(audioSource != null)
		{
			audioSource.Play();
		}
		GameObject effect = Instantiate(bloodEffectPrefab, player.transform.position, Quaternion.identity);

		yield return new WaitForSeconds(2);
		SceneChanger.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);
	}
}
