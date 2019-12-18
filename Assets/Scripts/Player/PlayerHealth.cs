using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	private bool immunity = false, dead = false, cutscene = false;
	private int hp = 1000;
	[SerializeField]
	private Text text_Hp;
	[SerializeField]
	private Image image_Hp;
	[SerializeField]
	private PostProcessProfile postProcessProfile;
	private Vignette vignette;


	public bool Dead { set { dead = value; } }
	public bool Immunity { get { return immunity; } }
	public bool Cutscene { set { cutscene = value; } }
	public int Hp { get { return hp; } }


	private void Awake()
	{
		vignette = postProcessProfile.GetSetting<Vignette>();
		vignette.intensity.value = 0;
	}

	private void OnDestroy()
	{
		vignette.intensity.value = 0;
	}

	public void SubtractHp(int value)
	{
		hp -= value;
		CheckHealth();
	}

	private void CheckHealth()
	{
		if(hp < 10) // Under 1%.
		{
			hp = 0;
			if(!dead)
			{
				dead = true;
				StopAllCoroutines();
				immunity = true;

				Animator animator = transform.Find("Character").GetComponent<Animator>();
				animator.SetBool("ChangeDimension", false);
				animator.Play("Death", 0, 0);
				GetComponents<AudioSource>()[1].Play();
				GetComponent<PlayerMovement>().enabled = false;

				Rigidbody2D rb = GetComponent<Rigidbody2D>();
				rb.velocity = new Vector2(0, rb.velocity.y);

				if(!cutscene)
				{
					StartCoroutine(RestartScene());
				}
				else
				{
					EndCutscene.Instance.OpenTeleport();
				}
			}
		}
		text_Hp.text = (hp/10)+"%";
		image_Hp.fillAmount = (float)hp/1000;
		vignette.intensity.value = 0.5f - (float)hp/1000;
	}

	public void SetTemporaryImmunity(bool value)
	{
		immunity = value;
		StartCoroutine(DisableImmunity());
	}

	private IEnumerator DisableImmunity()
	{
		yield return new WaitForSeconds(1);
		immunity = false;
	}

	private IEnumerator RestartScene()
	{
		yield return new WaitForSeconds(2);
		SceneChanger.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);
	}
}
