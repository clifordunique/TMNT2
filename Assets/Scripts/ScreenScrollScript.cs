using UnityEngine;
using System.Collections;

public class ScreenScrollScript : MonoBehaviour {

	void OnTriggerStay2D(Collider2D collider)
	{
		if(collider.gameObject.name == "Player")
		{
			GameObject player = GameObject.Find("Player");
			PlayerScript playerScript = player.GetComponent<PlayerScript>();
			if(playerScript.deltaX > 0)
			{
				GameObject camera = GameObject.Find("Main Camera");
				Vector3 position = camera.transform.position;
				position.x += playerScript.deltaX;
				camera.transform.position = position;
			}
		}
	}
}
