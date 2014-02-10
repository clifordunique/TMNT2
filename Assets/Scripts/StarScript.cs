using UnityEngine;
using System.Collections;

public class StarScript : MonoBehaviour {
	public float dir;
	public float screenstop;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		float x = transform.position.x;
		if(x < screenstop + 22f && x > screenstop - 278f && !animator.GetCurrentAnimatorStateInfo(0).IsName("Done")){
			x += dir*.9f;
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
		} else {
			Destroy (this.gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		//Objects are not in the same relative z position
		if(other.gameObject.transform.position.z >= gameObject.transform.position.z + 8
		   || other.gameObject.transform.position.z <= gameObject.transform.position.z - 8)
		{
			return;
		}
		if(other.gameObject.name == "PlayerAttackCollider" || other.gameObject.name == "Player"){
			animator.SetBool("Hit", true);
		}
		
	}
}
