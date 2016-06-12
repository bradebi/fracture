using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour {

	// Use this for initialization
	public enum Sector
    {
        None,
        Biology,
        Bridge,
        Commercial,
        Education,
        Engineering,
        Lobby,
        Medbay,
        Mining,
        Residential
    }
	public enum WindowState
	{
		Open,
		Closed,
		Cracked,
		Broken
	}
	
	public int ID = 105090;
	public Sector sector;
	public WindowState windowstate;
	//public 
}
