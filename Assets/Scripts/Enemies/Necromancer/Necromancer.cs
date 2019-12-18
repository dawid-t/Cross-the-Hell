using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : MonoBehaviour
{
	[SerializeField]
	private Transform player, staff;
	[SerializeField]
	private GameObject fireballPrefab;
	[SerializeField]
	private Animator animator;
	private NecromancerHealth health;


	private void Start()
	{
		health = GetComponent<NecromancerHealth>();

		// On hard mode is not showing the whole second dimension so then start attacking earlier:
		int firstAttackTime = (Settings.Difficulty != Settings.GameDifficulty.Hard) ? 9 : 3;
		Invoke("Attack", firstAttackTime);
	}

	public void FixedUpdate()
	{
		LookAtPlayer();
	}

	private void Attack()
	{
		AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
		if(!state.IsName("Hurt") && !state.IsName("Stunned"))
		{
			animator.Play("Attack", 1, 0);

			GameObject fireball = Instantiate(fireballPrefab, staff.position, Quaternion.identity);
			Vector3 direction = (new Vector3(player.position.x, player.position.y+0.375f) - staff.position).normalized;
			fireball.GetComponent<Fireball>().Direction = direction/10;
		}
		Invoke("Attack", health.Hp); // Necro have max 5hp.
	}

	private void LookAtPlayer()
	{
		if(!health.IsStunned)
		{
			if(player.position.x < transform.position.x)
			{
				transform.rotation = Quaternion.Euler(0, 180, 0);
			}
			else
			{
				transform.rotation = Quaternion.Euler(0, 0, 0);
			}
		}
	}
}
