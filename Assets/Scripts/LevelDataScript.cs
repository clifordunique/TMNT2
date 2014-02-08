using UnityEngine;
using System.Collections;

public class LevelDataScript : MonoBehaviour {
	
	public float walkSpaceHeight = 105;
	public float walkSpaceBottom = 15;
	public float levelLength = 1792;

	public float[] cameraStopArray;
	public int cameraStopIndex = 0;

	public float songLoopStart = 43.077f;
	public float songLoopEnd = 80.353f;

	public float stop1 = 320;
	public float stop2 = 384;
	public float stop3 = 577;
	public float stop4 = 719;
	public float stop5 = 898;
	public float stop6 = 960;
	public float stop7 = 1123;
	public float stop8 = 1376;
	public float stop9 = 1409;
	public float stop10 = 1503;
	public float stop11 = 1664;
	public float stop12 = 1792;
	
	void Start()
	{
		cameraStopArray = new float[13];

		cameraStopArray[0] = stop1;
		cameraStopArray[1] = stop2;
		cameraStopArray[2] = stop3;
		cameraStopArray[3] = stop4;
		cameraStopArray[4] = stop5;
		cameraStopArray[5] = stop6;
		cameraStopArray[6] = stop7;
		cameraStopArray[7] = stop8;
		cameraStopArray[8] = stop9;
		cameraStopArray[9] = stop10;
		cameraStopArray[10] = stop11;
		cameraStopArray[11] = stop12;
		cameraStopArray[12] = 99999999999;
	}
}
