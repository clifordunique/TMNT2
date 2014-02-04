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
	
	
	public float deltaX = 0;
	private float deltaY = 0;
	private float deltaJump = 0;
	
	public float yPos = 0;
	
	public float jumpPos = 0;
	private float jumpVelocity = 0;
	private float jumpAccel = -10f;
	
	public float jumpKickVelocity = 0;

	private BoxCollider2D attackCollider;
	private BoxCollider2D PattCol;

	private int entering = 5;

	//1 from left, 2 right, 3 door
	public int source;
	
	//private Animator animator;
	public int hitCooldown = 0;
	
	// Use this for initialization
	void Start () {
		//animator = this.GetComponent<Animator>();
		yPos = transform.position.y;
		//Entrance animation if applicable?
		attackCollider = GameObject.Find("PFSAttColl").GetComponent<BoxCollider2D>();
		PattCol = GameObject.Find("PlayerAttackCollider").GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(attacking && hitCooldown == 0)
		{
			if(jumpPos == 0)
			{
				attacking = false;
			}
		}
		
		deltaX = 0;
		deltaY = 0;
		deltaJump = 0;
		
		if(!attacking && hitCooldown == 0)
		{
			PurpleFSInput();
		}
		
		PurpleFSMovement();
		if(hitCooldown > 0){
			hitCooldown--;
		}
		
		//PurpleFSAnimations();
	}
	void PurpleFSMovement()
	{
		if(jumpPos > 0)
		{
			jumpVelocity += jumpAccel * Time.deltaTime;
			deltaJump += jumpVelocity;
		}
		
		if(jumpPos <= 0)
		{
			jumpKickVelocity = 0;
			jumpPos = 0;
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
	void PurpleFSInput()
	{
		GameObject player = GameObject.Find("Player");
		PlayerScript playerScript = player.GetComponent<PlayerScript>();
		float playerx = player.transform.position.x;
		float x = transform.position.x;
		float playery = playerScript.yPos;
		float xdist = Mathf.Abs(playerx-x);
		if(playerx > (x + punchdist))
		{
			deltaX += .7f;
		}
		
		if(playerx < (x - punchdist))
		{
			deltaX += -.7f;
		}
		
		if(playery > yPos+5)
		{
			deltaY += .6f;
		}
		
		if(playery < yPos - 5)
		{
			deltaY += -.6f;
		}
		
		if((xdist < jumpdist) && (xdist > punchdist + .7f) && jumpPos == 0)
		{
			float jumpchance = Random.value;
			if(jumpchance <= .01f)
			{
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
	void OnTriggerEnter2D(Collider2D other){
		gothit = 1;
		if(other == PattCol){
			OnHit();
		}

	}
	void OnHit(){
		if (life > 1){
			hitCooldown = 100;
			life--;
		} else{
			die();
		}
	}
	void die(){
		//Placeholder
		Destroy(this.gameObject);
	}
}
