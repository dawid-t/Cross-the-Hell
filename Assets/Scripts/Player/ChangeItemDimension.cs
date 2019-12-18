using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeItemDimension : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag.Equals("Box") || collision.gameObject.name.Equals("Enemy-MiniBoss"))
		{
			collision.gameObject.layer = 9; // Second dimension layer.
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.tag.Equals("Box") || collision.gameObject.name.Equals("Enemy-MiniBoss"))
		{
			collision.gameObject.layer = 8; // Default layer.
		}
	}
}
