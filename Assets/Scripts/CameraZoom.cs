using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
	private static bool followPlayer = true;

	[SerializeField]
	private Animator cameraAnimator;


	private void Start()
	{
		if(!followPlayer) // Remember camera settings when scene is changed/restarted.
		{
			ShowLevel();
		}
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.C) && !Settings.Pause)
		{
			if(cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("FollowPlayer"))
			{
				ShowLevel();
			}
			else
			{
				FollowPlayer();
			}
		}
	}

	private void FollowPlayer()
	{
		followPlayer = true;
		cameraAnimator.Play("FollowPlayer", 0, 0);
	}

	private void ShowLevel()
	{
		followPlayer = false;
		cameraAnimator.Play("ShowLevel", 0, 0);
	}
}
