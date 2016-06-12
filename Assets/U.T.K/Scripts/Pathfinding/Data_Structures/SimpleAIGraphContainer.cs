using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//TODO Find a way to resolve the need for a reference refresh.
[Serializable]
public class SimpleAIGraphContainer : MonoBehaviour {
	[SerializeField]
	private GraphSimple Graph=new GraphSimple();
	// Use this for initialization
	void Start (){
		if(Graph==null)
			Graph= new GraphSimple();
		RefreshReferences();
		//List<pathNode> path=Dijkstra.ShortestPath(Graph,0,2);
	}

	public GraphSimple GetGraph()
	{
		RefreshReferences();
		return Graph;
	}

	public void RefreshReferences()
	{
		if(Graph.NodeCount>1){
			for(int j=0;j<Graph.NodeCount;j++){
				for(int i=0;i<Graph.graph[j].neighbors.Count;i++){
					Graph.graph[j].neighbors[i]=Graph.graph[Graph.graph[j].neighbors[i].ID];
				}
			}
		}
	}
	//Call delete function for graph as well as reorganize the nodes in the editor net
	public void DeleteNode(int A)
	{
		Graph.DeleteNode(A);
		Debug.Log ("Deleting!");
		pathNodeContainer [] nodeObjs = gameObject.GetComponentsInChildren<pathNodeContainer>();
		Debug.Log (nodeObjs.Length);
		for(int i=A;i<nodeObjs.Length;i++){
			nodeObjs[i].Node=Graph.GetNodeList()[i];
			nodeObjs[i].GenerateName();
		}
	}

	//Call Merge function for graph as well as reorganize the nodes in the editor net
	public void MergeNodes(int From, int To)
	{
		Graph.MergeNodes(From,To);
		Debug.Log ("Merging!");
		pathNodeContainer [] nodeObjs = gameObject.GetComponentsInChildren<pathNodeContainer>();
		Debug.Log (nodeObjs.Length);
		for(int i=From;i<nodeObjs.Length;i++){
			nodeObjs[i].Node=Graph.GetNodeList()[i];
			nodeObjs[i].GenerateName();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "stepfinal2.png");
		if(Graph==null)
			return;
		if(Graph.NodeCount>1){
			for(int j=0;j<Graph.NodeCount;j++){
				for(int i=0;i<Graph.graph[j].neighbors.Count;i++){
					Debug.DrawLine(Graph.graph[j].position,Graph.graph[j].neighbors[i].position,Color.blue,0);
				}
			}
		}
	}
}