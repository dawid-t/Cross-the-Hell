using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
	private static CameraShake instance;

	private float shakeForce;
	private Vector3 camPos;
	//[SerializeField]
	//private CinemachineVirtualCamera vCam;
	//private CinemachineBasicMultiChannelPerlin noise;


	public static CameraShake Instance { get { return instance; } }


	private void Awake()
	{
		instance = this;
		camPos = transform.position;

		//noise = vCam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		//noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		//Debug.Log(noise);
		//Noise(10, 10);
	}

	/*public void Noise(float amplitudeGain, float frequencyGain)
	{
		noise.m_AmplitudeGain = amplitudeGain;
		noise.m_FrequencyGain = frequencyGain;
	}*/

	public void StartShake()
	{
		shakeForce = 5;
		InvokeRepeating("ShakeForceDown", 0.35f, 0.35f);
		StartCoroutine(Shake());
	}

	private void ShakeForceDown()
	{
		shakeForce -= 5;
		if(shakeForce == 0)
		{
			CancelInvoke("ShakeForceDown");
		}
	}

	private IEnumerator Shake()
	{
		float perlinNoise;
		while(shakeForce != 0)
		{
			perlinNoise = Mathf.PerlinNoise(Time.time*shakeForce, 0f);
			transform.position = new Vector3(camPos.x+perlinNoise, camPos.y+perlinNoise, camPos.z+Mathf.PerlinNoise(Time.time*shakeForce, Time.time*shakeForce));
			yield return new WaitForFixedUpdate();
		}
		perlinNoise = Mathf.PerlinNoise(Time.time*shakeForce, 0f);
		transform.position = new Vector3(camPos.x+perlinNoise, camPos.y+perlinNoise, camPos.z+Mathf.PerlinNoise(Time.time*shakeForce, Time.time*shakeForce));
		StartCoroutine(EndShake());
	}

	private IEnumerator EndShake()
	{
		while(transform.position != camPos)
		{
			transform.position = Vector3.Lerp(transform.position, camPos, 0.125f);
			yield return new WaitForFixedUpdate();
		}
	}
}
