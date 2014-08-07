using UnityEngine;
using System.Collections;

public class DrawManager : MonoBehaviour
{
	public GameObject[,] CardPosition;
	private int row;
	private int column;

	// SetupBoard with List and Column
	public void SetupBoard(int l, int c) 
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

	public void DestroyBoard() 
	{
		foreach (GameObject i in CardPosition)
		{
			Destroy(i);
		}
	}

	// Transform methods
	public void SetObjScaleXY(GameObject obj, float n, float m) 
	{
		obj.transform.localScale = new Vector3(n, m, obj.transform.localScale.z);
	}
	
	public void SetObjTransformXY(GameObject obj, float n, float m)
	{
		obj.transform.position = new Vector3(n, m, obj.transform.position.z);
	}
	
	public void SetObjTransformX(GameObject obj, float n)
	{
		obj.transform.position = new Vector3(n, transform.position.y, obj.transform.position.z);
	}
	
	public void SetObjTransformY(GameObject obj, float n)
	{
		obj.transform.position = new Vector3(obj.transform.position.x, n, obj.transform.position.z);
	}
}

