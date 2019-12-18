using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	private Canvas canvas_Menu, canvas_Difficulty, canvas_StartGame;
	[SerializeField]
	private Button button_Easy, button_Normal, button_Hard, button_Next;
	[SerializeField]
	private Animator cameraAnimator;
	[SerializeField]
	private VideoPlayerWebGL videoPlayerWebGL;


	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			ButtonBack();
		}
	}

	#region Menu & Credits panel:
	public void ButtonStartGame()
	{
		canvas_Menu.enabled = false;
		canvas_Difficulty.enabled = true;
		videoPlayerWebGL.PlayVideos();
	}

	public void ButtonCredits()
	{
		cameraAnimator.Play("ShowLevel", 0, 0); // Second camera.
	}
	#endregion Menu & Credits panel.

	#region Difficulty panel:
	public void ButtonDifficulty(int difficulty)
	{
		#region Set the colors of buttons to default:
		Color normalColor = new Color(1, 0, 0, 0.1255f);
		Color highlightedColor;

		ColorBlock colorBlock = button_Easy.colors; // ColorBlock from easy difficulty button.
		colorBlock.normalColor = normalColor;
		highlightedColor = colorBlock.highlightedColor;
		colorBlock.highlightedColor = new Color(highlightedColor.r, highlightedColor.g, highlightedColor.b, 0.4706f);
		button_Easy.colors = colorBlock;

		colorBlock = button_Normal.colors; // ColorBlock from normal difficulty button.
		colorBlock.normalColor = normalColor;
		highlightedColor = colorBlock.highlightedColor;
		colorBlock.highlightedColor = new Color(highlightedColor.r, highlightedColor.g, highlightedColor.b, 0.4706f);
		button_Normal.colors = colorBlock;

		colorBlock = button_Hard.colors; // ColorBlock from hard difficulty button.
		colorBlock.normalColor = normalColor;
		highlightedColor = colorBlock.highlightedColor;
		colorBlock.highlightedColor = new Color(highlightedColor.r, highlightedColor.g, highlightedColor.b, 0.4706f);
		button_Hard.colors = colorBlock;
		#endregion Set the colors of buttons to default.

		#region Change the colors of the clicked button:
		switch(difficulty)
		{
			case (int)Settings.GameDifficulty.Easy:
				colorBlock = button_Easy.colors;
				colorBlock.normalColor = colorBlock.pressedColor;
				colorBlock.highlightedColor = colorBlock.pressedColor;
				button_Easy.colors = colorBlock;
				break;
			case (int)Settings.GameDifficulty.Normal:
				colorBlock = button_Normal.colors;
				colorBlock.normalColor = colorBlock.pressedColor;
				colorBlock.highlightedColor = colorBlock.pressedColor;
				button_Normal.colors = colorBlock;
				break;
			case (int)Settings.GameDifficulty.Hard:
				colorBlock = button_Hard.colors;
				colorBlock.normalColor = colorBlock.pressedColor;
				colorBlock.highlightedColor = colorBlock.pressedColor;
				button_Hard.colors = colorBlock;
				break;
		}
		#endregion Change the colors of the clicked button.

		Settings.Difficulty = (Settings.GameDifficulty)difficulty; // Set new difficulty level.
		button_Next.interactable = true;
	}

	public void ButtonNext()
	{
		canvas_Difficulty.enabled = false;
		canvas_StartGame.enabled = true;
		videoPlayerWebGL.StopVideos();
	}
	#endregion Difficulty panel.

	#region Start Game panel:
	public void ButtonContinue()
	{
		SceneChanger.Instance.ChangeScene(1);
	}
	#endregion Start Game panel.

	public void ButtonBack()
	{
		if(canvas_Difficulty.enabled) // Back from Difficulty to Menu panel.
		{
			canvas_Difficulty.enabled = false;
			canvas_Menu.enabled = true;
			videoPlayerWebGL.StopVideos();
		}
		else if(canvas_StartGame.enabled) // Back from StartGame to Difficulty panel.
		{
			canvas_StartGame.enabled = false;
			canvas_Difficulty.enabled = true;
			videoPlayerWebGL.PlayVideos();
		}
		else if(cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowLevel")) // Back from Credits to Menu panel.
		{
			cameraAnimator.Play("FollowPlayer", 0, 0); // First camera.
		}
	}
}
