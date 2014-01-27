using UnityEngine;
using System.Collections;

public class DoorAScript : MonoBehaviour {
	Animator animator;

	// Use this for initialization
	void Start() 
	{
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("DoorAEnd"))
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.name == "Player")
		{
			animator.Play("DoorAAnimation");
		}
	}
}
