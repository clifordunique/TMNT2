using UnityEngine;
using System.Collections;

public class InitSpawn : MonoBehaviour {
	public GameObject PurpleFS;
	public GameObject pfs1;
	public PurpleFSScript pfsscript;
	// Use this for initialization
	void Start () {
		pfs1 = Instantiate(PurpleFS, new Vector3(-22f,114f,114f), Quaternion.identity) as GameObject;
		pfsscript = pfs1.GetComponent<PurpleFSScript>();
		pfsscript.source = 1;
		pfsscript.attacker = true;
		MainCameraScript cams = GameObject.Find("Main Camera").GetComponent<MainCameraScript>();
		cams.enemiesAlive += 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void FSdied(){

	}
}
