using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{
	private bool sawPlayer = false, attacking = false, isFallingDown = false;
	private float speed = 0;
	private Transform player;
	[SerializeField]
	private Animator spriteAnimator, spriteAnimator2;
	[SerializeField]
	private Collider2D swordTrigger, swordTrigger2;
	[SerializeField]
	private AudioSource enemyAudio, attackAudio;
	[SerializeField]
	private AudioClip sawPlayerClip, hurtClip, deathClip;


	private void Start()
	{
		InvokeRepeating("Look", 0.1f, 0.1f);
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.CompareTag("Player") && !sawPlayer)
		{
			player = collider.transform;
			enemyAudio.clip = sawPlayerClip;
			enemyAudio.volume = 0.1f;
			enemyAudio.pitch = 1;
			enemyAudio.Play();
			sawPlayer = true;

			speed = 0.11f;
			spriteAnimator.SetFloat("Speed", speed);
			spriteAnimator2.SetFloat("Speed", speed);
		}
	}

	private void Look()
	{
		if(attacking) // If you are attacking the player then do not anything else.
		{
			return;
		}

		if(sawPlayer)
		{
			if((player.position - transform.position).magnitude < 3.9f) // Attack the player if you are close to him.
			{
				speed = 0;
				spriteAnimator.SetFloat("Speed", speed);
				spriteAnimator2.SetFloat("Speed", speed);
				StartCoroutine(Attack());
			}
			else // Chase the player.
			{
				speed = 0.11f;
				spriteAnimator.SetFloat("Speed", speed);
				spriteAnimator2.SetFloat("Speed", speed);
			}
		}
	}

	private void Move()
	{
		float direction = -0.1f;
		transform.position += new Vector3(direction*speed, 0);
	}

	private IEnumerator Attack()
	{
		attacking = true;

		spriteAnimator.SetBool("Attack", true);
		spriteAnimator2.SetBool("Attack", true);
		swordTrigger.enabled = true;
		swordTrigger2.enabled = true;
		attackAudio.Play();
		yield return new WaitForSeconds(0.5f);

		spriteAnimator.SetBool("Attack", false);
		spriteAnimator2.SetBool("Attack", false);
		swordTrigger.enabled = false;
		swordTrigger2.enabled = false;
		yield return new WaitForSeconds(0.25f);

		attacking = false;
	}
}
