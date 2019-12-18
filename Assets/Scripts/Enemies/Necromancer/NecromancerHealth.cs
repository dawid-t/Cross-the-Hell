using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerHealth : MonoBehaviour
{
	private bool isStunned = false;
	private int hp = 5;
	[SerializeField]
	private AudioSource audioSourceHit, audioSourceBox;
	[SerializeField]
	private AudioClip hurtClip, deathClip;
	[SerializeField]
	private GameObject bloodEffectPrefab, headTrigger, deathParticleEffects, cutsceneTrigger;
	[SerializeField]
	private Animator spriteAnimator;
	private Coroutine endStunCoroutine;
	[SerializeField]
	private ParticleSystem ps, ps2;


	public bool IsStunned { get { return isStunned; } }
	public int Hp { get { return hp; } }


	private void Start()
	{
		
	}

	public void HitNecro()
	{
		if(isStunned)
		{
			if(endStunCoroutine != null)
			{
				StopCoroutine(endStunCoroutine);
				endStunCoroutine = null;
			}
			isStunned = false;
			hp -= 1;

			audioSourceHit.clip = hurtClip;
			audioSourceHit.Play();
			spriteAnimator.Play("Hurt", 0, 0);

			var emission = ps.emission;
			emission.rateOverTime = hp;
			emission = ps2.emission;
			emission.rateOverTime = hp*4;

			var main = ps.main;
			main.startSize = 1 + hp*0.2f;

			GameObject effect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
			Destroy(effect, 5);
			
			if(hp <= 0)
			{
				Die();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.name.Equals("Box"))
		{
			audioSourceBox.Play();
			spriteAnimator.Play("Stunned", 0, 0);

			var emission = ps.emission;
			emission.rateOverTime = 0;
			emission = ps2.emission;
			emission.rateOverTime = 0;

			if(endStunCoroutine != null)
			{
				StopCoroutine(endStunCoroutine);
			}
			isStunned = true;
			endStunCoroutine = StartCoroutine(EndStun());
		}
		else if(collider.name.Equals("Player"))
		{
			Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
			rb.velocity = new Vector2(rb.velocity.x, 0);
			rb.velocity += new Vector2(0, 12);

			collider.GetComponent<PlayerHealth>().SubtractHp(300);
			collider.GetComponents<AudioSource>()[1].Play();
			GameObject effect = Instantiate(bloodEffectPrefab, collider.transform.position, Quaternion.identity);
			Destroy(effect, 5);
		}
	}

	private IEnumerator EndStun()
	{
		yield return new WaitForSeconds(5);
		isStunned = false;

		var emission = ps.emission;
		emission.rateOverTime = hp;
		emission = ps2.emission;
		emission.rateOverTime = hp*4;
	}

	private void Die()
	{
		audioSourceHit.clip = deathClip;
		audioSourceHit.Play();

		spriteAnimator.Play("Dying", 0, 0);
		deathParticleEffects.SetActive(true);

		var emission = ps.emission;
		emission.rateOverTime = 0;
		emission = ps2.emission;
		emission.rateOverTime = 0;

		GameObject backgroundSound = GameObject.FindWithTag("BackgroundSound");
		if(backgroundSound != null)
		{
			backgroundSound.GetComponent<ChangeMusic>().PlayFinalMusic();
		}
		cutsceneTrigger.GetComponent<Collider2D>().enabled = true;

		headTrigger.SetActive(false);
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<Collider2D>().enabled = false;
		Destroy(GetComponent<Necromancer>());
		Destroy(GetComponent<NecromancerHealth>());
	}
}
