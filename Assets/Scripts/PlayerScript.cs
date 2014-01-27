using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public int life = 12;
	public int rest = 2;
	public int pts = 0;
	public string turtleName = "RAPH";

	public bool attacking = false;
	public bool specialAttack = false;

	public float deltaX = 0;
	private float deltaY = 0;
	private float deltaJump = 0;

	public float yPos = 0;

	public float jumpPos = 0;
	private float jumpVelocity = 0;
	private float jumpAccel = -10f;
	
	private float jumpKickVelocity = 0;

	private Animator animator;
	private LevelDataScript levelData;

	void Start() 
	{
		levelData = GameObject.Find("LevelData").GetComponent<LevelDataScript>();
		animator = this.GetComponent<Animator>();
		yPos = transform.position.y;
	}

	void Update() 
	{
		if(attacking)
		{
			if(jumpPos <= 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelJumpKick"))
			{
				attacking = false;
			}
			else if(animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelAttack2End"))
			{
				attacking = false;
			}
			else if(jumpPos <= 1 && animator.GetCurrentAnimatorStateInfo(0).IsName("RaphaelSpecialAttackEnd"))
			{
				specialAttack = false;
				attacking = false;
			}
		}

		deltaX = 0;
		deltaY = 0;
		deltaJump = 0;

		if(!attacking)
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
			deltaX += .8f;
		}

		if(Input.GetKey(KeyCode.LeftArrow))
		{
			deltaX += -.8f;
		}

		if(Input.GetKey(KeyCode.UpArrow))
		{
			deltaY += .7f;
		}

		if(Input.GetKey(KeyCode.DownArrow))
		{
			deltaY += -.7f;
		}

		if(Input.GetKeyDown(KeyCode.F) && jumpPos == 0)
		{
			jumpVelocity = 5.7f;
			deltaJump += jumpVelocity;
		}

		if(Input.GetKeyDown(KeyCode.D))
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
			else if(jumpVelocity > 5f)
			{
				jumpVelocity = 3;
				specialAttack = true;
			}
			else if(jumpPos != 0)
			{
				attacking = false;
			}
		}
	}

	void playerMovement()
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
		}
		else if(x > rightBorder)
		{
			x = rightBorder;
		}
		
		transform.position = new Vector3(x, y, z);
	}

	void playerAnimations()
	{		
		if(attacking)
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
				if(jumpVelocity < 3.6f)
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
}















