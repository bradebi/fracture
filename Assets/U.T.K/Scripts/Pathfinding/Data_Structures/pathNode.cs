using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public enum Tag{

[Serializable]
public class pathNode{

	public float dist=Mathf.Infinity;

	public List<pathNode> neighbors=new List<pathNode>();
	public List<float> weight=new List<float>();

	public pathNode previous; //Used for path backtrack

	public Vector3 position;

	public bool passThrough;

	public string tag;

	public bool Removed=false;

	public bool Visited=false;

	public int ID=0;

	public pathNode()
	{
		position=Vector3.zero;
	}
}