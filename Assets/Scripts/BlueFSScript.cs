using UnityEngine;
using System.Collections;

public class BlueFSScript : MonoBehaviour {
	public int life = 2;
	public int gothit = 0;
	public GameObject kunai;
	
	public bool attacking = false;
	private static float bodylength = 22;
	//Probably should fix that to be updated if it changes
	private float punchdist = 34;
	public bool attacker = false;
	private float rdir = 0;
	private float rtime = 0;
	public Component spawner;
	public int num;
	public float screenstop;
	
	
	
	public float deltaX = 0;
	private float deltaY = 0;
	
	public float yPos = 0;
	
	
	private BoxCollider2D attackCollider;
	private PlayerScript player;
	private LevelDataScript levelData;
	
	private int entering = 5;
	
	//1 from left, 2 right, 3 door
	public int source;
	
	private Animator animator;
	public float hitCooldown = 0;
	private float attackCooldown = 0;
	private float disableCollider = 0;
	private float throwCooldown = 0;
	
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		yPos = transform.position.y;
		levelData = GameObject.Find("LevelData").GetComponent<LevelDataScript>();
		attackCollider = this.transform.FindChild("BFSAttColl").GetComponent<BoxCollider2D>();
		player = GameObject.Find("Player").GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.Find("Player");
		PlayerScript playerScript = player.GetComponent<PlayerScript>();
		float playerx = player.transform.position.x;
		float x = transform.position.x;
		float playery = playerScript.yPos;
		float xdist = Mathf.Abs(playerx-x);
		float ydist = Mathf.Abs(playery-yPos);
		//animator.SetBool ("Walking", false);
		
		
		deltaX = 0;
		deltaY = 0;
		
		
		if(hitCooldown > 0)
		{
			attacking = false;
			animator.SetBool("Attacking", false);
			attackCollider.enabled = false;
		}
		
		if(disableCollider <= 0)
		{
			attackCollider.enabled = false;
		}
		
		if(!attacking && hitCooldown <= 0)
		{
			if(xdist <= punchdist && ydist <= 5 && attackCooldown <= 0){
				attackCooldown = 1.1f;
				punch();
			}else if(attacker){
				BlueFSInput(x, playerx, playery, xdist, ydist);
			}else if(rtime == 0) {
				wander();
			}else{
				if(rdir == 1){
					deltaX += .7f;
					animator.SetBool ("Walking", true);
				}else{
					deltaX += -.7f;
					animator.SetBool ("Walking", true);
				}
				rtime--;
			}
		}
		
		if(attacking)
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("PunchEnd")){
				attacking = false;
				animator.SetBool ("Attacking", false);
				attackCollider.enabled = false;
			}
		}
		
		BlueFSMovement();
		
		disableCollider -= Time.deltaTime;
		attackCooldown -= Time.deltaTime;
		hitCooldown -= Time.deltaTime;
		throwCooldown -= Time.deltaTime;
		
		if(hitCooldown <= 0){
			animator.SetBool("Hit", false);
		}
	}
	void BlueFSMovement()
	{
		
		//Using Z axis for Isometric effect
		float z = yPos + deltaY;
		
		if(z > levelData.walkSpaceHeight)
		{
			z = levelData.walkSpaceHeight;
		}
		else if(z < levelData.walkSpaceBottom)
		{
			z = levelData.walkSpaceBottom;
		}
		z = Mathf.Round(z*10f)/10f;
		yPos = z;
		
		float y = z;
		
		float x = transform.position.x + deltaX;
		//y = Mathf.Round(y*10f)/10f;
		x = Mathf.Round(x*10f)/10f;
		if (deltaX < 0 && transform.localScale.x > 0)
			transform.localScale = new Vector3(-1f, 1f, 1f);
		else if (deltaX > 0 && transform.localScale.x < 0)
			transform.localScale = new Vector3(1f, 1f, 1f);
		if ((deltaX != 0 || deltaY != 0)){
			animator.SetBool ("Walking", true);
		}
		
		transform.position = new Vector3(x, y, z);
	}
	void BlueFSInput(float x, float playerx, float playery, float xdist, float ydist)
	{
		if(playerx > (x + punchdist))
		{
			deltaX += .7f;
		}
		
		if(playerx < (x - punchdist))
		{
			deltaX += -.7f;
		}
		
		if(ydist >= (xdist-punchdist) && playery > yPos+5)
		{
			deltaY += .6f;
		}
		
		if(ydist >= (xdist-punchdist) && playery < yPos - 5)
		{
			deltaY += -.6f;
		}
		
		if((ydist < 8) && (xdist > punchdist + .7f) && throwCooldown <= 0)
		{
			throwCooldown = 3;
			throwKunai();
		}
	}
	void wander(){
		float dir = Random.value;
		
		if (dir <= .5){
			rdir = 1;
			deltaX += .7f;
			animator.SetBool ("Walking", true);
			rtime = 50;
		}else{
			rdir = -1;
			deltaX += -.7f;
			animator.SetBool ("Walking", true);
			rtime = 50;
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		//Objects are not in the same relative z position
		if(other.gameObject.transform.position.z >= gameObject.transform.position.z + 8
		   || other.gameObject.transform.position.z <= gameObject.transform.position.z - 8)
		{
			return;
		}
		
		gothit++;
		if(other.gameObject.name == "PlayerAttackCollider"){
			OnHit();
		}
		
	}
	void OnHit(){
		animator.SetBool ("Walking", false);
		if(player.specialAttack)
		{
			life -= 2;
		}
		else
		{
			life -= 1;
		}
		
		if(life <= 0)
		{
			die();
		}
		else
		{
			hitCooldown = .4f;
			animator.SetBool("Hit", true);
		}
	}
	void die(){
		//Placeholder
		//Destroy(this.gameObject);
		spawner.BroadcastMessage("SFSdied", num);
	}
	void punch(){
		attacking = true;
		disableCollider = .1f;
		animator.SetBool ("Attacking", true);
		animator.SetBool ("Walking", false);
		attackCollider.enabled = true;
	}
	void throwKunai(){
		float sx = transform.position.x + 24.5f * Mathf.Sign(transform.localScale.x);
		float sy = yPos + 36f;
		GameObject kunaigen = Instantiate(kunai, new Vector3(sx, sy, yPos), Quaternion.identity) as GameObject;
		KunaiScript ss = kunaigen.GetComponent<KunaiScript>();
		if(transform.position.x < player.transform.position.x){
			ss.dir = 1;
		}else{
			ss.dir = -1;
		}
		ss.screenstop = screenstop;
		
	}
}
