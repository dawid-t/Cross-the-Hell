using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossFallingDown : MonoBehaviour
{
	[SerializeField]
	private Animator animator, animator2;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.name.Equals("Enemy-MiniBoss"))
		{
			animator.SetBool("FallingDown", true);
			animator2.SetBool("FallingDown", true);
			GetComponent<AudioSource>().Play();
		}
	}
}
