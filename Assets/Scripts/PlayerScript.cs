using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public int life = 60;
	public int rest = 2;
	public int pts = 0;
	public string turtleName = "RAPH";

	private float invicibility = 0;

	public bool attacking = false;
	public bool specialAttack = false;

	private bool hit = false;
	private bool bigHit = false;
	private bool hitLeft = false;

	private bool respawn = false;

	public float deltaX = 0;
	private float deltaY = 0;
	private float deltaJump = 0;

	public float yPos = 0;

	public float jumpPos = 0;
	private float jumpVelocity = 0;
	private float jumpAccel = -12f;
	
	private float jumpKickVelocity = 0;

	private Animator animator;
	private BoxCollider2D attackCollider;
	private LevelDataScript levelData;
	private GameObject cam;

	void Start() 
	{
		cam = GameObject.Find("Main Camera");
		levelData = GameObject.Find("LevelData").GetComponent<LevelDataScript>();
		attackCollider = GameObject.Find("PlayerAttackCollider").GetComponent<BoxCollider2D>();
		animator = this.GetComponent<Animator>();
		yPos = transform.position.y;
	}

	void Update() 
	{
		if(life <= 0)
		{
			invicibility = 10;
		}

		if(life <= 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelDeadEnd"))
		{
			if(rest > 0)
			{
				life = 60;
				rest -= 1;
				respawn = true;
				invicibility = 2;

				attacking = false;
				specialAttack = false;
				hit = false;
				bigHit = false;

				this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				Vector3 position = cam.transform.position;
				position.y = 63f;
				position.z = 63f;
				transform.position = position;
			}
			else
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}

		invicibility -= Time.deltaTime;

		if(invicibility > 0)
		{
			this.gameObject.collider2D.enabled = false;
		}
		else
		{
			this.gameObject.collider2D.enabled = true;
			respawn = false;
		}

		if(respawn)
		{
			if(this.gameObject.renderer.enabled == true)
			{
				this.gameObject.renderer.enabled = false;
			}
			else
			{
				this.gameObject.renderer.enabled = true;
			}
		}
		else
		{
			this.gameObject.renderer.enabled = true;
		}

		if(hit)
		{
			attacking = false;
			specialAttack = false;
			attackCollider.enabled = false;
			invicibility = 10;

			if(bigHit)
			{
				if(animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelBigHitEnd"))
				{
					invicibility = 0;
					hit = false;
					bigHit = false;
				}
			}
			else
			{
				if(animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelHitEnd"))
				{
					invicibility = .1f;
					hit = false;
				}
			}
		}
		else if(attacking)
		{
			if(jumpPos <= 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelJumpKick"))
			{
				attackCollider.enabled = false;
				attacking = false;
			}
			else if(animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelAttack2End"))
			{
				attackCollider.enabled = false;
				attacking = false;
			}
			else if(jumpPos <= 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelSpecialAttackEnd"))
			{
				attackCollider.enabled = false;
				specialAttack = false;
				attacking = false;
			}
		}

		deltaX = 0;
		deltaY = 0;
		deltaJump = 0;

		if(!attacking && !hit && life > 0)
		{
			playerInput();
		}

		playerMovement();

		playerAnimations();
	}

	void playerInput()
	{
		//Didnt use getaxis because it has acceleration

		if(Input.GetKey(KeyCode.RightArrow))
		{
			deltaX += .85f;
		}

		if(Input.GetKey(KeyCode.LeftArrow))
		{
			deltaX += -.85f;
		}

		if(Input.GetKey(KeyCode.UpArrow))
		{
			deltaY += .7f;
		}

		if(Input.GetKey(KeyCode.DownArrow))
		{
			deltaY += -.7f;
		}

		if(Input.GetKeyDown(KeyCode.X) && jumpPos == 0)
		{
			jumpVelocity = 6.2f;
			deltaJump += jumpVelocity;
		}

		if(Input.GetKeyDown(KeyCode.Z))
		{
			attacking = true;
			attackCollider.enabled = true;

			if(jumpVelocity < 3.6f)
			{
				jumpKickVelocity = 3f;
				jumpVelocity = 0;

				if(deltaX != 0)
				{
					jumpKickVelocity *= Mathf.Sign(deltaX);
				}
				else
				{
					jumpKickVelocity *= transform.localScale.x;
				}
			}
			else if(jumpVelocity > 5.5f)
			{
				jumpVelocity = 3;
				specialAttack = true;
			}
			else if(jumpPos != 0)
			{
				attackCollider.enabled = false;
				attacking = false;
			}
		}
	}

	void playerMovement()
	{
		bool changed = false;

		if(animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelBigHitStart"))
		{
			changed = true;
			jumpVelocity = 5f;
			deltaJump = jumpVelocity;
		}

		if(bigHit)
		{
			if(!hitLeft)
			{
				jumpKickVelocity = -jumpVelocity;

				if(jumpKickVelocity > -.75f)
				{
					jumpKickVelocity = -.75f;
				}
			}
			else
			{
				jumpKickVelocity = jumpVelocity;
				
				if(jumpKickVelocity < .75f)
				{
					jumpKickVelocity = .75f;
				}
			}
		}

		if(jumpPos > 0 && !changed)
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
		
		if(z > levelData.walkSpaceHeight)
		{
			z = levelData.walkSpaceHeight;
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
			if(bigHit && jumpVelocity > 0)
			{
				jumpVelocity = 0;
			}
		}
		else if(x > rightBorder)
		{
			x = rightBorder;
			if(bigHit && jumpVelocity > 0)
			{
				jumpVelocity = 0;
			}
		}
		
		transform.position = new Vector3(x, y, z);
	}

	void playerAnimations()
	{		
		if(hit)
		{
			if(bigHit)
			{
				//Flip sprite left
				if(!hitLeft)
					transform.localScale = new Vector3(1f, 1f, 1f);
				else
					transform.localScale = new Vector3(-1f, 1f, 1f);

				if(jumpPos == 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelBigHitAir"))
				{
					animator.Play("RaphaelBigHitGround");
				}
				else if(!animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelBigHitGround")
				        && !animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelBigHitEnd")
				        && !animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelBigHitAir"))
				{
					animator.Play("RaphaelBigHitStart");
				}
			}
			else
			{
				if(!animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelHit") &&
				   !animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelHitEnd"))
				{
					animator.Play("RaphaelHit");
				}
			}
		}
		else if(attacking)
		{
			if(jumpPos == 0)
			{
				animator.Play("RaphaelAttack2");
			}
			else
			{
				//Flip sprite left/right
				if (deltaX < 0 && transform.localScale.x > 0)
					transform.localScale = new Vector3(-1f, 1f, 1f);
				else if (deltaX > 0 && transform.localScale.x < 0)
					transform.localScale = new Vector3(1f, 1f, 1f);

				if(specialAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelSpecialAttackEnd"))
				{
					animator.Play("RaphaelSpecialAttack");
				}
				else if(animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelSpecialAttackEnd"))
				{
					animator.Play("RaphaelSpecialAttackEnd");
				}
				else if(jumpVelocity < 3.6f)
				{
					animator.Play("RaphaelJumpKick");
				}
			}
		}
		else if(life <= 0)
		{
			if(!animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelDead")
			   && !animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelDeadEnd"))
			{
				animator.Play("RaphaelDead");
			}
		}
		else
		{
			if(jumpPos == 0)
			{
				//Flip sprite left/right
				if (deltaX < 0 && transform.localScale.x > 0)
					transform.localScale = new Vector3(-1f, 1f, 1f);
				else if (deltaX > 0 && transform.localScale.x < 0)
					transform.localScale = new Vector3(1f, 1f, 1f);

				if(deltaX != 0 || deltaY != 0)
				{
					animator.Play("RaphaelWalk");
				}
				else
				{
					animator.Play("RaphaelIdle");
				}
			}
			else
			{
				if(jumpVelocity < 4f)
				{
					animator.Play("RaphaelJumpSpin");
				}
				else
				{
					animator.Play("RaphaelJump");
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collide)
	{
		//Objects are not in the same relative z position
		if(collide.gameObject.transform.position.z >= gameObject.transform.position.z + 10
		   || collide.gameObject.transform.position.z <= gameObject.transform.position.z - 10
		   || hit || bigHit)
		{
			return;
		}

		if(collide.gameObject.name == "RollingBall(Clone)")
		{
			life -= 8;
			hit = true;
			bigHit = true;
			hitLeft = false;
		}
		else if(collide.gameObject.name == "PFSAttColl"
		        || collide.gameObject.name == "PFSJumpColl")
		{
			life -= 1;
			hit = true;

			if(jumpPos != 0)
			{
				bigHit = true;
				if(collide.gameObject.transform.position.x < gameObject.transform.position.x)
				{
					hitLeft = true;
				}
				else
				{
					hitLeft = false;
				}
			}
		}

		//All Enemy attack damages go here
		//bigHit means the character flies backwards










	}
}















