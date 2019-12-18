using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;


/* CLASS DESCRIPTION:
 * 
 * This class record the player behaviour (movement and changing dimension) for create videos to Main Menu difficulty panel.
 * If you want to PLAYBACK recorded data then you have to:
 * 1. Remove temporarily code from Update() in Weapon class,
 * 2. Change methods to public in Weapon class,
 * 3. Remove temporarily code from FixedUpdate() in PlayerMovement class responsible for animation,
 * 4. In ShowWholeSecondDimension class remove temporarily the code responsible for disable/enable Player scripts.
 * 5. Set Player rigidbody to kinematic.
 */

public class BotForVideo : MonoBehaviour
{
	[SerializeField]
	private bool record = false;
	private bool stop = false, weaponUsed = false;
	[SerializeField]
	private Transform player;
	private Weapon weapon;
	private RecordedData recordedData;


	public bool Record { get { return record; } set { record = value; } }
	public Transform Player { get { return player; } set { player = value; } }


	[Serializable]
	public class RecordedData
	{
		private List<float> playerWeaponUsageTime; // 1st value - start use time, 2nd value - stop use time, 3rd value - start use time, etc...
		private List<SerializableVector2> playerPositionsList;

		
		public List<float> PlayerWeaponUsageTime { get { return playerWeaponUsageTime; } set { playerWeaponUsageTime = value; } }
		public List<SerializableVector2> PlayerPositionsList { get { return playerPositionsList; } set { playerPositionsList = value; } }


		[Serializable]
		public struct SerializableVector2 // Vector2 can't be serialized via BinnaryFormater so create new struct with X and Y coordinates.
		{
			public float x;
			public float y;

			public SerializableVector2(Vector2 vector2)
			{
				x = vector2.x;
				y = vector2.y;
			}

			public SerializableVector2(Vector3 vector3)
			{
				x = vector3.x;
				y = vector3.y;
			}

			public Vector2 GetVector2()
			{
				return new Vector2(x, y);
			}
		}
	}


	private void Start()
	{
		if(record)
		{
			recordedData = new RecordedData();
			recordedData.PlayerWeaponUsageTime = new List<float>();
			recordedData.PlayerPositionsList = new List<RecordedData.SerializableVector2>();
			StartCoroutine(RecordPlayerMovement());
		}
		else
		{
			LoadRecordedData();
			StartCoroutine(PlaybackPlayerWeaponUsage());
			StartCoroutine(PlaybackPlayerMovement());
		}
	}

	private void Update()
	{
		if(record)
		{
			RecordPlayerWeaponUsage();
		}
		else if(!stop)
		{
			if(weaponUsed)
			{
				//weapon.IncreaseDimensionRange(); // By default this method is private.
			}
			else
			{
				//weapon.DecreaseDimensionRange(); // By default this method is private.
			}
		}
	}

	public void Stop() // Stop record or playback.
	{
		stop = true;
		if(record)
		{
			SaveRecordedData();
		}
	}

	#region Playback:
	private IEnumerator PlaybackPlayerWeaponUsage()
	{
		if(recordedData.PlayerWeaponUsageTime == null) // If list doesn't exists then do nothing.
		{
			Debug.Log("Player weapon usage record doesn't exists!");
			yield break;
		}

		weapon = player.GetComponent<Weapon>(); // While playback change the methods in this script to public.
		float lastTime = 0;
		for(int i = 0; i < recordedData.PlayerWeaponUsageTime.Count && !stop; i++)
		{
			yield return new WaitForSeconds(recordedData.PlayerWeaponUsageTime[i] - lastTime);
			lastTime = recordedData.PlayerWeaponUsageTime[i];

			if(i % 2 == 0)
			{
				weaponUsed = true;
			}
			else
			{
				weaponUsed = false;
			}
		}
	}

	private IEnumerator PlaybackPlayerMovement()
	{
		if(recordedData.PlayerPositionsList == null) // If list doesn't exists then do nothing.
		{
			Debug.Log("Player movement record doesn't exists!");
			yield break;
		}
		yield return new WaitForSeconds(8); // Animation time showing the whole second dimension.

		Animator animator = player.Find("Character").GetComponent<Animator>();
		for(int i = 0; i < recordedData.PlayerPositionsList.Count && !stop; i++)
		{
			Vector2 lastPlayerPosition = player.position;
			player.position = recordedData.PlayerPositionsList[i].GetVector2();

			#region Rotation & walk animation:
			if(lastPlayerPosition.x < player.position.x)
			{
				player.rotation = Quaternion.Euler(0, 0, 0);
				animator.SetBool("Run", true);
			}
			else if(lastPlayerPosition.x > player.position.x)
			{
				player.rotation = Quaternion.Euler(0, 180, 0);
				animator.SetBool("Run", true);
			}
			else
			{
				animator.SetBool("Run", false);
			}
			#endregion Rotation & walk animation.
			yield return new WaitForSeconds(0.02f); // Record the player position every 50fps.
		}
		animator.SetBool("Run", false); // In case when playback is stopped.
		Debug.Log("Playback has ended!");
	}
	#endregion Playback.

	#region Record:
	private void RecordPlayerWeaponUsage()
	{
		if(!stop)
		{
			if(Input.GetMouseButtonDown(1))
			{
				recordedData.PlayerWeaponUsageTime.Add(Time.timeSinceLevelLoad);
			}
			else if(Input.GetMouseButtonUp(1))
			{
				recordedData.PlayerWeaponUsageTime.Add(Time.timeSinceLevelLoad);
			}
		}
	}

	private IEnumerator RecordPlayerMovement()
	{
		yield return new WaitForSeconds(8); // Animation time showing the whole second dimension.

		while(!stop)
		{
			recordedData.PlayerPositionsList.Add(new RecordedData.SerializableVector2(player.position));
			yield return new WaitForSeconds(0.02f); // Record the player position every 50fps.
		}
		Debug.Log("Record has ended! List count: "+recordedData.PlayerPositionsList.Count);
	}
	#endregion Record.

	#region Save & load data:
	private void SaveRecordedData()
	{
		try
		{
			string path = Path.Combine(Application.persistentDataPath, "Data");
			if(!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			path += "/bot_for_video.dat";

			FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
			BinaryFormatter binFormatter = new BinaryFormatter();

			binFormatter.Serialize(stream, recordedData);
			stream.Close();
		}
		catch(Exception e)
		{
			Debug.Log("Can't Save Data"+e.Message);
			return;
		}
	}

	private void LoadRecordedData()
	{
		try
		{
			string path = Path.Combine(Application.persistentDataPath, "Data/bot_for_video.dat");
			if(File.Exists(path))
			{
				FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
				BinaryFormatter binFormatter = new BinaryFormatter();

				recordedData = (RecordedData)binFormatter.Deserialize(stream);
				stream.Close();
			}
		}
		catch
		{
			Debug.Log("Can't Load Data");
			return;
		}
	}
	#endregion Save & load data.
}
