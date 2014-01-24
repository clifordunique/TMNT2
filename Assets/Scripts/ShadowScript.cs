using UnityEngine;
using System.Collections;

public class ShadowScript : MonoBehaviour {
	private SpriteRenderer renderer;

	void Start() 
	{
		renderer = this.GetComponent<SpriteRenderer>();
	}

	void Update() 
	{
		GameObject player = GameObject.Find("Player");
		PlayerScript playerScript = player.GetComponent<PlayerScript>();

		if(playerScript.jumpPos == 0)
		{
			this.renderer.enabled = false;
		}
		else
		{
			this.renderer.enabled = true;
		}

		Vector3 position = player.transform.position;
		position.y = position.z;
		this.transform.position = position;
	}
}
