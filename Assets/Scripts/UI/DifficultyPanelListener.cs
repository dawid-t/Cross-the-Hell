using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DifficultyPanelListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Animator cameraAnimator;


	public void OnPointerEnter(PointerEventData eventData)
	{
		cameraAnimator.Play("ShowDifficultyPanel", 0, 0);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		cameraAnimator.Play("FollowPlayer", 0, 0);
	}
}
