using System.Collections;
using System.Collections.Generic;
using System;//if your code is broken, it might be this
using UnityEngine;
using Windows.Kinect;

//Manages position input from the Kinect regarding a single joint
//Required for all joints you want the Kinect to track

public class DetectJoints : MonoBehaviour {

	public GameObject BodySrcManager;
	private JointType TrackedJoint;
	private BodySourceManager bodyManager;
	private Body[] bodies;
	public float multiplier = 100f; 
	public HandState handBehavior;
	//private Vector3 rot;//rotation

	public bool isLeftHandClosed;
	public bool isRightHandClosed;

	private float posX, posY;
	//arbitrary non-zero start values
	private float oldPosX1 = 0.1f;
	private float oldPosY1 = 0.1f;
	private float oldPosX2 = 0.1f;
	private float oldPosY2 = 0.1f;
	private float oldPosX3 = 0.1f;
	private float oldPosY3 = 0.1f;
	private float oldPosX4 = 0.1f;
	private float oldPosY4 = 0.1f;
	private float easedPosX = 0.1f;
	private float easedPosY = 0.1f;

	// Use this for initialization
	void Start () 
	{
		BodySrcManager = GameObject.Find("BodySourceManager");
		if(BodySrcManager == null){
			Debug.Log("Assign Game Object to Body Source Manager");
		}
		else
		{
			bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(bodyManager == null)
		{
			Debug.Log("Body Manager not assigned");
			return;
		}
		bodies = bodyManager.GetData();

		if(bodies == null)
		{
			return;
		}
		foreach(var body in bodies)
		{
			if (body == null)
			{
				continue;
			}
			if(body.IsTracked)
			{
				var pos = body.Joints[TrackedJoint].Position;
				posX = pos.X;
				posY = pos.Y;

				//Debug.Log(TrackedJoint + " is at " + easedPosX + " " + easedPosY);
				//easedPosX = (posX * easedPosX)/lastFramePosX;
				//easedPosY = (posY * easedPosY)/lastFramePosY;
				//if you wanted to make this fancier, you could weight these
				easedPosX = (posX + oldPosX1 + (oldPosX2/2) + (oldPosX3/4) + (oldPosX4/4))/3;
				easedPosY = (posY + oldPosY1 + (oldPosY2/2) + (oldPosY3/4) + (oldPosY4/4))/3;

				// lastFramePosX = posX;
				// lastFramePosY = posY;

				oldPosX4 = oldPosX3;
				oldPosX3 = oldPosX2;
				oldPosX2 = oldPosX1;
				oldPosX1 = posX;
				oldPosY4 = oldPosY3;
				oldPosY3 = oldPosY2;
				oldPosY2 = oldPosY1;
				oldPosY1 = posY;

				if(!(TrackedJoint == Windows.Kinect.JointType.HandLeft || TrackedJoint == Windows.Kinect.JointType.HandRight)){//if what we're tracking isn't a hand
					gameObject.transform.localPosition = new Vector3(easedPosX * multiplier, easedPosY * multiplier);
				}

				isLeftHandClosed = body.HandLeftState == HandState.Closed;
				isRightHandClosed = body.HandRightState == HandState.Closed;

			}
		}
		
	}

	public JointType GetTrackedJoint(){
		return TrackedJoint;
	}
	public void SetTrackedJoint(JointType jointName){
		TrackedJoint = jointName;
		Debug.Log("Tracked Joint> " + TrackedJoint);
	}
	public float getPosX(){
		return posX;
	}
	public float getPosY(){
		return posY;
	}

	public float getEasedPosX(){
		return easedPosX;
	}

	public float getEasedPosY(){
		return easedPosY;
	}
}
