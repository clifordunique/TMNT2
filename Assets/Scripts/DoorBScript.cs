using UnityEngine;
using System.Collections;

public class DoorBScript : MonoBehaviour {
	Animator animator;
	
	// Use this for initialization
	void Start() 
	{
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("DoorBEnd"))
		{
			Destroy(this.gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.name == "Player")
		{
			animator.Play("DoorBAnimation");
		}
	}
}
