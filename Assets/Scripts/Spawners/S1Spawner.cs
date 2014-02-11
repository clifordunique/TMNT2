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
	public int atter = 1;
	public float screenstop = 320f;
	public float right;
	public float left;
	public int stage = 0;
	public int enemiesAlive = 0;

	// Use this for initialization
	void Start () {
		PattCol = GameObject.Find("PlayerAttackCollider").GetComponent<BoxCollider2D>();
		PCol = GameObject.Find("Player").GetComponent<BoxCollider2D>();
		cams = GameObject.Find("Main Camera").GetComponent<MainCameraScript>();
		pfs = new GameObject[5];
		right = screenstop + 32f;
		left = screenstop - (32f + 256f);
	}
	
	// Update is called once per frame
	void Update () {
		if(go){
			if(goTime == 0){
				spawn(1, new Vector3(right, 105f, 105f), 2);
				stage = 1;
			}else if(goTime == 50){
				spawn(2, new Vector3(left, 80f, 80f), 1);
				stage = 2;
			}else if(goTime == 100){
				spawn (3, new Vector3(left, 40f, 40f), 1);
				stage = 3;
			}else if(goTime == 150){
				spawn(4, new Vector3(right, 105f, 105f), 1);
				stage = 4;
				go = false;
			}
			goTime++;
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if ((other == PCol || other == PattCol)){
			go = true;
		}
	}
	void spawn(int num, Vector3 vec, int source){
		pfs[num] = Instantiate(PurpleFS, vec, Quaternion.identity) as GameObject;
		PurpleFSScript pfsscript = pfs[num].GetComponent<PurpleFSScript>();
		pfsscript.source = source;
		if (num == atter){
			pfsscript.attacker = true;
		}
		pfsscript.spawner = this;
		pfsscript.num = num;
		enemiesAlive += 1;
		cams.enemiesAlive += 1;
	}
	void PFSdied(int num){
		Destroy(pfs[num]);
		enemiesAlive -= 1;
		cams.enemiesAlive -= 1;
		cams.points += 1;
		pfs[num] = null;
		if(atter == num && enemiesAlive > 0){
			bool going = true;
			while(going){
				num++;
				if (pfs[num] != null){
					PurpleFSScript npscript = pfs[num].GetComponent<PurpleFSScript>();
					npscript.attacker = true;
					atter = num;
					going = false;
				}
			}
		}else if (stage != 4) {
			atter = stage + 1;
		}
	}
}
