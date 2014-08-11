// First attempt on writing Abstract Class for Card
// Objective: Trying to understand how,why and when should Abstract Classes be made. 
// Refering to Second Example of Properties Tutorial at http://msdn.microsoft.com/en-us/library/aa288470(v=vs.71).aspx
using UnityEngine;
using System;
using System.Collections;

public abstract class aCard : MonoBehaviour
{
	// Properties
	private string ColorID;
	private string CardType;
	private int RowID;
	private int ColumnID;

	// Constructor
	public aCard() 
	{
	
	}
 	
	public string myColor
	{
		get
		{
			return ColorID;
		}
		set
		{
			ColorID = value;
		}
	}

	public string myCardType
	{
		get
		{
			return CardType;
		}
		set
		{
			CardType = value;
		}
	}

	public int myRowID
	{
		get
		{
			return RowID;
		}
		set
		{
			RowID = value;
		}
	}

	public int myColumnID
	{
		get
		{
			return ColumnID;
		}
		set
		{
			ColumnID = value;
		}
	}
}

public class Work : aCard 
{

}

public class Food : aCard 
{
	
}

public class Hangout : aCard 
{
	
}

public class Exercise : aCard 
{
	
}

public class Relationship : aCard 
{
	
}

public class Vacation : aCard 
{
	
}