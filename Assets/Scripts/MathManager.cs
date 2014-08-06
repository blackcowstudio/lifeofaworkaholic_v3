// MathManager.cs 
// MathManager Class that contains all calculation-related methods to be simplified for ease of use in other classes.

using System;
using System.Collections;

public static class MathManager 
{
	// Random class instance requires to be static for the random.Next(int) to output different results.
	private static Random _rand = new Random();
	
	// Returns a random int based on n
	public static int RandomInt(int n)
	{
		return _rand.Next(n);
	}

}
