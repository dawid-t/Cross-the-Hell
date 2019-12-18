using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
	[SerializeField]
	private bool firstLevel = true;
	[SerializeField]
	private ParticleSystem[] psToScaleDown = new ParticleSystem[3];
	[SerializeField]
	private ParticleSystem[] psToStopEmission = new ParticleSystem[2];


	private void Start()
	{
		if(firstLevel)
		{
			StartCoroutine(Disappearance());
			GetComponent<Collider2D>().enabled = false;
		}
	}

	private IEnumerator Disappearance()
	{
		yield return new WaitForSeconds(2);

		#region Get 0.8% of sound volume & 1% of start sizes:
		AudioSource audio = GetComponent<AudioSource>();
		float percentOfSound = audio.volume/125;

		float[][] percentOfStartSizes = new float[psToScaleDown.Length][]; // 1% of start size.
		ParticleSystem.MainModule[] mains = new ParticleSystem.MainModule[psToScaleDown.Length];
		for(int i = 0; i < psToScaleDown.Length; i++)
		{
			mains[i] = psToScaleDown[i].main;
			percentOfStartSizes[i] = new float[2];
			percentOfStartSizes[i][0] = mains[i].startSize.constantMin/100;
			percentOfStartSizes[i][1] = mains[i].startSize.constantMax/100;
		}
		#endregion Get 0.8% of sound volume & 1% of start sizes.

		#region Volume down sound by 80% & scale down start sizes by 100%:
		for(int i = 0; i < 100; i++)
		{
			audio.volume -= percentOfSound;
			for(int j = 0; j < psToScaleDown.Length; j++)
			{
				var startSize = mains[j].startSize;
				startSize.constantMin -= percentOfStartSizes[j][0];
				startSize.constantMax -= percentOfStartSizes[j][1];
				mains[j].startSize = startSize;
			}
			yield return new WaitForSeconds(0.2f);
		}
		#endregion Volume down sound by 80% & scale down start sizes by 100%.

		#region Stop emission in other particles:
		yield return new WaitForSeconds(2);
		var emission = psToStopEmission[0].emission;
		emission.rateOverTime = 0;

		emission = psToStopEmission[1].emission;
		emission.rateOverTime = 0;
		#endregion Stop emission in other particles.

		audio.enabled = false;
		Destroy(gameObject, 5);
	}
}
