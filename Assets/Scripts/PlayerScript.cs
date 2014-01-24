using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public int life = 12;
	public int rest = 2;
	public int pts = 0;
	public string turtleName = "RAPH";



	public float deltaX = 0;
	public float yPos = 0;
	public float jumpPos = 0;
	private float jumpVelocity = 0;
	private float jumpAccel = -10f;

	private Animator animator;
	
	void Start() 
	{
		animator = this.GetComponent<Animator>();
		yPos = transform.position.y;
	}

	void Update() 
	{
		//Didnt use getaxis because it has acceleration
		deltaX = 0;

		if(Input.GetKey(KeyCode.RightArrow))
		{
			deltaX += .8f;
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			deltaX += -.8f;
		}

		float deltaY = 0;

		if(Input.GetKey(KeyCode.UpArrow))
		{
			deltaY += .7f;
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			deltaY += -.7f;
		}


		float deltaJump = 0;

		if(jumpPos > 0)
		{
			jumpVelocity += jumpAccel * Time.deltaTime;
			deltaJump += jumpVelocity;
		}

		if(Input.GetKeyDown(KeyCode.F) && jumpPos == 0)
		{
			jumpVelocity = 5.7f;
			deltaJump += jumpVelocity;
		}

		if(jumpPos < 0)
		{
			jumpPos = 0;
		}

		jumpPos += deltaJump;
		
		float x = transform.position.x + deltaX;

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

		transform.position = new Vector3(x, y, z);










		//Flip sprite left/right
		if (deltaX < 0 && transform.localScale.x > 0)
			transform.localScale = new Vector3(-1f, 1f, 1f);
		else if (deltaX > 0 && transform.localScale.x < 0)
			transform.localScale = new Vector3(1f, 1f, 1f);

		//All animations are handled here
		if(jumpPos == 0)
		{
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















