using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;//for list reasons

//When a book is opened, read the text in the corresponding text file, and put that text on pages
//A mess, but that's because you wrote it early on

public class turnPage : MonoBehaviour {



	//page turning animation
	private Animation anim;

	private bool first = true; //are we on the first page?
	private bool last = false;

	private int currentPage;
	private List<string> pages;

	public string text1;
	public string text2;
	public string text3;
	public string text4;
	public Text page1Text;
	public Text page2Text;
	public Text page3Text;
	public Text page4Text;

	//for Read()
	private StreamReader reader;
	string word = ""; //current word
	string page = ""; //current page --default is protected?
	public int PAGE_LENGTH = 350; //the number of characters which fit on the page-- you might not want const
	public int LINE_LENGTH = 35; //the number of characers which fit on the line
	int currentPageChar = 0; //how many characters into the page are you?
	int currentLineChar = 0; //how many characters into the line are you?
	private string title;



	// Use this for initialization
	void Awake () {
		title = selectBook.GetTitle();
		//Debug.Log("turnPage thinks that the title of this book is " + title);
        anim = GetComponent<Animation>();
        //anim["pageTurn"].speed = 0;//don't play automatically

        //read this book
		pages = new List<string>();
		//reader = new StreamReader("Assets/Text/Siddhartha.txt");//hardcoded for now
        //reader = new StreamReader("Assets/Text/"+ bookMeta.title);//you need to figure out how this is talking to the rest of this thing
        title = selectBook.GetTitle();
        string path = Application.dataPath + "/Text/";
        reader = new StreamReader(path +title);
        Read();

        //- points for style
        currentPage = 0;

        text1 = pages.ElementAt(currentPage);
        currentPage ++;
        text2 = pages.ElementAt(currentPage);
        currentPage ++;
        text3 = pages.ElementAt(currentPage);
        currentPage ++;
        text4 = pages.ElementAt(currentPage);
        //currentPage ++;

        page1Text.text = text1;
        page2Text.text = text2;
        page3Text.text = text3;
        page4Text.text = text4;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("j")){//flip forwards
			if(first == false){
	        	print("j key was pressed");
	        	text1 = text3;
	        	page1Text.text = text1; //these might not be necessary
	        	text2 = text4;
	        	page2Text.text = text4;
	        	anim.Play("pageTurn");

        		currentPage ++;
	        	text3 = pages.ElementAt(currentPage);
                Debug.Log("current page: " + currentPage);

        		currentPage ++;
                text4 = pages.ElementAt(currentPage);
                Debug.Log("current page: " + currentPage);

	        	page3Text.text = text3;
	        	page4Text.text = text4;
	        	last = true;
	        }
	        //turning from the first page is special
	        else{
	        	first = false;
	        	anim.Play("pageTurn");
	        	Debug.Log("the animation should have played?");
	        }
	    }
        if (Input.GetKeyDown("h")){//take it back now y'all
			if(currentPage>=-1){
	        	text3 = text1;
	        	page3Text.text = text3; //these might not be necessary
	        	text4 = text2;
	        	page4Text.text = text4;
	        	//anim.Play("pageTurnBack");//should work but it doesn't?


                text2 = pages.ElementAt(currentPage);
                Debug.Log("current page: " + currentPage);
                currentPage--;

                text1 = pages.ElementAt(currentPage);
                Debug.Log("current page: " + currentPage);
                currentPage--;

                page1Text.text = text1;
	        	page2Text.text = text2;
	        	first = true;
	        }
	        //turning from the first page is special
	        else{
	        	Debug.Log("You can't flip backwards.  There are no more pages.");
				first = true;
	        }
	      }
    }
    	private void Read(){
    		//get the data out of the file and into a better format
    			//why is C# like this?
    		string textString = reader.ReadToEnd();
    		Queue<char> text = new Queue<char>();
    		foreach(char c in textString){
    			text.Enqueue(c);
    		}
    		while(text.Count != 0){//this line is currently the issue
			char nextChar = text.Dequeue();
			if(nextChar == 92 && text.Peek()==110){//if there's a newline character
				page += word;//you're done with that word
				int remainder = LINE_LENGTH - currentLineChar;//# of characters left in the line
				currentPageChar += remainder;
				currentLineChar = 0;//you're at the 0th positionon the new line
				if(currentPageChar >= PAGE_LENGTH){//are you done with the page?
					pages.Add(page);
					page = "";
					currentPageChar = 0;
				}
			}
			else if(nextChar == 32){//is there a space?
				//still add the space
				word += nextChar;
				currentLineChar++;

				if(currentLineChar + word.Length> LINE_LENGTH){//if the word doesn't fit on the line
					int remainder = LINE_LENGTH - currentLineChar;//space left in the line
					currentPageChar += remainder;
					currentLineChar = word.Length;
					if(currentPageChar >= PAGE_LENGTH){//are you done with the page?
						pages.Add(page);
						page = "";
						currentPageChar = 0;
					}
				}
				page += word;
				currentPageChar += word.Length;
				word = "";
			}
			else{//just add a regular char
				word += nextChar;
				currentLineChar++;
			}
    	}
	}
}
