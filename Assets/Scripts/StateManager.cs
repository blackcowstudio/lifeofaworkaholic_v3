using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

	// declare DrawManager instance
	private DrawManager draw; 

	// enum to determine states in the state machine
	public enum State
	{
		Start,
		Draw,
		Destroy,
		Result,
		Reset
	}

	public State state = State.Start; 

	IEnumerator StartState () 
	{
		Debug.Log("Start: Enter");
		while (state == State.Start) 
		{
			draw.SetupBoard(25,5);
			yield return 0; 
		}
		Debug.Log ("Start: Exit");
	}

	IEnumerator DrawState () 
	{
		Debug.Log ("Draw: Enter");
		while (state == State.Draw) 
		{
			yield return 0; 
		}
		Debug.Log ("Draw: Exit");
	}

	IEnumerator DestroyState () 
	{
		Debug.Log ("Destroy: Enter");
		while (state == State.Destroy) 
		{
			yield return 0; 
		}
		Debug.Log ("Destroy: Exit");
	}

	IEnumerator ResultState () 
	{
		Debug.Log ("Result: Enter");
		while (state == State.Result) 
		{
			yield return 0; 
		}
		Debug.Log ("Result: Exit");
	}

	IEnumerator ResetState () 
	{
		Debug.Log ("Reset: Enter");
		while (state == State.Reset) 
		{
			yield return 0; 
		}
		Debug.Log ("Reset: Exit");
	}

	// Use this for initialization
	void Start () 
	{
		// kick off the first state of the state machine
		//NextState ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// invoke nextState of the statemachine
	void NextState () 
	{
		string methodName = state.ToString() + "State";
		System.Reflection.MethodInfo info =
			GetType().GetMethod(methodName,
			                    System.Reflection.BindingFlags.NonPublic |
			                    System.Reflection.BindingFlags.Instance);
		StartCoroutine((IEnumerator)info.Invoke(this, null));
	}
}
