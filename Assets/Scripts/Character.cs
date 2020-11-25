using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	// Use this for initialization
	string charstate;
	/*
	 *idle
	 *run
	 *jump
	 *slide
	 *attack 
	 *rev
	 *
	 *
	 */
	//float speed=2f;
	float upPush=0f;
	Rigidbody rb;
	bool hitslidewall = false;

	Animator ani;

	CharacterController controller;
	gameFlow gf;
	Vector3 movedir=Vector3.zero;
	//public float baseSpeed=3.0f; unused
	public float speed=1458f;
	public float jumpSpeed=8.0f;
	public float grav=40.0f;

	float elapsedTime=0f;
	int speedmodifier=1;

	Vector3 backwards;
	Vector3 forwards;
	bool lost = false;
	void OnBecameInvisible()
	{
		if (lost == false) {
			gf = GameObject.Find ("scriptCarrier").GetComponent<gameFlow> ();

			if (this.transform.position.x < gf.camera.transform.position.x) {
				gf.defeat ();
			} else if (this.transform.position.x > gf.camera.transform.position.x) {
				this.transform.position = new Vector3 (this.transform.position.x - 0.1f, this.transform.position.y, this.transform.position.z);
			}
			lost = true;
		} 

	}
	void OnControllerColliderHit(ControllerColliderHit col)
	{
		if (col.gameObject.name == "slideWall") {
			speedmodifier = 0;
			//print ("a");
		}
		//if(col.gameObject.name !="ground(Clone)")
	//	print (col.gameObject.name);
		if (col.gameObject.name == "obsAtk(Clone)" && charstate=="attack") {
			Destroy (col.gameObject);
		}
	}


	void Start () {
		rb = GetComponent<Rigidbody> ();
		controller = GetComponent<CharacterController> ();
		ani = transform.GetChild(0).GetComponent<Animator> ();
		backwards = new Vector3 (0, 180, 0);
		forwards = new Vector3 (0, 0, 0);
	}

	void animChange(string state)
	{
		if (state == "rev") 
			ani.Play ("Reverse");
			//transform.Rotate(backwards);
		else if (state == "jump")
			ani.Play ("Jump");
		else if (state == "slide")
			ani.Play ("Slide");
		else if (state == "attack")
			ani.Play ("Hurricane Kick");
		else 
			ani.Play ("Running");
		//print (state);
			
		
	}
	
	// Update is called once per frame
	void Update () {
		//float step = speed * Time.deltaTime;
		//transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x+1, transform.position.y, transform.position.z), step);
		//rb.velocity =new Vector3 (speed, upPush, 0);
		/*
		if (Input.GetKeyDown (KeyCode.A))
			speed -= baseSpeed;
		if (Input.GetKeyUp (KeyCode.A))
			speed += baseSpeed;
		*/
		speedmodifier = 1;

		if (hitslidewall)
			speed = 0;
	 
	/*	if (Input.GetKeyDown (KeyCode.A)) {
			speed = -6;
			charstate = "rev";
		}
		if (Input.GetKeyUp (KeyCode.A) && charstate=="rev") {
			speed = 6;
			charstate = "run";
			controller.height = 2;
		}

*/
		//transform.Rotate(new Vector3(0,0,0));

		if (Input.GetKey (KeyCode.A)) {
			speedmodifier = -1;
			charstate = "rev";
		//	transform.Rotate(new Vector3(0,180,0));
		}
		if(Input.GetKeyUp(KeyCode.A))
		{
			charstate = "run";
			speedmodifier = 1;
		}
		//if (speedmodifier == -1)
			

		if (Input.GetKeyDown (KeyCode.D)) {
			charstate = "attack";
		}
		if (Input.GetKeyUp (KeyCode.D) && charstate=="attack") {
			charstate = "run";
			controller.height = 2;
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			charstate = "slide";
			controller.height = 0.5f;
		}
		if (Input.GetKeyUp (KeyCode.S) ) {
			charstate = "run";		
			controller.height = 2;
		}
		
		if (controller.isGrounded) {
			//movedir = new Vector3 (speed,0,0);
			//movedir = transform.TransformDirection (movedir);
			//movedir *= speed;
			movedir.y=0;
			if (Input.GetKeyDown (KeyCode.W)) {
				movedir.y = jumpSpeed;
				charstate = "jump";
			}
			if (charstate == "jump")
				charstate = "run";
			//print ("a");
		}
		movedir.x =  speed*speedmodifier;
		movedir.y -= Time.deltaTime * grav *1.45f;
		controller.Move (movedir *1.8f* Time.deltaTime);

		if (transform.position.z != 1)
			transform.position = new Vector3 (transform.position.x, transform.position.y, 1);

		hitslidewall = false;
		//print (elapsedTime);
		elapsedTime += Time.deltaTime;

		animChange (charstate);
	}
}
