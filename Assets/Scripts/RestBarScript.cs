using UnityEngine;
using System.Collections;

public class RestBarScript : MonoBehaviour {
	private Animator animator;
	
	void Start()
	{
		animator = this.GetComponent<Animator>();
	}
	
	void Update() 
	{
		GameObject player = GameObject.Find("Player");
		PlayerScript playerScript = player.GetComponent<PlayerScript>();

		animator.SetInteger("rest", playerScript.rest);
	}
}
