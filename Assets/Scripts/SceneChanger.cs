using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneChanger : MonoBehaviour
{
	public static SceneChanger instance;

	private bool sceneIsChanging = true;
	private float imageWidth = 0;
	[SerializeField]
	private GameObject imageGameObject;
	private RectTransform imageRectTransform;
	[SerializeField]
	private Animator healthBarAnimator, pauseButtonAnimator;
	[SerializeField]
	private Transform player;


	public static SceneChanger Instance { get { return instance; } }


	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		imageRectTransform = imageGameObject.GetComponent<RectTransform>();
		imageWidth = Mathf.Sqrt(Screen.width*Screen.width + Screen.height*Screen.height)*2.25f; // Screen diagonal * 2.25f = image width covering the whole screen from the corner.

		imageRectTransform.sizeDelta = new Vector2(imageWidth, imageWidth);
		StartSceneEffect();
	}

	private void StartSceneEffect()
	{
		StartCoroutine(ChangeSceneEffect(-imageWidth/50));
	}

	private void EndSceneEffect()
	{
		StartCoroutine(ChangeSceneEffect(imageWidth/50));
	}

	private IEnumerator ChangeSceneEffect(float additionalSize)
	{
		Settings.UseSlowMotion(true);
		imageGameObject.SetActive(true);
		for(int i = 0; i < 50; i++)
		{
			#region Update image position:
			if(player != null)
			{
				Vector2 imageNewPosition = Camera.main.WorldToScreenPoint(player.transform.position);
				if(imageNewPosition.x < 0)
				{
					imageNewPosition.x = 0;
				}
				else if(imageNewPosition.x > Screen.width)
				{
					imageNewPosition.x = Screen.width;
				}

				if(imageNewPosition.y < 0)
				{
					imageNewPosition.y = 0;
				}
				else if(imageNewPosition.y > Screen.height)
				{
					imageNewPosition.y = Screen.height;
				}
				imageRectTransform.position = imageNewPosition;
			}
			else
			{
				imageRectTransform.position = new Vector2(Screen.width/2, Screen.height/2);
			}
			#endregion Update image position.

			yield return new WaitForSeconds(0.001f);
			imageRectTransform.sizeDelta += new Vector2(additionalSize, additionalSize); // Update image size.
		}

		if(additionalSize < 0)
		{
			imageGameObject.SetActive(false);
			Settings.UseSlowMotion(false);
			healthBarAnimator.Play("Start", 0, 0);
			pauseButtonAnimator.Play("Start", 0, 0);
		}
	}

	private int GetScreenMaxSize()
	{
		int width = Screen.width;
		int height = Screen.height;
		if(width > height)
		{
			return width;
		}
		else
		{
			return height;
		}
	}

	public void ChangeScene(int levelID)
	{
		healthBarAnimator.Play("End", 0, 0);
		pauseButtonAnimator.Play("End", 0, 0);
		OffPlayerScripts();
		EndSceneEffect();
		StartCoroutine(DelayedChangeScene(levelID));
	}

	private IEnumerator DelayedChangeScene(int levelID)
	{
		yield return new WaitForSecondsRealtime(1);
		Settings.UseSlowMotion(false);
		SceneManager.LoadScene(levelID);
	}

	private void OffPlayerScripts()
	{
		if(player != null)
		{
			player.GetComponent<PlayerMovement>().enabled = false;
			player.GetComponent<Weapon>().enabled = false;
			player.Find("JumpTrigger").GetComponent<PlayerJump>().enabled = false;
		}
	}
}
