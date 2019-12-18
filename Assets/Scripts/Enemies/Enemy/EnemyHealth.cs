using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	private int hp = 3;
	[SerializeField]
	private AudioSource audioSourceHit, audioSourceBox;
	[SerializeField]
	private GameObject bloodEffectPrefab, bloodSplashEffectPrefab, headTrigger;
	[SerializeField]
	private AudioClip hurtClip, deathClip;
	[SerializeField]
	private Animator spriteAnimator;
	[SerializeField]
	private Collider2D swordTrigger;
	private Coroutine hurtCoroutine;


	private void Start()
	{
		SetHP();
	}

	private void SetHP()
	{
		hp = 3 * (1+(int)Settings.Difficulty); // Easy = 3hp, Normal = 6hp, Hard = 9hp.
	}

	public void HitEnemy()
	{
		hp -= 1;

		audioSourceHit.clip = hurtClip;
		audioSourceHit.Play();

		spriteAnimator.SetBool("Hurt", true);
		spriteAnimator.Play("Hurt", 0, 0);
		if(hurtCoroutine != null)
		{
			StopCoroutine(hurtCoroutine);
		}
		hurtCoroutine = StartCoroutine(OffHurt());

		GameObject effect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
		Destroy(effect, 5);

		if(hp <= 0)
		{
			Die();
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag.Equals("Box"))
		{
			// Create splash effect:
			audioSourceBox.Play();
			GameObject effect = Instantiate(bloodSplashEffectPrefab, transform.position, Quaternion.identity);
			Destroy(effect, 5); // Destroy effect gameObject after 5s.

			// Unparent child object with audio & disable trigger:
			GameObject headTrigger = audioSourceBox.gameObject;
			headTrigger.transform.parent = null;
			headTrigger.GetComponent<Collider2D>().enabled = false;

			Destroy(headTrigger, 2); // Destroy gameObject with audio after 2s.
			Destroy(gameObject); // Destroy enemy gameObject.
		}
		else if(collider.name.Equals("Player") && !swordTrigger.enabled)
		{
			Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
			rb.velocity = new Vector2(rb.velocity.x, 0);
			rb.velocity += new Vector2(0, 5);

			collider.GetComponent<PlayerHealth>().SubtractHp(50);
			collider.GetComponents<AudioSource>()[1].Play();
			GameObject effect = Instantiate(bloodEffectPrefab, collider.transform.position, Quaternion.identity);
			Destroy(effect, 5);
		}
	}

	private void Die()
	{
		audioSourceHit.clip = deathClip;
		audioSourceHit.Play();
		spriteAnimator.Play("Dying", 0, 0);
		
		headTrigger.SetActive(false);
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(0, rb.velocity.y);

		GetComponent<Collider2D>().enabled = false;
		Destroy(GetComponent<Enemy>());
		swordTrigger.enabled = false;
		Destroy(GetComponent<EnemyHealth>());
	}

	private IEnumerator OffHurt()
	{
		yield return new WaitForSeconds(0.66f);
		spriteAnimator.SetBool("Hurt", false);
		spriteAnimator.Play("Idle", 0, 0);
		hurtCoroutine = null;
	}
}
