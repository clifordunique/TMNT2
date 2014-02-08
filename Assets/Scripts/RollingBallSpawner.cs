using UnityEngine;
using System.Collections;

public class RollingBallSpawner : MonoBehaviour {

	public GameObject ballPrefab;

	public GameObject cam;

	public bool spawnBalls = false;

	public float timer1 = 0;
	public float timer2 = .6f;
	
	public float xRangeMin = .2f;
	public float xRangeMax = 1.3f;


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
				ballClone.GetComponent<RollingBallScript>().setValues(Random.Range(xRangeMin, xRangeMax), -2.4f);

				timer1 = 2.6f;
			}

			if(timer2 <= 0)
			{
				ballClone = Instantiate(ballPrefab, this.transform.position, Quaternion.identity) as GameObject;
				ballClone.GetComponent<RollingBallScript>().setValues(Random.Range(xRangeMin, xRangeMax), -2.4f);

				timer2 = 3.2f;
			}
		}
	}
}
