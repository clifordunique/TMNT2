using UnityEngine;
using System.Collections;

public class MainCameraScript : MonoBehaviour {
	public bool stopMovement = false;

	void Update()
	{
		if(this.transform.position.x > 1792 - (256 / 2))
		{
			stopMovement = true;
		}
		
		if(!stopMovement)
		{
			GameObject player = GameObject.Find("Player");
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
