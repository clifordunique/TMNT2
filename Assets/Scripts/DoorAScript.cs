using UnityEngine;
using System.Collections;

public class DoorAScript : MonoBehaviour {
	private Animator animator;
	private GameObject cam;

	private bool check = false;

	void Start() 
	{
		cam = GameObject.Find("Main Camera");
		animator = this.GetComponent<Animator>();
	}

	void Update() 
	{
		if(cam.transform.position.x + 25 > transform.position.x && !check)
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
