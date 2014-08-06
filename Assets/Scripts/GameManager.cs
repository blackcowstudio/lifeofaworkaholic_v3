// GameManager.cs 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	private GameObject[,] CardPosition;
	private int row;
	private int column;
	private DrawManager draw;
	private string CurrentHit;
	private string LastHit;
	private bool FirstHit;
	private int currentRow;
	private int currentCol;
	private int lastRow;
	private int lastCol;
	private GameObject currentCard;
	private GameObject lastCard;
	private int currentConnect;
	private int maxConnect = 12;
	private List<GameObject> connectedObj;
	private List<GameObject> drawLines;

	// Use this for initialization
	void Start () 
	{
		SetupBoard(25,5);
	
	}

	// Update is called once per frame
	void Update () 
	{	
		// Update event on Mouse Down, using GetMouseButton allows for repeat fires to the input while GetMouseButtonDown only fire once.
		if(Input.GetMouseButton(0))
		{	
			//Get Mouse Position when left click
			Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Debug.Log(MousePos);

			// Test for hit by Mouse
			RaycastHit2D hit = Physics2D.Raycast(MousePos, Vector2.zero);

			// Check for firstHit object
			if (hit.collider != null && CurrentHit != hit.collider.name && !FirstHit)
			{
				CurrentHit = hit.collider.name;
				LastHit = hit.collider.name;
				FirstHit = true;
				currentConnect = 1;
				connectedObj = new List<GameObject>();
				drawLines = new List<GameObject>();

				foreach (GameObject i in draw.CardPosition)
				{
					if (i.name == CurrentHit) 
					{
						connectedObj.Add(i);
					}
				}
			}

			// Check when second hit object is detected
			if (hit.collider != null && CurrentHit != hit.collider.name && FirstHit && currentConnect < maxConnect)
			{
				// set CurrentHit
				CurrentHit = hit.collider.name;

				// find out Row and Column IDs of both CurrentHit and LastHit GameObject
				foreach (GameObject i in draw.CardPosition)
				{
					if (i.name == CurrentHit) 
					{
						currentCard = i;
						currentRow = i.GetComponent<Work>().myRowID;
						currentCol = i.GetComponent<Work>().myColumnID;
					}

					if (i.name == LastHit) 
					{
						lastCard = i;
						lastRow = i.GetComponent<Work>().myRowID;
						lastCol = i.GetComponent<Work>().myColumnID;
					}
				}

				// Debug.Log(string.Format("Current: {0},{1} ", currentRow,currentCol) + string.Format("Last: {0},{1} ", lastRow,lastCol));
				// check if the two hits are connected
				if (Mathf.Abs(currentRow - lastRow) + Mathf.Abs(currentCol - lastCol) == 1)
				{
					Debug.Log("Connected" + CurrentHit + "with " + LastHit);

					// draw Line
					GameObject draw = new GameObject();
					drawLines.Add(draw);
					LineRenderer line = draw.AddComponent<LineRenderer>();
					line.material =  new Material(Shader.Find("Particles/Additive"));
					line.SetVertexCount(2);
					line.SetWidth(0.1f,0.1f);
					line.SetColors(Color.green, Color.green);
					line.SetPosition(0, currentCard.transform.position);
					line.SetPosition(1, lastCard.transform.position);

					// save CurrentHit as LastHit
					LastHit = CurrentHit;

					// save CurrentCard
					connectedObj.Add(currentCard);
					currentConnect++;

				}
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			FirstHit = false;

			foreach (GameObject i in connectedObj)
			{
				Destroy(i);
			} 

			foreach (GameObject i in drawLines)
			{
				Destroy(i);
			} 

			//SetupBoard(25,5);

			Debug.Log("Disconnected");
		}
	}
		
	// SetupBoard with List and Column
	void SetupBoard(int l, int c) 
	{
		// load asset using Resource.LoadALL into a Sprite Array
		Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/icons");

		// instantiate Card Class
		CardManager card = new CardManager(l,c);
		
		// an array of GameObjects to store eachs individual card position
		CardPosition = new GameObject[(card.ListSize/card.ColumnSize),card.ColumnSize];
		
		
		// for loop to create list of caards
		for (int i = 0; i<card.ListSize; i++)
		{ 
			// depending on listSize, calculate no. of rows dynamically based on column size
			row = i / card.ColumnSize;
			column = i % card.ColumnSize;
			
			// create GameObject to hold sprite
			GameObject obj = new GameObject();
			
			// AddComponent of Work script
			obj.AddComponent<Work>();
			obj.GetComponent<Work>().myRowID = row;
			obj.GetComponent<Work>().myColumnID = column;
			
			// name the created GameObject with string by {0} -> row , {1} -> column , {2} -> color int
			obj.name = string.Format("card_{0}_{1}_{2}", row , column, card.CardList[i]);
			
			// Attach BoxCollider2D to GameObject and turn it as trigger
			BoxCollider2D coll = obj.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
			coll.isTrigger = true;
			
			// Attach SpriteRenderer to GameObj
			SpriteRenderer rend = obj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
			
			// Assign sprite to SpriteRenderer
			rend.sprite = sprites[card.CardList[i]];

			obj.AddComponent<DrawManager>();
			// Set position for Sprite
			// Need a better way to set position dynamically based on screen width and height and sprite width and height
			obj.GetComponent<DrawManager>().SetObjTransformXY(obj, (column * 1.28f) - 2.56f, (row * 1.28f) - 5.04f);
			
			// add GameObject to multidimension array
			CardPosition[row,column] = obj;
		}
	}	
}


