using UnityEngine;
using System.Collections;

public class MainCameraScript : MonoBehaviour {
	public bool stopMovement = false;

	private LevelDataScript levelData;
	private GameObject player;

	void Start()
	{
		levelData = GameObject.Find("LevelData").GetComponent<LevelDataScript>();
		player = GameObject.Find("Player");
	}

	void Update()
	{
		if(this.transform.position.x > levelData.levelLength - (256 / 2))
		{
			stopMovement = true;
		}
		
		if(!stopMovement)
		{
			float dist = (transform.position - Camera.main.transform.position).z;
			float threshold = Camera.main.ViewportToWorldPoint(new Vector3(.6f, 0, dist)).x;
			if(player.transform.position.x > threshold)
			{
				PlayerScript playerScript = player.GetComponent<PlayerScript>();
				if(playerScript.deltaX > 0)
				{
					Vector3 position = transform.position;
					position.x += playerScript.deltaX;
					transform.position = position;
				}
			}
		}
	}
}
