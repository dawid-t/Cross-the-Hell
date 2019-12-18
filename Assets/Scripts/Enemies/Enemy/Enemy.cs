using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private bool goRight = true;
	private bool sawPlayer = false, attacking = false, lookAround = false, isFallingDown = false;
	private int idleCounter = 0;
	private float speed = 0;
	[SerializeField]
	private Animator spriteAnimator, enemyStateAnimator;
	[SerializeField]
	private Collider2D swordTrigger;
	[SerializeField]
	private SpriteRenderer enemyStateSpriteRenderer;
	[SerializeField]
	private Sprite enemyStateDangerMark, enemyStateSuspectMark;
	[SerializeField]
	private AudioSource enemyAudio, attackAudio;
	[SerializeField]
	private AudioClip sawPlayerClip, suspectClip;
	private Coroutine lookAroundCoroutine = null;


	private void Start()
	{
		Rotate();
		InvokeRepeating("Look", 0.1f, 0.1f);
	}

	private void FixedUpdate()
	{
		#region Debug:
		/*float offsetRayPosition = (goRight) ? 0.4f : -0.4f;
		Vector2 rayPosition = new Vector2(transform.position.x + offsetRayPosition, transform.position.y + 0.2f);
		Ray2D ray = new Ray2D(rayPosition, transform.right);
		Ray2D ray2 = new Ray2D(rayPosition + new Vector2(0, -0.7f), transform.right - transform.up);
		Debug.DrawLine(ray.origin, ray.GetPoint(5));
		Debug.DrawLine(ray2.origin, ray2.GetPoint(0.35f));*/
		#endregion Debug.

		Move();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(isFallingDown && collision.gameObject.CompareTag("Ground"))
		{
			isFallingDown = false;
			spriteAnimator.SetBool("FallingDown", false);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if(!isFallingDown && collision.gameObject.CompareTag("Ground"))
		{
			isFallingDown = true;
			spriteAnimator.SetBool("FallingDown", true);
		}
	}

	private void Rotate()
	{
		transform.rotation = (goRight) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
	}

	private void Look()
	{
		if(attacking) // If you are attacking the player then do not anything else.
		{
			return;
		}
		else if(spriteAnimator.GetBool("Hurt")) // If player attacked you then look around.
		{
			speed = 0;
			spriteAnimator.SetFloat("Speed", speed);
			StopLookAroundCoroutine();
			lookAround = true;
			return;
		}

		int layer = gameObject.layer; // 12 = FirstDimensionEnemy, 13 = SecondDimensionEnemy.
		int layerMask = (layer == 12) ? LayerMask.GetMask("FirstDimension") : LayerMask.GetMask("SecondDimension");
		float offsetRayPosition = (goRight) ? 0.4f : -0.4f;
		Vector2 rayPosition = new Vector2(transform.position.x + offsetRayPosition, transform.position.y + 0.2f);
		RaycastHit2D hitPlayer = Physics2D.Raycast(rayPosition, transform.right, 8, layerMask);
		RaycastHit2D hitGround = Physics2D.Raycast(rayPosition + new Vector2(0, -0.7f), transform.right - transform.up, 0.35f, layerMask);

		#region Debug:
		/*string playerName = (hitPlayer.transform != null) ? hitPlayer.transform.name : "NULL";
		string groundName = (hitGround.transform != null) ? hitGround.transform.name : "NULL";
		Debug.Log("hitPlayer.transform.name: " + playerName + "; hitGround.transform.name: " + groundName);
		Ray2D ray = new Ray2D(rayPosition, transform.right);
		Ray2D ray2 = new Ray2D(rayPosition + new Vector2(0, -0.7f), transform.right - transform.up);
		Debug.DrawLine(ray.origin, ray.GetPoint(5));
		Debug.DrawLine(ray2.origin, ray2.GetPoint(0.3f));*/
		#endregion Debug.

		if(hitPlayer.transform != null && hitPlayer.transform.CompareTag("Player")) // If you see the player.
		{
			if(!sawPlayer)
			{
				enemyStateSpriteRenderer.sprite = enemyStateDangerMark;
				enemyStateAnimator.Play("BounceMarkEffect", 0, 0);
				enemyAudio.clip = sawPlayerClip;
				enemyAudio.volume = 0.1f;
				enemyAudio.pitch = 1;
				enemyAudio.Play();
				sawPlayer = true;
			}
			StopLookAroundCoroutine();

			if((hitPlayer.transform.position - transform.position).magnitude < 1.5f) // Attack the player if you are close to him.
			{
				speed = 0;
				spriteAnimator.SetFloat("Speed", speed);
				StartCoroutine(Attack());
			}
			else if(hitGround.transform != null && hitGround.transform.CompareTag("Ground")) // Chase the player to the platform edge.
			{
				speed = 1;
				spriteAnimator.SetFloat("Speed", speed);
			}
			else
			{
				speed = 0;
				spriteAnimator.SetFloat("Speed", speed);
			}

			lookAround = true;
		}
		else if(lookAround) // Look around after chasing the player.
		{
			if(lookAroundCoroutine == null)
			{
				sawPlayer = false;
				speed = 0;
				spriteAnimator.SetFloat("Speed", speed);
				lookAroundCoroutine = StartCoroutine(LookAround());
			}
		}
		else if(hitGround.transform != null && hitGround.transform.CompareTag("Ground")) // Walk to the platform edge.
		{
			sawPlayer = false;
			speed = 0.4f;
			spriteAnimator.SetFloat("Speed", speed);
		}
		else if(idleCounter < 50) // Wait 5s on the platform edge.
		{
			sawPlayer = false;
			speed = 0;
			spriteAnimator.SetFloat("Speed", speed);
			idleCounter++;
		}
		else // Change direction and walk to the second platform edge.
		{
			sawPlayer = false;
			goRight = !goRight;
			Rotate();
			idleCounter = 0;
		}
	}

	private void Move()
	{
		float direction = (goRight) ? 0.1f : -0.1f;
		transform.position += new Vector3(direction*speed, 0);
	}

	private IEnumerator Attack()
	{
		attacking = true;

		spriteAnimator.SetBool("Attack", true);
		swordTrigger.enabled = true;
		attackAudio.Play();
		yield return new WaitForSeconds(0.5f);

		spriteAnimator.SetBool("Attack", false);
		swordTrigger.enabled = false;
		yield return new WaitForSeconds(0.25f);

		attacking = false;
	}

	private IEnumerator LookAround()
	{
		enemyStateSpriteRenderer.sprite = enemyStateSuspectMark;
		enemyStateAnimator.Play("BounceMarkEffect", 0, 0);
		enemyAudio.clip = suspectClip;
		enemyAudio.volume = 0.05f;
		enemyAudio.pitch = 1.4f;
		enemyAudio.Play();

		for(int i = 0; i < 4; i++)
		{
			yield return new WaitForSeconds(0.5f);
			goRight = !goRight;
			Rotate();
		}

		lookAround = false;
	}

	private void StopLookAroundCoroutine()
	{
		if(lookAroundCoroutine != null)
		{
			StopCoroutine(lookAroundCoroutine);
			lookAroundCoroutine = null;
		}
	}
}
