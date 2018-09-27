 using UnityEngine;
 using System.Collections;
 
 public class CamRotation : MonoBehaviour 
 {
     private float x;
     private float y;
     private Vector3 rotateValue;
     private float currentPosition;

     private float currentRotation;

     public float moveSpeed;
     public float rotationSpeed;

     private Rigidbody rb;

     void Start(){
        rb = gameObject.GetComponent<Rigidbody>();
     }
 

         void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
 }
