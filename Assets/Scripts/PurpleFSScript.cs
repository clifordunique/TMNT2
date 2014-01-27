using UnityEngine;
using System.Collections;

public class PurpleFSScript : MonoBehaviour {
	public int life = 2;
	
	private bool attacking = false;
	private static float bodylength = 22;
	//Probably should fix that to be updated if it changes
	private float jumpdist = bodylength*3f;

	
	public float deltaX = 0;
	private float deltaY = 0;
	private float deltaJump = 0;
	
	public float yPos = 0;
	
	public float jumpPos = 0;
	private float jumpVelocity = 0;
	private float jumpAccel = -10f;

	private float jumpKickVelocity = 0;
	
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		yPos = transform.position.y;
		//Entrance animation if applicable?
	}
	
	// Update is called once per frame
	void Update () {
		if(attacking)
		{
			if(jumpPos == 0)
			{
				attacking = false;
			}
		}
		
		deltaX = 0;
		deltaY = 0;
		deltaJump = 0;
		
		if(!attacking)
		{
			PurpleFSInput();
		}
		
		PurpleFSMovement();
		
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
		
		if(z > 105)
		{
			z = 105;
		}
		else if(z < 15)
		{
			z = 15;
		}
		
		yPos = z;
		
		float y = z + jumpPos + deltaJump;
		
		float x = transform.position.x + deltaX + jumpKickVelocity;
		
		float dist = (transform.position - Camera.main.transform.position).z;
		float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(.0625f,0,dist)).x;
		float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(.9125f,0,dist)).x;
		
		if(x < leftBorder)
		{
			x = leftBorder;
		}
		else if(x > rightBorder)
		{
			x = rightBorder;
		}
		
		transform.position = new Vector3(x, y, z);
	}
	void PurpleFSInput()
	{
		//Didnt use getaxis because it has acceleration
		GameObject player = GameObject.Find("Player");
		PlayerScript playerScript = player.GetComponent<PlayerScript>();
		float playerx = player.transform.position.x;
		float x = transform.position.x;
		float playery = playerScript.yPos;
		float xdist = Mathf.Abs(playerx-x);
		if(playerx > (x + bodylength))
		{
			deltaX += .8f;
		}
		
		if(player.transform.position.x < (x - bodylength))
		{
			deltaX += -.8f;
		}
		
		if(playery > yPos)
		{
			deltaY += .7f;
		}
		
		if(playery < yPos)
		{
			deltaY += -.7f;
		}
		
		if(playery == yPos && (xdist < jumpdist) && (xdist > bodylength) && jumpPos == 0)
		{
			jumpVelocity = 5.7f;
			deltaJump += jumpVelocity;
			//attacking = true;
			jumpKickVelocity = 3f;
			jumpKickVelocity *= Mathf.Sign(playerx-x);
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
}
