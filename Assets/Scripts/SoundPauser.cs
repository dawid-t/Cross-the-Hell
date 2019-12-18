using System.Collections.Generic;
using UnityEngine;

public class SoundPauser : MonoBehaviour
{
	private static List<AudioSource> audioSourcesList = new List<AudioSource>();

	[SerializeField]
	private bool pauseAllAudioSources = true, addToListOnAwake = true;
	[SerializeField]
	private AudioSource[] audioSources = new AudioSource[0];


	private void Awake()
	{
		if(addToListOnAwake)
		{
			AddAudioSourcesToList();
		}
	}

	private void OnDestroy()
	{
		RemoveAudioSourcesFromList();
	}

	public void AddAudioSourcesToList()
	{
		if(pauseAllAudioSources)
		{
			AudioSource[] audioSources = GetComponents<AudioSource>();
			for(int i = 0; i < audioSources.Length; i++)
			{
				if(!audioSources[i].mute)
				{
					audioSourcesList.Add(audioSources[i]);
				}
			}
		}
		else
		{
			for(int i = 0; i < audioSources.Length; i++)
			{
				if(!audioSources[i].mute)
				{
					audioSourcesList.Add(audioSources[i]);
				}
			}
		}
	}

	public void RemoveAudioSourcesFromList()
	{
		if(pauseAllAudioSources)
		{
			AudioSource[] audioSources = GetComponents<AudioSource>();
			for(int i = 0; i < audioSources.Length; i++)
			{
				if(!audioSources[i].mute)
				{
					audioSourcesList.Remove(audioSources[i]);
				}
			}
		}
		else
		{
			for(int i = 0; i < audioSources.Length; i++)
			{
				if(!audioSources[i].mute)
				{
					audioSourcesList.Remove(audioSources[i]);
				}
			}
		}
	}

	public static void ResumeSound()
	{
		foreach(AudioSource audioSource in audioSourcesList)
		{
			audioSource.UnPause();
		}
	}

	public static void PauseSound()
	{
		foreach(AudioSource audioSource in audioSourcesList)
		{
			audioSource.Pause();
		}
	}
}
