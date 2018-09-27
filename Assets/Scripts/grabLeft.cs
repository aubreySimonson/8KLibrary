using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;


//Keeps track of when the left hand has entered the collider of a book
//Manages book holding

public class grabLeft : MonoBehaviour {
	private Collider held;
	private DetectJoints handL, elbowL;
	public JointType handLeft, elbowLeft;
	public static bool holding = false;//you only have one left hand

	public float multiplier = 100f; //for hand position

	//calculate reach into the z axis
	public GameObject elbow, realHand;
	public float ARM_LENGTH;
	private float measuredArm;
	private float reachDepth;

	public float openBookW, openBookH; //how close to the center of books do you need to be?

	//for keeping tack of highlighting/unhighlighting books
	private Material coverColor;

	// Use this for initialization
	void Start () {
		held = null;
		//create arm
		handL = gameObject.AddComponent<DetectJoints>() as DetectJoints;
		handL.SetTrackedJoint(handLeft);
		elbowL = elbow.AddComponent<DetectJoints>() as DetectJoints;
		elbowL.SetTrackedJoint(elbowLeft);
	}
	
	// Update is called once per frame
	void Update () {
		//Checks if you're holding extra objects, and lets them go if you are
		if(holding == false && transform.childCount>0){
			transform.GetChild(0).transform.parent = GameObject.Find("world").transform;
		}
		//are you indicating that you want to do something with one of the open books?
		//currently hardcoded into 4 quadrants
		//selectedBook is a pretty bad name for this
		if(handL.isLeftHandClosed){
			if(handL.getPosX() <0){
				if(handL.getPosY()<0){
					selectBook.selectedBook = 0;
				}
				else{
					selectBook.selectedBook = 2;
				}
			}
			else{
				if(handL.getPosY()<0){
					selectBook.selectedBook = 1;
				}
				else{
					selectBook.selectedBook = 3;
				}
			}
		}

		//calculate reach into the z axis
		float xChunk = (handL.getEasedPosX()-elbowL.getEasedPosX());
		float yChunk = (handL.getEasedPosY()-elbowL.getEasedPosY());
		measuredArm = xChunk * xChunk + yChunk * yChunk;
		//you should technically take the square root of this, but mirroring reality exactly is lame
		reachDepth = ARM_LENGTH * ARM_LENGTH - measuredArm * measuredArm;
		//and update the position
		//only kind of a hardcoded nightmare
		gameObject.transform.localPosition = new Vector3(handL.getEasedPosX() * multiplier, handL.getEasedPosY() * multiplier, (reachDepth * multiplier*100) -40);
		
		//grab/hold book
		if(handL.isLeftHandClosed && holding == false|| Input.GetKeyDown("o")&& holding == false){
			if(selectBook.getHeldObject().tag == "closedBook"){
				selectBook.getHeldObject().transform.SetParent(this.gameObject.transform);
				holding = true;
			}
		}
		//let go
		if(Input.GetKeyDown("z") && holding ==true){
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
		if(holding == false){
			if (other.gameObject.tag == "closedBook"){
				held = other;
				selectBook.setHeldObject(other.gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.transform.root.gameObject.tag == "hand"){
			//Debug.Log("Hands are touching");
			selectBook.handsAreTouching = false;
		}

		if(holding == false){;
			selectBook.setHeldObject(GameObject.Find("handL"));
		}
	}

	public static bool isHolding(){
		return holding;
	}
}
