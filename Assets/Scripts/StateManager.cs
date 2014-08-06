using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

	public DrawManager _draw;
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

	// enum to determine states in the state machine
	public enum State
	{
		Start,
		Draw,
		Destroy,
		Result,
		Reset
	}

	public State currentState = State.Start; 
	

	// Use this for initialization
	void Start () 
	{
		// kick off the first state of the state machine
		//NextState ();
	}
	
	// Update is called once per frame
	void Update () {
		// doing level 1 state machine
		switch (currentState) 
		{
		case State.Start:
			Debug.Log ("Start State");
			// Get DrawManager Component
			_draw = GetComponent<DrawManager>();
			_draw.SetupBoard(25,5);
			currentState = State.Draw;
			break;

		case State.Draw:
			Debug.Log ("Draw State");

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
					
					foreach (GameObject i in _draw.CardPosition)
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
					foreach (GameObject i in _draw.CardPosition)
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
				StartCoroutine(Wait(2.0f));
				currentState = State.Destroy;
				Debug.Log("Disconnected");
			}

			break;

		case State.Destroy:
			Debug.Log ("Destroy State");

			FirstHit = false;
			
			foreach (GameObject i in connectedObj)
			{
				Destroy(i);
			} 
			
			foreach (GameObject i in drawLines)
			{
				Destroy(i);
			} 

			StartCoroutine(Wait(5.0f));

			currentState = State.Result;

			break;

		case State.Result:
			Debug.Log ("Result State");
			StartCoroutine(Wait(5.0f));
			currentState = State.Reset;
			break;
		
		case State.Reset:
			Debug.Log ("Reset State");
			_draw.SetupBoard(25,5);
			currentState = State.Draw;
			break;
		}
	
	}

	IEnumerator Wait(float waitTime) 
	{
		yield return new WaitForSeconds (waitTime);
	}
}
