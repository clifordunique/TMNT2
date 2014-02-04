using UnityEngine;
using System.Collections;

public class RollingBallSpawner : MonoBehaviour {

	public GameObject ballPrefab;

	public GameObject cam;

	private bool spawnBalls = false;

	private float timer1 = 0;
	private float timer2 = .5f;

	void Start()
	{
		cam = GameObject.Find("Main Camera");
	}

	void Update()
	{
		if(cam.transform.position.x + 25 > transform.position.x - 20 && !spawnBalls)
		{
			spawnBalls = true;
		}

		if(cam.transform.position.x > transform.position.x + 256)
		{
			Destroy(this.gameObject);
		}

		if(spawnBalls)
		{
			GameObject ballClone;
			timer1 -= Time.deltaTime;
			timer2 -= Time.deltaTime;

			if(timer1 <= 0)
			{
				ballClone = Instantiate(ballPrefab, this.transform.position, Quaternion.identity) as GameObject;
				ballClone.GetComponent<RollingBallScript>().setValues(Random.Range(.2f, 1.3f), -2.4f);

				timer1 = 2;
			}

			if(timer2 <= 0)
			{
				ballClone = Instantiate(ballPrefab, this.transform.position, Quaternion.identity) as GameObject;
				ballClone.GetComponent<RollingBallScript>().setValues(Random.Range(.2f, 1.3f), -2.4f);

				timer2 = 3f;
			}
		}
	}
}
