using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
	private static bool playerReturned = false;
	private static int level;

	[SerializeField]
	private bool exit = true;
	[SerializeField]
	private Transform playerSpawn;


	private void Awake()
	{
		level = SceneManager.GetActiveScene().buildIndex;

		GameObject player = GameObject.FindWithTag("Player");
		if((!playerReturned && !exit) || (playerReturned && exit)) // Spawn player on "StartDoor" or on "ExitDoor".
		{
			player.transform.position = playerSpawn.position;
			player.transform.rotation = playerSpawn.rotation;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag.Equals("Player"))
		{
			if(exit)
			{
				if(level < SceneManager.sceneCountInBuildSettings-1)
				{
					playerReturned = false;
					//SceneManager.LoadScene(level+1);
					SceneChanger.Instance.ChangeScene(level+1);
				}
			}
			/*else
			{
				if(level > 1)
				{
					playerReturned = true;
					//SceneManager.LoadScene(level-1);
					SceneChanger.Instance.ChangeScene(level-1);
				}
			}*/
		}
	}
}
