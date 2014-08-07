using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

	private DrawManager _draw;
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
		Debug.Log ("Are we here yet?");
		StartCoroutine ("StartState");
	}
	
	// Update is called once per frame
	void Update () {

		// Mouse Update
		if (currentState == State.Draw) {

			Debug.Log ("Draw State");

			// Update event on Mouse Down, using GetMouseButton allows for repeat fires to the input while GetMouseButtonDown only fire once.
				if (Input.GetMouseButton (0)) {	
						//Get Mouse Position when left click
						Vector2 MousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						//Debug.Log(MousePos);
	
						// Test for hit by Mouse
						RaycastHit2D hit = Physics2D.Raycast (MousePos, Vector2.zero);
	
						// Check for firstHit object
						if (hit.collider != null && CurrentHit != hit.collider.name && !FirstHit) {
								CurrentHit = hit.collider.name;
								LastHit = hit.collider.name;
								FirstHit = true;
								currentConnect = 1;
								connectedObj = new List<GameObject> ();
								drawLines = new List<GameObject> ();
		
								foreach (GameObject i in _draw.CardPosition) {
										if (i.name == CurrentHit) {
												connectedObj.Add (i);
										}
								}
						}
	
						// Check when second hit object is detected
						if (hit.collider != null && CurrentHit != hit.collider.name && FirstHit && currentConnect < maxConnect) {
								// set CurrentHit
								CurrentHit = hit.collider.name;
		
								// find out Row and Column IDs of both CurrentHit and LastHit GameObject
								foreach (GameObject i in _draw.CardPosition) {
										if (i.name == CurrentHit) {
												currentCard = i;
												currentRow = i.GetComponent<Work> ().myRowID;
												currentCol = i.GetComponent<Work> ().myColumnID;
										}
			
										if (i.name == LastHit) {
												lastCard = i;
												lastRow = i.GetComponent<Work> ().myRowID;
												lastCol = i.GetComponent<Work> ().myColumnID;
										}
								}
		
								// Debug.Log(string.Format("Current: {0},{1} ", currentRow,currentCol) + string.Format("Last: {0},{1} ", lastRow,lastCol));
								// check if the two hits are connected
								if (Mathf.Abs (currentRow - lastRow) + Mathf.Abs (currentCol - lastCol) == 1) {
										Debug.Log ("Connected" + CurrentHit + "with " + LastHit);
			
										// draw Line
										GameObject draw = new GameObject ();
										drawLines.Add (draw);
										LineRenderer line = draw.AddComponent<LineRenderer> ();
										line.material = new Material (Shader.Find ("Particles/Additive"));
										line.SetVertexCount (2);
										line.SetWidth (0.1f, 0.1f);
										line.SetColors (Color.green, Color.green);
										line.SetPosition (0, currentCard.transform.position);
										line.SetPosition (1, lastCard.transform.position);
			
										// save CurrentHit as LastHit
										LastHit = CurrentHit;
			
										// save CurrentCard
										connectedObj.Add (currentCard);
										currentConnect++;
			
								}
						}
				}

				// on MouseUp Event
				if (Input.GetMouseButtonUp (0)) {
						currentState = State.Destroy;
						Debug.Log ("Disconnected");
						StartCoroutine ("DestroyState");
				}
			}
	}

	// using Coroutine method to do stateMachine

	IEnumerator StartState() 
	{
		Debug.Log ("Start State Coroutine Method");
		// Get DrawManager Component
		_draw = GetComponent<DrawManager>();
		_draw.SetupBoard(25,5);
		currentState = State.Draw;
		//StartCoroutine ("DrawState");
		yield return null;
	}

	IEnumerator DestroyState()
	{
		Debug.Log ("Destroy State");
		// reset bool for next draw
		FirstHit = false;
		// destroy all lines 
		foreach (GameObject i in drawLines)
		{
			Destroy(i);
		} 
		// destroy all cards with a 0.1 second delay each
		foreach (GameObject i in connectedObj)
		{
			Destroy(i);
			yield return new WaitForSeconds (0.1f);
		} 
		
		currentState = State.Result;
		yield return null;
		StartCoroutine ("ResultState");
	}

	IEnumerator ResultState()
	{
		Debug.Log ("Result State");	
		currentState = State.Reset;
		yield return new WaitForSeconds(1f);
		StartCoroutine ("ResetState");
	}

	IEnumerator ResetState()
	{
		Debug.Log ("Reset State");
		_draw.DestroyBoard ();
		_draw.SetupBoard(25,5);
		currentState = State.Draw;
		yield return new WaitForSeconds(1f);
	}
	
}
