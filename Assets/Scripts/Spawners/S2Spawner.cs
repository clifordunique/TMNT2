using UnityEngine;
using System.Collections;

public class S2Spawner : MonoBehaviour {
	public GameObject PurpleFS;
	public GameObject StarFS;
	public GameObject[] pfs;
	public GameObject[] sfs;
	public bool go = false;
	public Component PattCol;
	public Component PCol;
	public int goTime = 0;
	public MainCameraScript cams;
	public int atter = 1;
	public float screenstop = 384f;
	public float right;
	public float left;
	public int stage = 0;
	public int numEn = 4;
	public int enemiesAlive = 0;

	
	// Use this for initialization
	void Start () {
		PattCol = GameObject.Find("PlayerAttackCollider").GetComponent<BoxCollider2D>();
		PCol = GameObject.Find("Player").GetComponent<BoxCollider2D>();
		cams = GameObject.Find("Main Camera").GetComponent<MainCameraScript>();
		sfs = new GameObject[numEn+1];
		right = screenstop + 32f;
		left = screenstop - (32f + 256f);
	}
	
	// Update is called once per frame
	void Update () {
		if(go){
			if(goTime == 0){
				spawnSFS(1, new Vector3(left, 40f, 40f), 1);
				stage = 1;
			}else if(goTime == 50){
				spawnSFS(2, new Vector3(right, 105f, 105f), 2);
				stage = 2;
			}else if(goTime == 100){
				spawnSFS (3, new Vector3(right, 105f, 105f), 2);
				stage = 3;
			}else if(goTime == 150){
				spawnSFS(4, new Vector3(right, 105f, 105f), 2);
				stage = 4;
				go = false;
			}
			goTime++;
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other == PCol || other == PattCol){
			go = true;
		}
	}
	void spawnPFS(int num, Vector3 vec, int source){
		pfs[num] = Instantiate(PurpleFS, vec, Quaternion.identity) as GameObject;
		PurpleFSScript pfsscript = pfs[num].GetComponent<PurpleFSScript>();
		pfsscript.source = source;
		if (num == atter){
			pfsscript.attacker = true;
		}
		pfsscript.spawner = this;
		pfsscript.num = num;
		cams.enemiesAlive += 1;
		enemiesAlive += 1;
	}
	void PFSdied(int num){
		Destroy(pfs[num]);
		cams.enemiesAlive -= 1;
		enemiesAlive -= 1;
		cams.points += 1;
		pfs[num] = null;
		bool neednew = false;
		if(atter == num){
			neednew = true;
		}
		if(neednew && enemiesAlive > 0){
			bool going = true;
			while(going){
				num++;
				if(numEn >= num){
					if (pfs[num] != null){
						PurpleFSScript npscript = pfs[num].GetComponent<PurpleFSScript>();
						npscript.attacker = true;
						atter = num;
						going = false;
					}
				}else{
					num = 0;
				}
			}
		}else if (neednew && stage != 4) {
			atter = stage + 1;
		}
	}
	void spawnSFS(int num, Vector3 vec, int source){
		sfs[num] = Instantiate(StarFS, vec, Quaternion.identity) as GameObject;
		StarFSScript sfsscript = sfs[num].GetComponent<StarFSScript>();
		sfsscript.source = source;
		sfsscript.screenstop = screenstop;
		if (num == atter){
			sfsscript.attacker = true;
		}
		sfsscript.spawner = this;
		sfsscript.num = num;
		cams.enemiesAlive += 1;
		enemiesAlive += 1;
	}
	void SFSdied(int num){
		Destroy(sfs[num]);
		cams.enemiesAlive -= 1;
		enemiesAlive -= 1;
		cams.points += 1;
		sfs[num] = null;
		bool neednew = false;
		if(atter == num){
			neednew = true;
		}
		if(neednew && enemiesAlive > 0){
			bool going = true;
			bool gone = false;
			while(going){
				num++;
				if(numEn >= num){
					if (sfs[num] != null){
						StarFSScript nsscript = sfs[num].GetComponent<StarFSScript>();
						nsscript.attacker = true;
						atter = num;
						going = false;
					}
				}else{
					gone = true;
					num = 0;
				}
			}
		}else if (neednew && stage != 4) {
			atter = stage + 1;
		}
	}
}

