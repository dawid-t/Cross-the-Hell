using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionEnergy : MonoBehaviour
{
	private void Awake()
	{
		if(Settings.Difficulty == Settings.GameDifficulty.Easy)
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
}
