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
	public bool thisTriggered = false;
	
	
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
	private PlayerScript player;
	private LevelDataScript levelData;

	public bool entering = true;

	//1 from left, 2 right, 3 door
	public int source;
	
	private Animator animator;
	public float hitCooldown = 0;
	private float attackCooldown = 0;
	private float disableCollider = 0;
	
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		yPos = transform.position.y;
		levelData = GameObject.Find("LevelData").GetComponent<LevelDataScript>();
		attackCollider = this.transform.FindChild("PFSAttColl").GetComponent<BoxCollider2D>();
		jumpColl = this.transform.FindChild("PFSJumpColl").GetComponent<BoxCollider2D>();
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
		deltaJump = 0;
		/*if(jumpPos == 0){
			animator.SetBool ("Jumping", false);
		}*/
		if(entering == true){
			if(source == 3){
				animator.Play("pfsKick");
			}
			if(animator.GetCurrentAnimatorStateInfo(0).IsName("PurpleStand") || animator.GetCurrentAnimatorStateInfo(0).IsName("PurpleFSWalking")){
				entering = false;
			}
		}else{
			thisTriggered = true;
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
			if(attacking)
			{
				if (animator.GetCurrentAnimatorStateInfo(0).IsName("PurpleFSPunchingEnd")){
					attacking = false;
					animator.SetBool ("Attacking", false);
					attackCollider.enabled = false;
				}
			}
		}

		PurpleFSMovement();

		disableCollider -= Time.deltaTime;
		attackCooldown -= Time.deltaTime;
		hitCooldown -= Time.deltaTime;

		if(hitCooldown <= 0){
			animator.SetBool("Hit", false);
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
			jumpColl.enabled = true;
		}
		
		if(jumpPos <= 0)
		{
			if(jumpColl.enabled)
			{
				attacking = false;
			}
			jumpPos = 0;
			animator.SetBool ("Jumping", false);
			jumpColl.enabled = false;
		}
		
		jumpPos += deltaJump;
		
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
		
		float y = z + jumpPos + deltaJump;
		float x;

		if(jumpPos <= 0)
		{
			x = transform.position.x + deltaX;
		}
		else
		{
			x = transform.position.x + deltaX + jumpKickVelocity;
		}

		//y = Mathf.Round(y*10f)/10f;
		x = Mathf.Round(x*10f)/10f;
		if (deltaX < 0 && transform.localScale.x > 0)
			transform.localScale = new Vector3(-1f, 1f, 1f);
		else if (deltaX > 0 && transform.localScale.x < 0)
			transform.localScale = new Vector3(1f, 1f, 1f);
		if ((deltaX != 0 || deltaY != 0) && jumpPos == 0){
			animator.SetBool ("Walking", true);
		}
		
		transform.position = new Vector3(x, y, z);
	}
	void PurpleFSInput(float x, float playerx, float playery, float xdist, float ydist)
	{
		if(playerx > (x + punchdist))
		{
			deltaX += .8f;
		}
		
		if(playerx < (x - punchdist))
		{
			deltaX += -.8f;
		}
		
		if(jumpPos == 0 && ydist >= (xdist-punchdist) && playery > yPos+5)
		{
			deltaY += .65f;
		}
		
		if(jumpPos == 0 && ydist >= (xdist-punchdist) && playery < yPos - 5)
		{
			deltaY += -.65f;
		}
		
		if((xdist < jumpdist) && (xdist > punchdist + .7f) && jumpPos == 0)
		{
			float jumpchance = Random.value;
			if(jumpchance <= .01f)
			{
				animator.SetBool ("Walking", false);
				jumpColl.enabled = true;
				jumpVelocity = 4f;
				deltaJump += jumpVelocity;
				jumpKickVelocity = 2f;
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
		//Objects are not in the same relative z position
		if(other.gameObject.transform.position.z >= gameObject.transform.position.z + 10
		   || other.gameObject.transform.position.z <= gameObject.transform.position.z - 10)
		{
			return;
		}

		gothit++;
		if(other.gameObject.name == "PlayerAttackCollider"){
			OnHit();
		}

	}
	void OnHit(){
		attacking = false;
		attackCollider.enabled = false;
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
		spawner.BroadcastMessage("PFSdied", num);
	}
	void punch(){
		attacking = true;
		disableCollider = .1f;
		animator.SetBool ("Attacking", true);
		animator.SetBool ("Walking", false);
		attackCollider.enabled = true;
	}
}
