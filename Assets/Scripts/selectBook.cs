using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Determines if a book should be opened, and handles opening it, closing it, and putting it back
//asimonso

public class selectBook : MonoBehaviour {
	private static GameObject heldObject;
	private GameObject book;
	public static GameObject [] openBooks = new GameObject [5];
	public GameObject openBook, playerBody;
	public grabLeft leftHand;
	public grabRight rightHand;
	private static Collider held;
	public static int selectedBook; //what book is your hand hovering over right now?  (there are a lot of problems with this whole system)
	private static string title = "no book selected";
	private static string [] titles = new string [5];
	public static bool handsAreTouching = false; //for using bringing hands together to open books
	public static bool tooManyBooks = false; //do you have space on the screen for opening more books?
	public static Vector3 [] storedBookLocations = new Vector3 [5]; //where to put open book back
	public static GameObject [] putBookBackonthisShelves = new GameObject[5]; 
	public static int numOpenBooks = 0;                                                                                          //!

	// Use this for initialization
	void Start () {
		selectedBook = -1;
		heldObject = GameObject.Find("Sphere");
        if (heldObject != null)
            held = heldObject.GetComponent<Collider>();
        else
            held = null;
	}
	
	// Update is called once per frame
	void Update () {
		//close book
		if(Input.GetKeyDown("x")){
			Debug.Log("current array: " + openBooks[0] + ", " + openBooks[1] + ", " + openBooks[2] + ", " + openBooks[3] + ", " + openBooks[4]);
			Debug.Log("closing something?");
			if(selectedBook > -1){
			    numOpenBooks --;
				Destroy(openBooks[selectedBook]);
				openBooks[selectedBook] = null; //(unclear if you need to do this excplicitly)
				tooManyBooks = false;//this will always be true if you've just closed a book
				//reshelve
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			    shelfBook currentBook = cube.AddComponent<shelfBook>() as shelfBook;
			    currentBook.setName(titles[selectedBook]);
			    currentBook.transform.parent = putBookBackonthisShelves[selectedBook].transform;                                                                     //!
			    currentBook.transform.position = storedBookLocations[selectedBook];
			    string[] nameParts = titles[selectedBook].Split('-');
			    currentBook.setAuthor(nameParts[0]);
			    currentBook.setTitle(nameParts[1]);
			    currentBook.transform.localEulerAngles = new Vector3(0, 285, 0);
			    selectedBook = -1;//you are no longer selecting a book, because you just closed it
			    Debug.Log("updated array: " + openBooks[0] + ", " + openBooks[1] + ", " + openBooks[2] + ", " + openBooks[3] + ", " + openBooks[4]);
			 }
		}

		//open book
		if (Input.GetKeyDown("space") && tooManyBooks ==false || handsAreTouching == true && tooManyBooks == false){
			Debug.Log("Opening a book now");
			if(grabLeft.isHolding() || grabRight.isHolding()){//you'll need to sort this out more if which hand is grabbing matters

				//you're definitely not holding anything anymore
				grabLeft.holding = false;
				grabRight.holding = false;

				if(heldObject.tag == "closedBook"){
					title = heldObject.name;
					Debug.Log("the title of this book is " + title);
					if(numOpenBooks<4){
						book = Instantiate(openBook);
					}
					int arrayPosition;
					if(numOpenBooks == 0){
						openBooks[numOpenBooks] = book.gameObject;
						arrayPosition = 0;
					}
					else{
						arrayPosition = System.Array.IndexOf(openBooks, null);//this line might be the problem
						if(arrayPosition < 4 && arrayPosition >= 0){//seriously making sure this isn't broken
							openBooks[arrayPosition] = book.gameObject;   
						}             
					}     
					Debug.Log("placed in the array at " + arrayPosition);    
					Debug.Log("number of open books: " + numOpenBooks);
					//Debug.Log("current array: " + openBooks[0] + ", " + openBooks[1] + ", " + openBooks[2] + ", " + openBooks[3] + ", " + openBooks[4]);

					//position opened book relative to the camera
					if(arrayPosition == 0){
						Debug.Log("You're opening book 0");
						book.transform.SetParent(Camera.main.transform);
						book.transform.localPosition = new Vector3(-115, -50, 150);
						book.transform.localEulerAngles = new Vector3(0, 0, 0);
					}
					else if(arrayPosition == 1){
						Debug.Log("You're opening book 1");
						book.transform.SetParent(Camera.main.transform);
						book.transform.localPosition = new Vector3(115, -50, 150);//these numbers need hard-coded jiggling
						book.transform.localEulerAngles = new Vector3(0, 0, 0);
					}
					else if(arrayPosition == 2){
						Debug.Log("You're opening book 2");
						book.transform.SetParent(Camera.main.transform);
						book.transform.localPosition = new Vector3(-115, 60, 150);//these numbers need hard-coded jiggling
						book.transform.localEulerAngles = new Vector3(0, 0, 0);
					}
					else if(arrayPosition == 3){
						book.transform.SetParent(Camera.main.transform);
						book.transform.localPosition = new Vector3(115, 60, 150);//these numbers need hard-coded jiggling
						book.transform.localEulerAngles = new Vector3(0, 0, 0);
					}
					// else if(arrayPosition == 4){
					// 	book.transform.SetParent(Camera.main.transform);
					// 	book.transform.localPosition = new Vector3(115, 0, 150);//these numbers need hard-coded jiggling
					// 	book.transform.localEulerAngles = new Vector3(0, 0, 0);
					// }
					// else if(arrayPosition == 5){
					// 	book.transform.SetParent(Camera.main.transform);
					// 	book.transform.localPosition = new Vector3(-115, 0, 150);//these numbers need hard-coded jiggling
					// 	book.transform.localEulerAngles = new Vector3(0, 0, 0);
					// }
					// else if(arrayPosition == 6){
					// 	book.transform.SetParent(Camera.main.transform);
					// 	book.transform.localPosition = new Vector3(50, -50, 150);//these numbers need hard-coded jiggling
					// 	book.transform.localEulerAngles = new Vector3(0, 0, 0);
					// }
					// else if(arrayPosition == 7){
					// 	book.transform.SetParent(Camera.main.transform);
					// 	book.transform.localPosition = new Vector3(-50, -50, 150);//these numbers need hard-coded jiggling
					// 	book.transform.localEulerAngles = new Vector3(0, 0, 0);
					// }
					// else if(arrayPosition == 8){
					// 	book.transform.SetParent(Camera.main.transform);
					// 	book.transform.localPosition = new Vector3(50, 60, 150);//these numbers need hard-coded jiggling
					// 	book.transform.localEulerAngles = new Vector3(0, 0, 0);
					// }
					// else if(arrayPosition == 9){
					// 	book.transform.SetParent(Camera.main.transform);
					// 	book.transform.localPosition = new Vector3(-50, 60, 150);//these numbers need hard-coded jiggling
					// 	book.transform.localEulerAngles = new Vector3(0, 0, 0);
					// 	tooManyBooks = true;
					// }
					if(numOpenBooks < 4){
						titles[arrayPosition] = title;
						Destroy(heldObject);//does this set things to null or do you now have a weird broken ref?
						numOpenBooks ++;
						
						//clear out variables (this is garbage that returns a null error but its fine)
						heldObject = null;
						held = null;
						title = "no book selected";

						Debug.Log("updated array: " + openBooks[0] + ", " + openBooks[1] + ", " + openBooks[2] + ", " + openBooks[3] + ", " + openBooks[4]);
					}
					else{
						Debug.Log("There are too many books open!  Close a book in order to open this one.");
					}
				}
			}
		}
	}
	public static string GetTitle(){
		return title;
	}

	public static void setHeldObject(GameObject grabbed){
		heldObject = grabbed;
		held = heldObject.GetComponent<Collider>();
	}

	public static GameObject getHeldObject(){
		return heldObject;
	}
}
