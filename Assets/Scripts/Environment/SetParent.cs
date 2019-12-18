using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
	private void OnCollisionStay2D(Collision2D collision)
	{
		if(collision.transform.tag.Equals("Ground"))
		{
			Animator animator = collision.transform.GetComponent<Animator>();
			if(animator == null)
			{
				transform.parent = collision.transform;
			}
			else
			{
				if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && animator.GetBool("Active"))
				{
					transform.parent = collision.transform;
				}
				else
				{
					transform.parent = null;
				}
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		transform.parent = null;
	}
}
