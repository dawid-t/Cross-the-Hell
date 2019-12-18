using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject panel_Menu;
	private Animator menuPanelAnimator;
	private Coroutine delayedOffMenuPanelCoroutine;


	private void Awake()
	{
		menuPanelAnimator = panel_Menu.GetComponent<Animator>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			ButtonPause();
		}
	}

	public void ButtonPause()
	{
		Settings.PauseGame(!Settings.Pause);
		
		if(delayedOffMenuPanelCoroutine != null)
		{
			StopCoroutine(delayedOffMenuPanelCoroutine);
			delayedOffMenuPanelCoroutine = null;
		}

		if(Settings.Pause)
		{
			panel_Menu.SetActive(true);
			menuPanelAnimator.Play("Start", 0, 0);
		}
		else
		{
			menuPanelAnimator.Play("SlowEnd", 0, 0);
			delayedOffMenuPanelCoroutine = StartCoroutine(DelayedOffMenuPanel());
		}
	}

	public void ButtonBackToMenu()
	{
		Settings.PauseGame(false);
		SceneChanger.Instance.ChangeScene(0);
		menuPanelAnimator.Play("SlowEnd", 0, 0);
	}

	public void ButtonRestartLevel()
	{
		Settings.PauseGame(false);
		SceneChanger.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);
		menuPanelAnimator.Play("SlowEnd", 0, 0);
	}

	public void ButtonNextLevel()
	{
		if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings-1)
		{
			Settings.PauseGame(false);
			SceneChanger.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex+1);
			menuPanelAnimator.Play("SlowEnd", 0, 0);
		}
	}

	private IEnumerator DelayedOffMenuPanel()
	{
		yield return new WaitForSecondsRealtime(0.117f);
		panel_Menu.SetActive(false);
		delayedOffMenuPanelCoroutine = null;
	}
}
