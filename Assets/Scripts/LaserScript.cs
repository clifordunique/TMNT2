using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

	public float deltaX = 2f;

	private float distance = 0;

	void Update() 
	{
		Vector3 position = this.transform.position;
		position.x += deltaX;
		this.transform.position = position;

		distance += deltaX;
		if(distance > 256)
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.transform.position.z >= gameObject.transform.position.z + 10
		   || coll.gameObject.transform.position.z <= gameObject.transform.position.z - 10)
		{
			return;
		}

		if(coll.gameObject.name == "Player")
		{
			this.collider2D.enabled = false;
		}
	}
}
