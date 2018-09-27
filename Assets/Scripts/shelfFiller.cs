using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Fills the shelves with shelfBooks from the Text folder on Start

public class shelfFiller : MonoBehaviour {

	//start here for each shelf--should this be a vector3??
	public float bookXPosition; 
	public float bookYPosition; 
	public float bookZPosition;
	public float swivel = 285.0f; //rotating the books to face the user

	private const float SHELF_SPACING = 40;//height of each shelf 
	private const float SHELF_WIDTH = 75;
	private const int SHELF_NUM = 5;//number of shelves per shelf
	private int currentShelfNum = 1;//what shelf are you on?
	private int currentBookshelfNum = 0; //bookshelves are larger than shelves

	private GameObject[] shelves;

	//the number of times we're looping over the collection to make it look like the shelves are full
	public int collectionLoops;

	// Use this for initialization
	//the order of everything in here is kind of fragile?
	void Start () {
		bookXPosition = 1.7f; 
		bookYPosition = 4.1f; 
		bookZPosition = -7.0f;
		shelves = GameObject.FindGameObjectsWithTag("shelf");
		GameObject currentBookshelf = shelves[currentBookshelfNum];
		string path = Application.dataPath + "/Text";
	    string[] filePaths = Directory.GetFiles(@path, "*.txt");
	    for(int i = 0; i<= collectionLoops; i++){
		    foreach (string file in filePaths)
		    {
		     	//create the new book, assign it's details
		        int startNamePosition = file.LastIndexOf('/');//where does the name of the text file start?
		        startNamePosition += 6; //deal with the fact that it likes to include "/Text\"
		        string fileName = file.Substring(startNamePosition);
		        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		        shelfBook currentBook = cube.AddComponent<shelfBook>() as shelfBook;
		      	currentBook.setName(fileName);

		      	//set position
		      	if(bookZPosition <= SHELF_WIDTH){//this book will fit on the shelf
			      	currentBook.transform.parent = currentBookshelf.transform;
			      	currentBook.transform.localPosition = new Vector3(bookXPosition, bookYPosition, bookZPosition);
			      	bookZPosition += 7.0f;
			      	bookXPosition += 2.0f;
			      	currentBook.transform.localEulerAngles = new Vector3(0, 285, 0);
			      	//Debug.Log(currentBook + "placed at " + currentBook.transform.position);
			    }
			    else{
			    	if(currentShelfNum < 5){//there's another shelf on this bookshelf
				    	bookZPosition = -7.0f;
				    	bookYPosition -= SHELF_SPACING;
				    	bookXPosition = 1.7f;
				    	currentShelfNum ++;
				    	currentBook.transform.parent = currentBookshelf.transform;
			      		currentBook.transform.localPosition = new Vector3(bookXPosition, bookYPosition, bookZPosition);
			      		bookZPosition += 7.0f;
			      		bookXPosition += 2.0f;
			      		currentBook.transform.localEulerAngles = new Vector3(0, 285, 0);
			      		//Debug.Log(currentBook + "placed at " + currentBook.transform.position);
				    }
				    else{//go to the next bookshelf
				    	currentBookshelfNum ++;
				    	currentBookshelf = shelves [currentBookshelfNum];
				    	bookXPosition = 1.7f; 
						bookYPosition = 4.1f; 
						bookZPosition = -7.0f;
						currentShelfNum = 1;
						currentBook.transform.parent = currentBookshelf.transform;
			      		currentBook.transform.localPosition = new Vector3(bookXPosition, bookYPosition, bookZPosition);
			      		bookZPosition += 7.0f;
			      		bookXPosition += 2.0f;
			      		currentBook.transform.localEulerAngles = new Vector3(0, 285, 0);
			      		//Debug.Log(currentBook + "placed at " + currentBook.transform.position);
				    }
			    }
		      	//the good way to go to the next book, but currently broken.
		      	// bookZPosition += currentBook.getWidth();
		      	// Debug.Log("increment by: "+ currentBook.getWidth());
		      	// Debug.Log("New Z> "+ bookZPosition);

		      	//set remaining book details
		      	string[] nameParts = fileName.Split('-');
		      	currentBook.setAuthor(nameParts[0]);
		      	currentBook.setTitle(nameParts[1]);
		      	
		    }
	    }
	}
	
	// Update is called once per frame
	void Update () {
	}
}
