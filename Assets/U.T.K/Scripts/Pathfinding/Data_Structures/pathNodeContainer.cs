using UnityEngine;
using System.Collections;
using System;
[Serializable]
public class pathNodeContainer : MonoBehaviour {
	public pathNode Node;

	public void GenerateName(){
		gameObject.name="Node "+Node.ID;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "Node.png");
	}
}
