using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;


//Keeps track of when the left hand has entered the collider of a book
//Manages book holding

public class grabRight : MonoBehaviour {
	private Collider held;
	private DetectJoints handR, elbowR;
	public JointType handRight, elbowRight;
	public static bool holding = false;//you only have one right hand

	public float multiplier = 100f; //for hand position

	//calculate reach into the z axis
	public GameObject elbow, realHand;
	public float ARM_LENGTH;
	private float measuredArm;
	private float reachDepth;

	public float openBookW, openBookH; //how close to the center of books do you need to be?


	// Use this for initialization
	void Start () {
		held = null;
		//create arm
		handR = gameObject.AddComponent<DetectJoints>() as DetectJoints;
		handR.SetTrackedJoint(handRight);
		elbowR = elbow.AddComponent<DetectJoints>() as DetectJoints;
		elbowR.SetTrackedJoint(elbowRight);
	}
	
	// Update is called once per frame
	void Update () {
		//Checks if you're holding extra objects, and lets them go if you are
		if(holding == false && transform.childCount>0){
			transform.GetChild(0).transform.parent = GameObject.Find("world").transform;
		}

		//are you indicating that you want to do something with one of the open books?
		//currently hardcoded into 4 quadrants
		if(handR.isRightHandClosed){
			if(handR.getPosX() <0){
				if(handR.getPosY()<0){
					selectBook.selectedBook = 0;
					//Debug.Log("Selected Book: " + selectBook.selectedBook);
				}
				else{
					selectBook.selectedBook = 2;
					//Debug.Log("Selected Book: " + selectBook.selectedBook);
				}
			}
			else{
				if(handR.getPosY()<0){
					selectBook.selectedBook = 1;
					//Debug.Log("Selected Book: " + selectBook.selectedBook);
				}
				else{
					selectBook.selectedBook = 3;
					//Debug.Log("Selected Book: " + selectBook.selectedBook);
				}
			}
		}


		//calculate reach into the z axis
		float xChunk = (handR.getEasedPosX()-elbowR.getEasedPosX());
		float yChunk = (handR.getEasedPosY()-elbowR.getEasedPosY());
		measuredArm = xChunk * xChunk + yChunk * yChunk;
		//you should technically take the square root of this, but mirroring reality exactly is lame
		reachDepth = ARM_LENGTH * ARM_LENGTH - measuredArm * measuredArm;
		//and update the position
		//only kind of a hardcoded nightmare
		gameObject.transform.localPosition = new Vector3(handR.getEasedPosX() * multiplier, handR.getEasedPosY() * multiplier, (reachDepth * multiplier*100) -40);

		//grab a book
		if(handR.isRightHandClosed && holding == false|| Input.GetKeyDown("p") && holding == false){
            if(selectBook.getHeldObject()!=null)
			if(selectBook.getHeldObject().tag == "closedBook"){
				selectBook.getHeldObject().transform.SetParent(this.gameObject.transform);
				holding = true;//we need to check that it worked before we do this
			}
		}
		//let go
		if(Input.GetKeyDown("z") && holding == true){

            holding = false;
			//if its a hand, keep it on your body
			if(selectBook.getHeldObject().tag == "hand"){
				selectBook.getHeldObject().transform.parent = GameObject.Find("playerBody").transform;
			}
			else{                                                                                  
				selectBook.getHeldObject().transform.parent = GameObject.Find("world").transform;
			}
		}
	}
	
	//Checks if you're touching a book
	void OnTriggerEnter(Collider other){
		if(other.transform.root.gameObject.tag == "hand" && selectBook.tooManyBooks == false){
			Debug.Log("Hands are touching");
			selectBook.handsAreTouching = true;
		}
		if(holding == false){//if we aren't already holding something
			//Debug.Log("You touched " + other.gameObject);
			if (other.gameObject.tag == "closedBook"){
				held = other;
				selectBook.setHeldObject(other.gameObject);
				//Debug.Log("(R)You're holding a thing!\n" + other.gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.transform.root.gameObject.tag == "hand"){
			//Debug.Log("Hands are touching");
			selectBook.handsAreTouching = false;
		}
		if(holding == false){
			//Debug.Log("(R)You are no longer touching " + other.gameObject);
			selectBook.setHeldObject(GameObject.Find("handR"));
		}
	}
	public static bool isHolding(){
		return holding;
	}
}
