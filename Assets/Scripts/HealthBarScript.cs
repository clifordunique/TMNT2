using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {
	private Animator animator;
	private int life = 12;

	void Start()
	{
		animator = this.GetComponent<Animator>();
	}

	void Update() 
	{
		GameObject player = GameObject.Find("Player");
		PlayerScript playerScript = player.GetComponent<PlayerScript>();
		int health = playerScript.life;

		if(health == 60)
		{
			life = 12;
		}
		else if(health >= 55)
		{
			life = 11;
		}
		else if(health >= 50)
		{
			life = 10;
		}
		else if(health >= 45)
		{
			life = 9;
		}
		else if(health >= 40)
		{
			life = 8;
		}
		else if(health >= 35)
		{
			life = 7;
		}
		else if(health >= 30)
		{
			life = 6;
		}
		else if(health >= 25)
		{
			life = 5;
		}
		else if(health >= 20)
		{
			life = 4;
		}
		else if(health >= 15)
		{
			life = 3;
		}
		else if(health >= 10)
		{
			life = 2;
		}
		else if(health >= 1)
		{
			life = 1;
		}
		else
		{
			life = 0;
		}

		animator.SetInteger("life", life);
	}
}
