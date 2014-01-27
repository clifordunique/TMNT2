using UnityEngine;
using System.Collections;

public class RollingBallShadowScript : MonoBehaviour {
	
	GameObject ball;
	RollingBallScript ballScript;

	void Start()
	{
		ball = GameObject.Find("RollingBall");
		ballScript = ball.GetComponent<RollingBallScript>();
	}

	void Update() 
	{
		Vector3 position = ball.transform.position;
		position.y = ballScript.yPos;
		this.transform.position = position;
	}
}
