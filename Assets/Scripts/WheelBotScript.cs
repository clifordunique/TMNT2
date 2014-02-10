using UnityEngine;
using System.Collections;

public class WheelBotScript : MonoBehaviour {
	public int life = 16;

	public float deltaX = 0;
	private float deltaY = 0;

	private bool left = true;
	private bool up = true;

	public float yPos = 0;

	private PlayerScript player;
	private LevelDataScript levelData;
	private MainCameraScript cam;
	private Animator animator;

	public GameObject laser;

	public float hitCooldown = 0;
	public float attackCooldown = 0;
	public float attackAnimation = 0;

	void Start () 
	{
		yPos = transform.position.y;
		animator = this.GetComponent<Animator>();
		cam = GameObject.Find("Main Camera").GetComponent<MainCameraScript>();
		levelData = GameObject.Find("LevelData").GetComponent<LevelDataScript>();
		player = GameObject.Find("Player").GetComponent<PlayerScript>();
	}
	
	void Update () 
	{
		attackCooldown -= Time.deltaTime;
		attackAnimation -= Time.deltaTime;
		hitCooldown -= Time.deltaTime;

		if(life <= 0 && hitCooldown <= 0)
		{
			cam.points += 1;
			Destroy(this.gameObject);
		}

		deltaX = 0;
		deltaY = 0;

		if(attackAnimation <= 0 && hitCooldown <= 0)
		{
			Input();
		}

		Movement();

		Animations();
	}

	void Input()
	{
		if(attackCooldown <= 0)
		{
			if(player.transform.position.x < this.transform.position.x && this.transform.localScale.x < 0)
			{
				Vector3 position = this.transform.position;
				position.x += 12.5f * this.transform.localScale.x;
				position.y += 17.3f;
				GameObject laz = Instantiate(laser, position, Quaternion.identity) as GameObject;
				laz.GetComponent<LaserScript>().deltaX = -2.4f;
				attackAnimation = .8f;
				attackCooldown = 6f;
			}
			else if(player.transform.position.x > this.transform.position.x && this.transform.localScale.x > 0)
			{
				Vector3 position = this.transform.position;
				position.x += 12.5f * this.transform.localScale.x;
				position.y += 17.3f;
				GameObject laz = Instantiate(laser, position, Quaternion.identity) as GameObject;
				laz.GetComponent<LaserScript>().deltaX = 2.4f;
				attackAnimation = .8f;
				attackCooldown = 6f;
			}
		}
		
		float dist = (transform.position - Camera.main.transform.position).z;
		float leftborder = Camera.main.ViewportToWorldPoint(new Vector3(.2f, 0, dist)).x;
		float rightborder = Camera.main.ViewportToWorldPoint(new Vector3(.8f, 0, dist)).x;

		if(left)
		{
			if(this.transform.position.x < leftborder - Random.Range(5, 30))
			{
				left = false;
			}
			else if(this.transform.position.x < leftborder)
			{
				deltaX = -.3f;

				if(up)
				{
					deltaY = 1.2f;
				}
				else
				{
					deltaY = -1.2f;
				}
			}
			else
			{
				deltaX = -1.5f;
				if(this.transform.position.y > levelData.walkSpaceHeight - 20)
				{
					up = false;
				}
				else if(this.transform.position.y < levelData.walkSpaceBottom + 20)
				{
					up = true;
				}
			}
		}
		else
		{	
			if(this.transform.position.x > rightborder + Random.Range(5, 30))
			{
				left = true;
			}
			else if(this.transform.position.x > rightborder)
			{
				deltaX = .3f;

				if(up)
				{
					deltaY = 1.2f;
				}
				else
				{
					deltaY = -1.2f;
				}
			}
			else
			{
				deltaX = 1.5f;
				if(this.transform.position.y > levelData.walkSpaceHeight - 20)
				{
					up = false;
				}
				else if(this.transform.position.y < levelData.walkSpaceBottom + 20)
				{
					up = true;
				}
			}
		}
	}

	void Movement()
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

		yPos = z;
		float y = z;
		float x = transform.position.x + deltaX;
		
		transform.position = new Vector3(x, y, z);
	}

	void Animations()
	{
		if(deltaX < 0 && this.transform.localScale.x > 0)
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if(deltaX > 0 && this.transform.localScale.x < 0)
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
		}

		if(hitCooldown > 0)
		{
			animator.Play("WheelBotHit");
		}
		else if(attackAnimation > 0)
		{
			animator.Play("WheelBotShoot");
		}
		else
		{
			animator.Play("WheelBotIdle");
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Objects are not in the same relative z position
		if(other.gameObject.transform.position.z >= gameObject.transform.position.z + 10
		   || other.gameObject.transform.position.z <= gameObject.transform.position.z - 10
		   || hitCooldown > 0)
		{
			return;
		}

		if(other.gameObject.name == "PlayerAttackCollider")
		{
			if(player.specialAttack)
			{
				life -= 2;
			}
			else
			{
				life -= 1;
			}

			hitCooldown = .5f;
			attackAnimation = 0;
		}
	}
}
