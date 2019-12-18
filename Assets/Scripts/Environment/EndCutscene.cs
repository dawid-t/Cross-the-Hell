using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour
{
	public static EndCutscene instance;

	[SerializeField]
	private GameObject panel_Cutscene, player, child, teleport;
	[SerializeField]
	private Transform playerPosition, childPosition;
	[SerializeField]
	private CinemachineVirtualCamera camera;


	public static EndCutscene Instance { get { return instance; } }


	private void Awake()
	{
		instance = this;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag.Equals("Player"))
		{
			player.GetComponent<PlayerMovement>().enabled = false;
			player.transform.Find("JumpTrigger").GetComponent<PlayerJump>().enabled = false;
			Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
			rb.velocity = new Vector2(0, rb.velocity.y);

			Animator anim = player.transform.Find("Character").GetComponent<Animator>();
			anim.SetBool("Run", false);
			anim.Play("Idle", 0, 0);

			StartCoroutine(BeginCutscene());
		}
		
	}

	public void OpenTeleport()
	{
		StartCoroutine(ContinueCutscene());
	}

	private IEnumerator BeginCutscene() // I.
	{
		yield return new WaitForSeconds(2);
		panel_Cutscene.SetActive(true);
		panel_Cutscene.GetComponent<Animator>().Play("End", 0, 0);

		Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(0, 0); // Sometimes player is sliding.

		yield return new WaitForSeconds(2);
		player.transform.position = playerPosition.position;
		player.transform.rotation = Quaternion.Euler(0, 0, 0);

		child.transform.position = childPosition.position;
		child.transform.rotation = Quaternion.Euler(0, 180, 0);

		panel_Cutscene.GetComponent<Animator>().Play("Start", 0, 0);
		player.GetComponent<PlayerHealth>().Cutscene = true;
	}

	private IEnumerator ContinueCutscene() // II.
	{
		yield return new WaitForSeconds(1.5f);
		Transform soulEffect = player.transform.Find("SoulEffect");
		camera.Follow = soulEffect;
		camera.m_Lens.OrthographicSize = 4;
		soulEffect.gameObject.SetActive(true);

		yield return new WaitForSeconds(4.75f);
		teleport.SetActive(true);
		teleport.GetComponent<Animator>().Play("Spawn", 0, 0);
		child.transform.rotation = Quaternion.Euler(0, 0, 0);

		yield return new WaitForSeconds(2);
		panel_Cutscene.SetActive(true);
		panel_Cutscene.GetComponent<Animator>().Play("End", 0, 0);

		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(1); // 1 level.
	}
}
