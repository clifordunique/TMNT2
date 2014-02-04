using UnityEngine;
using System.Collections;

public class DoorAScript : MonoBehaviour {
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
		if(camera.transform.position.x + 25 > transform.position.x && !check)
		{
			animator.Play("DoorAAnimation");
			check = true;
		}

		if(animator.GetCurrentAnimatorStateInfo(0).IsName("DoorAEnd"))
		{
			Destroy(this.gameObject);
		}
	}
}
