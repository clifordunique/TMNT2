using UnityEngine;
using System.Collections;

public class LevelSelectScript : MonoBehaviour {

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(Screen.width/2 - 62.5f, Screen.height/2 - 62.5f, Screen.width, Screen.height));
		GUI.Box(new Rect(0, 0, 150, 200), "Select Level");
		if(GUI.Button(new Rect(10, 30, 125, 30), "Level 1"))
		{
			Application.LoadLevel("_Level1");
		}
		if(GUI.Button(new Rect(10, 70, 125, 30), "Custom Level"))
		{
			Application.LoadLevel("_CustomLevel");
		}
		GUI.EndGroup();
	}
}
