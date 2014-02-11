using UnityEngine;
using System.Collections;

public class S6Spawner : MonoBehaviour {
	public GameObject PurpleFS;
	public GameObject StarFS;
	public GameObject BlueFS;
	public GameObject[] pfs;
	public GameObject[] sfs;
	public GameObject[] bfs;
	public bool go = false;
	public Component PattCol;
	public Component PCol;
	public int goTime = 0;
	public MainCameraScript cams;
	public int atter = 1;
	public float screenstop = 960f;
	public float right;
	public float left;
	public int stage = 0;
	public int numEn = 4;
	public bool done1 = false;
	public bool done2 = false;
	public bool go2 = false;
	public int goTime2 = 0;
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
				spawnSFS(1, new Vector3(right, 105f, 105f), 2);
				stage = 1;
			}else if(goTime == 50){
				spawnSFS(2, new Vector3(left, 105f, 105f), 1);
				stage = 2;
			} else if(goTime == 150){
				spawnSFS (3, new Vector3(left, 60f, 60f), 1);
				stage = 3;
			}else if(goTime == 150){
				spawnSFS (4, new Vector3(left, 105f, 105f), 1);
				stage = 4;
			}else if(goTime == 150){
				spawnSFS (5, new Vector3(right, 60f, 60f), 2);
				stage = 5;
			}else if(goTime == 150){
				spawnSFS (6, new Vector3(right, 40f, 40f), 2);
				stage = 6;
				go = false;
				done1 = true;
			}
			goTime++;
		}
		/*if(go2){
			if(goTime2 == 0){
				spawnBFS (3, new Vector3(right, 40f, 40f), 2);
				stage = 1;
			}else if(goTime2 == 50){
				spawnBFS(4, new Vector3(left, 105f, 105f), 1);
				stage = 2;
			}else if(goTime2 == 100){
				spawnSFS (1, new Vector3(left, 40f, 40f), 1);
				stage = 3;
			}else if(goTime2 == 150){
				spawnSFS (2, new Vector3(right, 105f, 105f), 2);
				stage = 4;
			}
			goTime2++;
		}*/
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
		/*if (cams.enemiesAlive == 0 && done1 == true && done2 == false) {
			go2 = true;
		}*/
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
		}/*if (cams.enemiesAlive == 0 && done1 == true && done2 == false) {
			go2 = true;
		}*/
	}
	void spawnBFS(int num, Vector3 vec, int source){
		bfs[num] = Instantiate(BlueFS, vec, Quaternion.identity) as GameObject;
		BlueFSScript bfsscript = bfs[num].GetComponent<BlueFSScript>();
		bfsscript.source = source;
		bfsscript.screenstop = screenstop;
		if (num == atter){
			bfsscript.attacker = true;
		}
		bfsscript.spawner = this;
		bfsscript.num = num;
		cams.enemiesAlive += 1;
		enemiesAlive += 1;
	}
	void BFSdied(int num){
		Destroy(bfs[num]);
		cams.enemiesAlive -= 1;
		enemiesAlive -= 1;
		cams.points += 1;
		bfs[num] = null;
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
					if (bfs[num] != null){
						BlueFSScript bsscript = bfs[num].GetComponent<BlueFSScript>();
						bsscript.attacker = true;
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
		}/*if (cams.enemiesAlive == 0 && done1 == true && done2 == false) {
			go2 = true;
		}*/
	}
}


