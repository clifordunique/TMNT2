using UnityEngine;
using System.Collections;

public class InitSpawn : MonoBehaviour {
	public GameObject PurpleFS;
	public GameObject pfs1;
	public PurpleFSScript pfsscript;
	public MainCameraScript cams;
	// Use this for initialization
	void Start () {
		pfs1 = Instantiate(PurpleFS, new Vector3(-22f,114f,114f), Quaternion.identity) as GameObject;
		pfsscript = pfs1.GetComponent<PurpleFSScript>();
		pfsscript.source = 1;
		pfsscript.attacker = true;
		pfsscript.spawner = this;
		pfsscript.num = 1;
		cams = GameObject.Find("Main Camera").GetComponent<MainCameraScript>();
		cams.enemiesAlive += 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void FSdied(int num){
		Destroy(pfs1);
		cams.enemiesAlive -= 1;
	}
}
