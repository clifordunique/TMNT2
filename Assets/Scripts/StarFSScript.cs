using UnityEngine;
using System.Collections;

public class StarFSScript : MonoBehaviour {
	public int life = 2;
	public int gothit = 0;
	public GameObject star;
	
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
	
	private int entering = 5;
	
	//1 from left, 2 right, 3 door
	public int source;
	
	private Animator animator;
	public int hitCooldown = 0;
	
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		yPos = transform.position.y;
		attackCollider = this.transform.FindChild("SFSAttColl").GetComponent<BoxCollider2D>();
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
		
		if(!attacking && hitCooldown == 0)
		{
			if(xdist <= punchdist && ydist <= 5){
				punch();
			}else if(attacker){
				StarFSInput(x, playerx, playery, xdist, ydist);
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
		if(attacking && hitCooldown == 0)
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("PunchEnd")){
				attacking = false;
				animator.SetBool ("Attacking", false);
				attackCollider.enabled = false;
			}
		}
		
		StarFSMovement();
		if(hitCooldown > 0){
			hitCooldown--;
			if(hitCooldown == 0){
				animator.SetBool ("Hit", false);
			}
		}
	}
	void StarFSMovement()
	{
		
		//Using Z axis for Isometric effect
		float z = yPos + deltaY;
		
		if(z > 105f)
		{
			z = 105f;
		}
		else if(z < 15f)
		{
			z = 15f;
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
	void StarFSInput(float x, float playerx, float playery, float xdist, float ydist)
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
		
		if((ydist < 8) && (xdist > punchdist + .7f))
		{
			throwStar();
//			float jumpchance = Random.value;
//			if(jumpchance <= .01f)
//			{
//				animator.SetBool ("Walking", false);
//				jumpVelocity = 5.7f;
//				deltaJump += jumpVelocity;
//				//attacking = true;
//				jumpKickVelocity = 3f;
//				//jumpKickVelocity *= Mathf.Sign(playerx-x);
//				if(deltaX != 0)
//				{
//					jumpKickVelocity *= Mathf.Sign(deltaX);
//				}
//				else
//				{
//					jumpKickVelocity *= transform.localScale.x;
//				}
//				animator.SetBool("Jumping", true);
//				jumped = true;
//				attacking = true;
//			}

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
			hitCooldown = 100;
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
		animator.SetBool ("Attacking", true);
		animator.SetBool ("Walking", false);
		attackCollider.enabled = true;
	}
	void throwStar(){
		float sx = transform.position.x + 24.5;
		float sy = yPos + 40;
		GameObject stargen = Instantiate(star, new Vector3(sx, sy, sy), Quaternion.identity);
		StarScript ss = stargen.GetComponent<StarScript>();
		if(transform.position.x > player.transform.position.x){
			ss.dir = 1;
		}else{
			ss.dir = -1;
		}
		ss.screenstop = screenstop;

	}
}
