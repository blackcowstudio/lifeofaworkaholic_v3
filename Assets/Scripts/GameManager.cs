// GameManager.cs 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	private StateManager _state;

	// Use this for initialization
	void Start () 
	{
		// Get StateManager Component
		_state = GetComponent<StateManager>();
	}

	// Update is called once per frame
	void Update () 
	{	

	}
}


