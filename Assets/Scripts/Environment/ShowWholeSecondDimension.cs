using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWholeSecondDimension : MonoBehaviour
{
	private bool changeCamera;
	[SerializeField]
	private GameObject panel_OtherDimensionTransition;
	private Weapon weapon;
	private PlayerMovement playerMovement;
	private PlayerJump playerJump;
	[SerializeField]
	private Animator cameraAnimator, skipInfoAnimator;
	private List<GameObject> masksList = new List<GameObject>();
	private Coroutine showCoroutine, backToNormalCoroutine;


	private void Start()
	{
		if(Settings.Difficulty != Settings.GameDifficulty.Hard)
		{
			showCoroutine = StartCoroutine(Show());
		}
		else
		{
			SelfDestruction();
		}
	}

	private void Update()
	{
		StopShow();
	}

	private IEnumerator Show()
	{
		cameraAnimator.GetComponent<CameraZoom>().enabled = false;

		#region Disable player scripts:
		GameObject player = GameObject.FindWithTag("Player");
		weapon = player.GetComponent<Weapon>();
		playerMovement = player.GetComponent<PlayerMovement>();
		playerJump = player.transform.Find("JumpTrigger").GetComponent<PlayerJump>();

		weapon.enabled = false;
		playerMovement.enabled = false;
		playerJump.enabled = false;
		yield return new WaitForSeconds(1); // Wait for end of "scene change effect".
		#endregion Disable player scripts.

		#region Show the whole level if is not showed:
		skipInfoAnimator.Play("Start", 0, 0); // Show skip info text.
		changeCamera = false;
		if(cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("FollowPlayer"))
		{
			changeCamera = true;
			cameraAnimator.Play("ShowLevel", 0, 0);
		}
		yield return new WaitForSeconds(2);
		#endregion Show the whole level if is not showed.

		#region First dimension transition effect & sound:
		panel_OtherDimensionTransition.SetActive(true);
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
		yield return new WaitForSeconds(0.1665f); // Half time of transition effect.
		#endregion First dimension transition effect & sound.

		#region Add masks to the list and set them active:
		for(int i = 0; i < transform.childCount; i++)
		{
			GameObject mask = transform.GetChild(i).gameObject;
			masksList.Add(mask);
			mask.SetActive(true);
		}
		yield return new WaitForSeconds(3);
		#endregion Add masks to the list and set them active.

		#region Second dimension transition effect & sound:
		panel_OtherDimensionTransition.SetActive(false); // Turn off and on to repeat the animation.
		panel_OtherDimensionTransition.SetActive(true);
		audio.Play();
		yield return new WaitForSeconds(0.1665f); // Half time of transition effect.
		#endregion Second dimension transition effect & sound.

		backToNormalCoroutine = StartCoroutine(BackToNormal(false));
		showCoroutine = null;
	}

	private IEnumerator BackToNormal(bool interrupted)
	{
		for(int i = 0; i < masksList.Count; i++) // Deactivate masks.
		{
			masksList[i].SetActive(false);
		}

		if(!interrupted) // If player pressed any key then don't wait and let him move.
		{
			yield return new WaitForSeconds(1);
		}

		if(changeCamera) // Change camera view if was changed.
		{
			cameraAnimator.Play("FollowPlayer", 0, 0);
		}

		weapon.enabled = true;
		playerMovement.enabled = true;
		playerJump.enabled = true;

		panel_OtherDimensionTransition.SetActive(false);
		skipInfoAnimator.Play("SlowEnd", 0, 0);
		cameraAnimator.GetComponent<CameraZoom>().enabled = true;

		SelfDestruction();
	}

	private void StopShow()
	{
		// If space is pressed 1s after the level has been loaded, stop showing the whole second dimension:
		if(Input.GetKeyDown(KeyCode.Space) && Time.timeSinceLevelLoad > 1)
		{
			if(showCoroutine != null)
			{
				StopCoroutine(showCoroutine);
			}
			if(backToNormalCoroutine != null)
			{
				StopCoroutine(backToNormalCoroutine);
			}
			StartCoroutine(BackToNormal(true));
		}
	}

	private void SelfDestruction()
	{
		// These gameObjects are not longer needed:
		Destroy(panel_OtherDimensionTransition.transform.parent.gameObject, 1); // Wait 1s to end the text animation.
		Destroy(gameObject);
	}
}
