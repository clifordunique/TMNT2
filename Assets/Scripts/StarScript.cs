using UnityEngine;
using System.Collections;

public class StarScript : MonoBehaviour {
	
	public float deltaX = 2f;
	private float distance = 0;
	
	private Animator animator;
	
	void Start () 
	{
		animator = this.GetComponent<Animator>();
	}
	
	void Update () 
	{
		if(deltaX < 0)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}

		Vector3 position = this.transform.position;
		position.x += deltaX;
		this.transform.position = position;
		
		distance += deltaX;
		if(distance > 512 || distance < -512 || animator.GetCurrentAnimatorStateInfo(0).IsName("Done"))
		{
			Destroy(this.gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		//Objects are not in the same relative z position
		if(other.gameObject.transform.position.z >= gameObject.transform.position.z + 8
		   || other.gameObject.transform.position.z <= gameObject.transform.position.z - 8)
		{
			return;
		}
		
		if(other.gameObject.name == "PlayerAttackCollider" || other.gameObject.name == "Player")
		{
			this.collider2D.enabled = false;
			animator.SetBool("Hit", true);
		}
		
	}
}
