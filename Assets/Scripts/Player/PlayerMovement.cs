using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private float speed = 40;
	private Rigidbody2D rb;
	private Animator animator;


	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = transform.Find("Character").GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		#region Walk:
		float x = Input.GetAxis("Horizontal") * speed * Time.fixedDeltaTime;
		//transform.position += new Vector3(x, 0, 0);
		rb.velocity += new Vector2(x, 0);

		float maxSpeed = speed/10;
		if(x == 0 || (x > 0 && rb.velocity.x < 0) || (x < 0 && rb.velocity.x > 0))
		{
			rb.velocity = new Vector2(0, rb.velocity.y);
		}
		else if(rb.velocity.x > maxSpeed)
		{
			rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
		}
		else if(rb.velocity.x < -maxSpeed)
		{
			rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
		}
		#endregion Walk.

		#region Rotation & walk animation:
		if(x > 0)
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
			animator.SetBool("Run", true);
		}
		else if(x < 0)
		{
			transform.rotation = Quaternion.Euler(0, 180, 0);
			animator.SetBool("Run", true);
		}
		else
		{
			animator.SetBool("Run", false);
		}
		#endregion Rotation & walk animation.
	}
}
