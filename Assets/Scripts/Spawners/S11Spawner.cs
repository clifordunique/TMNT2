using UnityEngine;
using System.Collections;

public class S11Spawner : MonoBehaviour {
	public GameObject PurpleFS;
	public GameObject StarFS;
	public GameObject BlueFS;
	public GameObject[] pfs;
	public GameObject[] sfs;
	public GameObject[] bfs;
	public bool go1 = false;
	public bool go2 = false;
	public Component PattCol;
	public Component PCol;
	public int goTime = 0;
	public MainCameraScript cams;
	public int atter = 1;
	public float screenstop = 1664f;
	public float right;
	public float left;
	public int stage = 0;
	public int numEn = 4;
	public bool done1 = false;
	public bool done2 = false;
	public bool go3 = false;
	public int goTime2 = 0;
	public Animator dooranimB_1;
	public Animator dooranimB_2;
	public GameObject pfsstandB_1;
	public GameObject pfsstandB_2;
	public bool doordone1 = false;
	public bool doordone2 = false;
	public int enemiesAlive = 0;
	public bool stopSpawn1 = false;
	public bool stopSpawn2 = false;
	
	
	// Use this for initialization
	void Start () {
		cams = GameObject.Find("Main Camera").GetComponent<MainCameraScript>();
		sfs = new GameObject[numEn+1];
		right = screenstop + 32f;
		left = screenstop - (32f + 256f);
		dooranimB_1 = GameObject.Find("DoorB_1").GetComponent<Animator>();
		dooranimB_2 = GameObject.Find("DoorB_2").GetComponent<Animator>();
		pfsstandB_1 = GameObject.Find("pfsstandB_1");
		pfsstandB_2 = GameObject.Find("pfsstandB_2");
	}
	
	// Update is called once per frame
	void Update () {
		if (dooranimB_1 != null){
			doordone1 = dooranimB_1.GetCurrentAnimatorStateInfo(0).IsName("DoorBEnd");
		}
		if (dooranimB_2 != null){
			doordone2 = dooranimB_2.GetCurrentAnimatorStateInfo(0).IsName("DoorBEnd");
		}
		if(!go1 && doordone1 && !stopSpawn1){
			go1 = true;
			stopSpawn1 = true;
			Destroy (pfsstandB_1);
			stage++;
			spawnSFS(stage, new Vector3(1458f,114f,114f), 3);

		}
		if(!go2 && doordone2 && !stopSpawn2){
			go2 = true;
			stopSpawn2 = true;
			Destroy (pfsstandB_2);
			stage++;
			spawnSFS(stage, new Vector3(1587f,114f,114f), 3);
		}
		if(go1 && go2){
			if(goTime == 0){
				spawnSFS(3, new Vector3(right, 105f, 105f), 2);
				stage = 3;
			}else if(goTime == 50){
				spawnSFS(4, new Vector3(left, 105f, 105f),1);
				stage = 4;
			}else if(goTime == 50){
				spawnSFS(5, new Vector3(left, 60f, 60f),1);
				stage = 5;
			}else if(goTime == 50){
				spawnSFS(6, new Vector3(right, 105f, 105f),2);
				stage = 6;
				go1 = false;
				go2 = false;
				done1 = true;
			}
			goTime++;
		}
		/*if(go3){
			if(goTime2 == 0){
				spawnBFS (1, new Vector3(right, 105f, 105f), 2);
				stage = 4;
				go3 = false;
				done2 = true;
			}
			goTime2++;
		}*/
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
		if (enemiesAlive == 0 && done1 == true && done2 == false) {
			go3 = true;
			atter = 1;
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
		}if (enemiesAlive == 0 && done1 == true && done2 == false) {
			go3 = true;
			atter = 1;
		}
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
		}if (enemiesAlive == 0 && done1 == true && done2 == false) {
			go3 = true;
			atter = 1;
		}
	}
}

