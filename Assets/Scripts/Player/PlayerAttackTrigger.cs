using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag.StartsWith("Enemy"))
		{
			EnemyHealth health = collider.GetComponent<EnemyHealth>();
			health.HitEnemy();
		}
		else if(collider.name.Equals("Necromancer"))
		{
			NecromancerHealth health = collider.GetComponent<NecromancerHealth>();
			health.HitNecro();
		}
	}
}
