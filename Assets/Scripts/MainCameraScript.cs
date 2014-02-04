using UnityEngine;
using System.Collections;

public class MainCameraScript : MonoBehaviour {
	public bool stopMovement = false;

	public int enemiesAlive = 0;

	private LevelDataScript levelData;
	private GameObject player;
	private AudioSource audio;

	void Start()
	{
		audio = GetComponent<AudioSource>();
		levelData = GameObject.Find("LevelData").GetComponent<LevelDataScript>();
		player = GameObject.Find("Player");
	}

	void Update()
	{
		if(audio.time >= levelData.songLoopEnd)
		{
			audio.time = levelData.songLoopStart;
		}

		if(enemiesAlive > 0)
		{
			if(this.transform.position.x > levelData.cameraStopArray[levelData.cameraStopIndex] - 128)
			{
				stopMovement = true;
				levelData.cameraStopIndex += 1;
			}
		}
		else
		{
			if(this.transform.position.x < levelData.levelLength - 128)
			{
				stopMovement = false;
			}
			else
			{
				stopMovement = true;
			}
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
