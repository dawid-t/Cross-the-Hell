using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


/* CLASS DESCRIPTION:
 * 
 * In WebGL if you disable gameObject with VideoPlayer and enable again, the video will be not playing again.
 * You have to create VideoPlayer in OnEnable() method and destroy in OnDisable().
 * 
 * In this case all UI is active (for performance - when disable some windows, disable the parent canvas component)
 * so use methods PlayVideos() and StopVideos() in MainMenu script.
*/

public class VideoPlayerWebGL : MonoBehaviour
{
	[SerializeField]
	private GameObject panel_EasyModeExampleImage, panel_NormalModeExampleImage, panel_HardModeExampleImage;
	[SerializeField]
	private RenderTexture easyModeTexture, normalModeTexture, hardModeTexture;
	private Coroutine repeatVideosCoroutine;


	private void Start()
	{
		// For synchronize:
		PlayVideos();
		Invoke("StopVideos", 1);
	}

	public void PlayVideos()
	{
		ReleaseRenderTextures(); // Reset image of RenderTextures.
		repeatVideosCoroutine = StartCoroutine(RepeatVideos());
	}

	private IEnumerator RepeatVideos()
	{
		// Create and destroy in loop instead of use "videoPlayer.isLooping = true"
		// because videos have different length in milliseconds:
		while(true)
		{
			#region Create VideoPlayers:
			// Add VideoPlayer to easy mode panel:
			VideoPlayer videoPlayer = panel_EasyModeExampleImage.AddComponent<VideoPlayer>();
			videoPlayer.source = VideoSource.Url;
			videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "EasyMode.mp4");
			videoPlayer.targetTexture = easyModeTexture;
			videoPlayer.audioOutputMode = VideoAudioOutputMode.None;

			// Add VideoPlayer to normal mode panel:
			videoPlayer = panel_NormalModeExampleImage.AddComponent<VideoPlayer>();
			videoPlayer.source = VideoSource.Url;
			videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "NormalMode.mp4");
			videoPlayer.targetTexture = normalModeTexture;
			videoPlayer.audioOutputMode = VideoAudioOutputMode.None;

			// Add VideoPlayer to hard mode panel:
			videoPlayer = panel_HardModeExampleImage.AddComponent<VideoPlayer>();
			videoPlayer.source = VideoSource.Url;
			videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "HardMode.mp4");
			videoPlayer.targetTexture = hardModeTexture;
			videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
			#endregion Create VideoPlayers.

			yield return new WaitForSeconds(17); // Videos length is "16" seconds and "?" milliseconds.

			DestroyVideoPlayers();
		}
	}

	public void StopVideos()
	{
		DestroyVideoPlayers();
		ReleaseRenderTextures(); // Release RenderTextures (they will not be garbage collected).

		if(repeatVideosCoroutine != null)
		{
			StopCoroutine(repeatVideosCoroutine);
		}
	}

	private void DestroyVideoPlayers()
	{
		Destroy(panel_EasyModeExampleImage.GetComponent<VideoPlayer>());
		Destroy(panel_NormalModeExampleImage.GetComponent<VideoPlayer>());
		Destroy(panel_HardModeExampleImage.GetComponent<VideoPlayer>());
	}

	private void ReleaseRenderTextures()
	{
		easyModeTexture.Release();
		normalModeTexture.Release();
		hardModeTexture.Release();
	}
}
