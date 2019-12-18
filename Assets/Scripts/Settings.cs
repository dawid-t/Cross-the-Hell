using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
	private static bool pause = false, slowMotion = false;
	private static GameDifficulty difficulty = GameDifficulty.Normal;


	public static bool Pause { get { return pause; } }
	public static bool SlowMotion { get { return slowMotion; } }
	public static GameDifficulty Difficulty { get { return difficulty; } set { difficulty = value; } }


	public enum GameDifficulty { Easy, Normal, Hard }


	private void Start()
	{

	}

	public void SetDifficulty()
	{

	}

	public static void PauseGame(bool pause)
	{
		float timeScale = (slowMotion) ? 0.5f : 1;
		Settings.pause = pause;
		//Time.timeScale = (pause) ? 0 : timeScale;
		
		if(pause)
		{
			Time.timeScale = 0;
			SoundPauser.PauseSound();
		}
		else
		{
			Time.timeScale = timeScale;
			SoundPauser.ResumeSound();
		}
	}

	public static void UseSlowMotion(bool use)
	{
		slowMotion = use;
		if(!pause)
		{
			Time.timeScale = (slowMotion) ? 0.5f : 1;
		}
	}
}
