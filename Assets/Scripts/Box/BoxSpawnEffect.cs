using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawnEffect : MonoBehaviour
{
	private Animator animator;

	
	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void SpawnEffect()
	{
		animator.Play("Spawn", 0, 0);
	}

	public void RespawnEffect()
	{
		animator.Play("Respawn", 0, 0);
	}
}
