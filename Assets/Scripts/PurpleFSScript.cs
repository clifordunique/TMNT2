using UnityEngine;
using System.Collections;

public class PurpleFSScript : MonoBehaviour {
	public int life = 2;
	public int gothit = 0;
	
	public bool attacking = false;
	private static float bodylength = 22;
	//Probably should fix that to be updated if it changes
	private float jumpdist = bodylength*3f;
	private float punchdist = 34;
	public bool attacker = false;
	private float rdir = 0;
	private float rtime = 0;
	public Component spawner;
	public int num;
	public bool jumped = false;

	
	
	public float deltaX = 0;
	private float deltaY = 0;
	private float deltaJump = 0;
	
	public float yPos = 0;
	
	public float jumpPos = 0;
	private float jumpVelocity = 0;
	private float jumpAccel = -10f;
	
	public float jumpKickVelocity = 0;

	private BoxCollider2D attackCollider;
	private BoxCollider2D jumpColl;
	private BoxCollider2D PattCol;

	private int entering = 5;

	//1 from left, 2 right, 3 door
	public int source;
	
	private Animator animator;
	public int hitCooldown = 0;
	
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		yPos = transform.position.y;
		//Entrance animation if applicable?
		//attackCollider = GameObject.Find("PFSAttColl").GetComponent<BoxCollider2D>();
		attackCollider = this.transform.FindChild("PFSAttColl").GetComponent<BoxCollider2D>();
		PattCol = GameObject.Find("PlayerAttackCollider").GetComponent<BoxCollider2D>();
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
		deltaJump = 0;
		/*if(jumpPos == 0){
			animator.SetBool ("Jumping", false);
		}*/
		
		if(!attacking && hitCooldown == 0)
		{
			if(xdist <= punchdist && ydist <= 5){
				punch();
			}else if(attacker){
				PurpleFSInput(x, playerx, playery, xdist, ydist);
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
			if (jumpPos == 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("PunchEnd")){
				attacking = false;
				animator.SetBool ("Attacking", false);
				attackCollider.enabled = false;
			}
		}
		
		PurpleFSMovement();
		if(hitCooldown > 0){
			hitCooldown--;
			if(hitCooldown == 0){
				animator.SetBool ("Hit", false);
			}
		}
		
		//PurpleFSAnimations();
	}
	void PurpleFSMovement()
	{
		if(jumpPos > 0)
		{
			jumpVelocity += jumpAccel * Time.deltaTime;
			deltaJump += jumpVelocity;
			animator.SetBool ("Jumping", true);
		}
		
		if(jumpPos <= 0)
		{
			jumpKickVelocity = 0;
			jumpPos = 0;
			animator.SetBool ("Jumping", false);
			attacking = false;
		}
		
		jumpPos += deltaJump;
		
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
		
		float y = z + jumpPos + deltaJump;
		
		float x = transform.position.x + deltaX + jumpKickVelocity;
		//y = Mathf.Round(y*10f)/10f;
		x = Mathf.Round(x*10f)/10f;
		
		transform.position = new Vector3(x, y, z);
	}
	void PurpleFSInput(float x, float playerx, float playery, float xdist, float ydist)
	{
		if(playerx > (x + punchdist))
		{
			deltaX += .7f;
		}
		
		if(playerx < (x - punchdist))
		{
			deltaX += -.7f;
		}
		
		if(jumpPos == 0 && ydist >= (xdist-punchdist) && playery > yPos+5)
		{
			deltaY += .6f;
		}
		
		if(jumpPos == 0 && ydist >= (xdist-punchdist) && playery < yPos - 5)
		{
			deltaY += -.6f;
		}
		
		if((xdist < jumpdist) && (xdist > punchdist + .7f) && jumpPos == 0)
		{
			float jumpchance = Random.value;
			if(jumpchance <= .01f)
			{
				animator.SetBool ("Walking", false);
				jumpVelocity = 5.7f;
				deltaJump += jumpVelocity;
				//attacking = true;
				jumpKickVelocity = 3f;
				//jumpKickVelocity *= Mathf.Sign(playerx-x);
				if(deltaX != 0)
				{
					jumpKickVelocity *= Mathf.Sign(deltaX);
				}
				else
				{
					jumpKickVelocity *= transform.localScale.x;
				}
				animator.SetBool("Jumping", true);
				jumped = true;
				attacking = true;
			}
		}
		if (deltaX < 0 && transform.localScale.x > 0)
			transform.localScale = new Vector3(-1f, 1f, 1f);
		else if (deltaX > 0 && transform.localScale.x < 0)
			transform.localScale = new Vector3(1f, 1f, 1f);
		if ((deltaX != 0 || deltaY != 0) && jumpPos == 0){
			animator.SetBool ("Walking", true);
		}

		
		/*if(Input.GetKeyDown(KeyCode.D))
		{
			attacking = true;
			
			if(jumpVelocity < 3.6f)
			{
				jumpKickVelocity = 3f;
				jumpVelocity = -.8f;
				
				if(deltaX != 0)
				{
					jumpKickVelocity *= Mathf.Sign(deltaX);
				}
				else
				{
					jumpKickVelocity *= transform.localScale.x;
				}
			}
		}*/
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
		gothit++;
		if(other == PattCol){
			OnHit();
		}

	}
	void OnHit(){
		animator.SetBool ("Walking", false);
		if (life > 1){
			hitCooldown = 100;
			life--;
			animator.SetBool ("Hit", true);
		}else{
			die();
		}
	}
	void die(){
		//Placeholder
		//Destroy(this.gameObject);
		spawner.BroadcastMessage("FSdied", num);
	}
	void punch(){
		attacking = true;
		animator.SetBool ("Attacking", true);
		animator.SetBool ("Walking", false);
		attackCollider.enabled = true;
	}
}
