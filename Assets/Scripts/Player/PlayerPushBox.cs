using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushBox : MonoBehaviour
{
	private Animator animator;


	private void Start()
	{
		animator = transform.root.Find("Character").GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag.Equals("Box"))
		{
			animator.SetBool("PushBox", true);
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.tag.Equals("Box"))
		{
			Rigidbody2D boxRb = collider.gameObject.GetComponent<Rigidbody2D>();
			if(boxRb != null && (boxRb.velocity.y < 0.2f && boxRb.velocity.y > -0.2f))
			{
				boxRb.velocity = Vector2.zero;
			}
			animator.SetBool("PushBox", false);
		}
	}
}
