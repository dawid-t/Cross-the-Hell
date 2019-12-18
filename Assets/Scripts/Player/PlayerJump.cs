using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
	[SerializeField]
	private float jumpVelocity = 7;
	private float lastVelocityY = 0, fallMultiplier = 2, lowJumpMultiplier = 1;
	private Rigidbody2D rb;
	private Animator animator;
	[SerializeField]
	private AudioSource jumpSound;
	[SerializeField]
	private GameObject smokePrefab;
	private Collider2D boxCollider;
	private Collider2D[] colliders = new Collider2D[1];


	private void Awake()
	{
		rb = transform.root.GetComponent<Rigidbody2D>();
		animator = transform.root.Find("Character").GetComponent<Animator>();
		boxCollider = GetComponent<Collider2D>();
	}

	private void Update()
	{
		if(Settings.Pause)
		{
			return;
		}

		// Jump:
		if(Input.GetKeyDown(KeyCode.Space))
		{
			int collidersCount = boxCollider.GetContacts(colliders);
			if(collidersCount > 0)
			{
				rb.velocity = new Vector2(0, 0);
				rb.velocity += new Vector2(0, jumpVelocity);
				jumpSound.Play();
				animator.SetBool("Jump", true);
			}
		}
	}

	private void FixedUpdate()
	{
		lastVelocityY = rb.velocity.y; // FixedUpdate() is calling before OnTriggerEnter() so save last velocity for trigger method.

		// Jump modifiers (Mario style):
		/*if(rb.velocity.y < 0)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;
		}
		else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.fixedDeltaTime;
		}*/
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag.Equals("Ground"))
		{
			animator.SetBool("Jump", false);
			animator.SetBool("FallingDown", false);

			if(lastVelocityY < -2)
			{
				GameObject smoke = Instantiate(smokePrefab, new Vector2(transform.position.x, transform.position.y-0.5f), smokePrefab.transform.rotation);
				Destroy(smoke, 2);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.tag.Equals("Ground"))
		{
			int collidersCount = boxCollider.GetContacts(colliders);
			if(collidersCount == 0)
			{
				animator.SetBool("FallingDown", true);
			}
		}
	}
}
