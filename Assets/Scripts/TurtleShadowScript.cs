using UnityEngine;
using System.Collections;

public class TurtleShadowScript : MonoBehaviour {
	private GameObject player;
	private PlayerScript playerScript;

	void Start() 
	{
		player = GameObject.Find("Player");
		playerScript = player.GetComponent<PlayerScript>();
	}

	void Update() 
	{
		if(playerScript.jumpPos < 5)
		{
			this.gameObject.renderer.enabled = false;
		}
		else
		{
			this.gameObject.renderer.enabled = true;
		}

		Vector3 position = player.transform.position;
		position.y = playerScript.yPos;
		this.transform.position = position;
	}
}
