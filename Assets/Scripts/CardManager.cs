using UnityEngine;
using System;
using System.Collections;

public enum Activities 
{
	Work,
	Food,
	Socialize,
	Exercise,
	Relationship
}
public enum Colors 
{
	Purple,
	Green,
	Yellow,
	Grey,
	Red
}


public class CardManager
{
	
	public int ListSize = 25; 
	public int ColumnSize = 5;
	public int RowSize = 5;
	public int[] CardList;

	// constructor
	public CardManager(int listNum, int columnNum) 
	{
		ListSize = listNum;
		ColumnSize = columnNum;
		RowSize = listNum/columnNum;
		SetRandCardList();
	}

	public void SetRandCardList()
	{
		// initialize array with listSize
		CardList = new int[ListSize];

		//get enum size
		int EnumSize = Enum.GetNames(typeof(Activities)).Length;

		//populate multidimension array with values from Enum
		for (int i = 0; i<CardList.Length; i++) {
			int result = MathManager.RandomInt(EnumSize);
			CardList[i] = result;
		}
	}

}
