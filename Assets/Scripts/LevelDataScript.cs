using UnityEngine;
using System.Collections;

public class LevelDataScript : MonoBehaviour {
	
	public float walkSpaceHeight = 105;
	public float levelLength = 1792;

	public float[] cameraStopArray;
	public int cameraStopIndex = 0;

	void Start()
	{
		cameraStopArray = new float[13];

		cameraStopArray[0] = 320;
		cameraStopArray[1] = 384;
		cameraStopArray[2] = 577;
		cameraStopArray[3] = 719;
		cameraStopArray[4] = 898;
		cameraStopArray[5] = 960;
		cameraStopArray[6] = 1123;
		cameraStopArray[7] = 1376;
		cameraStopArray[8] = 1409;
		cameraStopArray[9] = 1503;
		cameraStopArray[10] = 1664;
		cameraStopArray[11] = 1792;
		cameraStopArray[12] = 99999999999;
	}
}
