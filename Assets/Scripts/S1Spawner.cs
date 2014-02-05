using UnityEngine;
using System.Collections;

public class S1Spawner : MonoBehaviour {
	public GameObject PurpleFS;
	public GameObject[] pfs;
	public bool go = false;
	public Component PattCol;
	public Component PCol;
	public int goTime = 0;
	public MainCameraScript cams;
	public int attacker;


	// Use this for initialization
	void Start () {
		PattCol = GameObject.Find("PlayerAttackCollider").GetComponent<BoxCollider2D>();
		PCol = GameObject.Find("Player").GetComponent<BoxCollider2D>();
		cams = GameObject.Find("Main Camera").GetComponent<MainCameraScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(go){
			if(goTime == 0){

			}
			goTime++;
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other == PCol || other == PattCol){
			go = true;
		}
	}
	void spawn(int num, Vector3 vec){
		pfs[num] = Instantiate(PurpleFS, vec, Quaternion.identity) as GameObject;
		PurpleFSScript pfsscript = pfs[num].GetComponent<PurpleFSScript>();
		pfsscript.source = 1;
		if (num == 1){
			pfsscript.attacker = true;
			attacker = 1;
		}
		pfsscript.spawner = this;
		pfsscript.num = num;
		cams.enemiesAlive += 1;
	}
	void FSdied(GameObject pfspass, int num){
		Destroy(pfspass);
		cams.enemiesAlive -= 1;
		pfs[num] = null;
		if(attacker == num && cams.enemiesAlive > 0){
			bool going = true;
			while(going){
				num++;
				if (pfs[num] != null){
					PurpleFSScript npscript = pfs[num].GetComponent<PurpleFSScript>();
					npscript.attacker = true;
					attacker = num;
					going = false;
				}
			}
		}
	}
}
