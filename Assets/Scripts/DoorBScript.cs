using UnityEngine;
using System.Collections;

public class DoorBScript : MonoBehaviour {
	private Animator animator;
	private GameObject camera;
	
	private bool check = false;
	
	void Start() 
	{
		camera = GameObject.Find("Main Camera");
		animator = this.GetComponent<Animator>();
	}

	void Update() 
	{
		if(camera.transform.position.x + 25 > 1530 && !check)
		{
			animator.Play("DoorBAnimation");
			check = true;
		}

		if(animator.GetCurrentAnimatorStateInfo(0).IsName("DoorBEnd"))
		{
			Destroy(this.gameObject);
		}
	}
}
