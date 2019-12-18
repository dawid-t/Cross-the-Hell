using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon : MonoBehaviour
{
	private bool playHeartbeatEffect = false, isAttacking = false;
	private int attackCombo = 0;
	[SerializeField]
	private float speed = 500;
	[SerializeField]
	private GameObject jumpTrigger, stabEffectPrefab;
	private Animator spriteMaskAnimator, characterAnimator;
	private SpriteMask spriteMask;
	[SerializeField]
	private Collider2D attackTrigger;
	private Collider2D spriteMaskCollider;
	[SerializeField]
	private AudioClip heartbeatFastClip, heartbeatSlowClip;
	[SerializeField]
	private AudioClip[] attackClips = new AudioClip[3];
	[SerializeField]
	private AudioSource attackAudio;
	private AudioSource[] audioSource;
	private PlayerHealth health;
	[SerializeField]
	private ParticleSystem bloodEffect;
	private Coroutine stopAttackCoroutine;
	[SerializeField]
	private Transform[] attackTransforms = new Transform[3];


	private void Start()
	{
		audioSource = GetComponents<AudioSource>();
		health = GetComponent<PlayerHealth>();

		Transform spriteMaskParent = transform.Find("MaskParent");
		spriteMaskAnimator = spriteMaskParent.GetComponent<Animator>();
		spriteMask = spriteMaskParent.GetChild(0).GetComponent<SpriteMask>();
		spriteMaskCollider = spriteMask.GetComponent<Collider2D>();

		characterAnimator = transform.Find("Character").GetComponent<Animator>();
	}

	private void Update()
	{
		if(Settings.Pause)
		{
			return;
		}

		if((Input.GetMouseButton(1) || Input.GetKey(KeyCode.P)) && health.Hp >= 10) // If mouse/enter is pressed and hp is >= 1%.
		{
			IncreaseDimensionRange();
		}
		else
		{
			DecreaseDimensionRange();

			if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.L)) && health.Hp >= 10 && !EventSystem.current.IsPointerOverGameObject())
			{
				Attack();
			}
		}
	}

	private void IncreaseDimensionRange()
	{
		// Change physics layer:
		spriteMaskAnimator.enabled = true;
		spriteMask.enabled = true;
		spriteMaskCollider.enabled = true;
		gameObject.layer = 9;
		jumpTrigger.layer = 9;

		// Change dimension visual and sound effects:
		if(!audioSource[0].isPlaying)
		{
			if(health.Hp >= 310) // 31% - 100%.
			{
				audioSource[0].clip = heartbeatFastClip;
				spriteMaskAnimator.Play("HeartbeatFast", 0, 0);
			}
			else
			{
				audioSource[0].clip = heartbeatSlowClip;
				spriteMaskAnimator.Play("HeartbeatSlow", 0, 0);
			}
			audioSource[0].Play();
			var emission = bloodEffect.emission;
			emission.rateOverTime = 8;
		}
		//if(!characterAnimator.GetCurrentAnimatorStateInfo(1).IsName("SecondDimension"))
		if(!characterAnimator.GetBool("ChangeDimension"))
		{
			characterAnimator.SetBool("ChangeDimension", true);
			audioSource[1].Play();
		}

		// Change dimension circle size:
		float speed = this.speed * Time.fixedDeltaTime;
		if(spriteMask.transform.localScale.x < 5)
		{
			spriteMask.transform.localScale += new Vector3(0.035f, 0.035f) * speed;
		}
		else if(spriteMask.transform.localScale.x < 8)
		{
			spriteMask.transform.localScale += new Vector3(0.015f, 0.015f) * speed;
		}
		else if(spriteMask.transform.localScale.x < 10)
		{
			spriteMask.transform.localScale += new Vector3(0.0025f, 0.0025f) * speed;
		}

		health.SubtractHp(1);
		if(health.Hp <= 309) // 30%.
		{
			audioSource[0].clip = heartbeatSlowClip;
			spriteMaskAnimator.SetBool("Slow", true);
		}
	}

	private void DecreaseDimensionRange()
	{
		if(spriteMask.transform.localScale.x > 0)
		{
			spriteMask.transform.localScale -= new Vector3(0.07f, 0.07f) * speed * Time.fixedDeltaTime;
			characterAnimator.SetBool("ChangeDimension", false);
		}
		else
		{
			spriteMask.transform.localScale = Vector2.zero;
			spriteMaskAnimator.enabled = false;
			spriteMask.enabled = false;
			spriteMaskCollider.enabled = false;
			gameObject.layer = 8;
			jumpTrigger.layer = 8;

			if(audioSource[0].isPlaying)
			{
				audioSource[0].Stop();
				var emission = bloodEffect.emission;
				emission.rateOverTime = 0;
			}
		}
	}

	private void Attack()
	{
		AnimatorStateInfo animatorStateInfo = characterAnimator.GetCurrentAnimatorStateInfo(1);
		if((animatorStateInfo.IsName("Idle") || animatorStateInfo.normalizedTime > 0.7f) && attackCombo < 3)
		{
			#region Animation, combo & weapon trigger:
			attackCombo++;
			characterAnimator.SetInteger("Attack", attackCombo);

			if(stopAttackCoroutine != null)
			{
				StopCoroutine(stopAttackCoroutine);
			}
			stopAttackCoroutine = StartCoroutine(StopAttack());

			attackTrigger.enabled = true;
			StartCoroutine(OffAttackTrigger());
			#endregion Animation, combo & weapon trigger.

			#region Visual effect:
			GameObject effect = Instantiate(stabEffectPrefab, attackTransforms[attackCombo-1].position, Quaternion.identity);
			ParticleSystem ps = effect.GetComponent<ParticleSystem>();
			var main = ps.main;
			main.startRotationY = (transform.localRotation.y == 0) ? 0 : Mathf.PI;
			main.startRotationZ = attackTransforms[attackCombo-1].localRotation.z;
			Destroy(effect, 1);
			#endregion Visual effect.

			#region Audio effect:
			attackAudio.clip = attackClips[attackCombo-1];
			attackAudio.Play();
			#endregion Attack audio effect.
		}
	}

	private IEnumerator StopAttack()
	{
		yield return new WaitForSeconds(0.33333f); // Attack animation time is 333 milliseconds.
		characterAnimator.SetInteger("Attack", 0); // Set animation to "Idle".

		yield return new WaitForSeconds(0.2f); // Can do combo too in 200 milliseconds after finished attack.
		attackCombo = 0; // Break combo.
		stopAttackCoroutine = null;
	}

	private IEnumerator OffAttackTrigger()
	{
		yield return new WaitForSeconds(0.1f);
		attackTrigger.enabled = false;
	}
}
