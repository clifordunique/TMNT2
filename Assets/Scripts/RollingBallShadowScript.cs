using UnityEngine;
using System.Collections;

public class RollingBallShadowScript : MonoBehaviour {

	void Update() 
	{
		GameObject ball = GameObject.Find("RollingBall");
		RollingBallScript ballScript = ball.GetComponent<RollingBallScript>();
		Vector3 position = ball.transform.position;
		position.y = ballScript.yPos;
		this.transform.position = position;
	}
}
