
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class path{
	public List<pathNode> pathway;
	public float distance;
	public int Start,End;
	public path(List<pathNode> pathway, int Start, int End)
	{
		this.pathway=pathway;
		this.Start=Start;
		this.End=End;
		this.distance=pathway[pathway.Count-1].dist;
	}

	public bool isConnecting(int NodeA, int NodeB, out bool reversed)
	{
		reversed=false;
		if((NodeA==Start&&NodeB==End))
			return true;

		else if(NodeB==Start&&NodeA==End){
			reversed=true;
			return true;
		}
		return false;
	}
}
/*
public class GraphLink
{
	public int GraphLinkID;
	public int GraphLinkNode;
	public GraphSimple LinkedGraph;

	public GraphLink(GraphSimple Link,int ID,int Node)
	{
		this.GraphLinkID=ID;
		this.GraphLinkNode=Node;
		this.LinkedGraph=Link;
	}

}
*/
[Serializable]
public class GraphSimple{
	public List<pathNode> graph = new List<pathNode>();
	public int NodeCount;
	public int GraphID;
	public GraphSimple()
	{
		NodeCount=0;
	}

	public GraphSimple(int count)
	{
		for(int i=0;i<count;i++){
			graph.Add(new pathNode());
			graph[i].ID=i;
		}

		NodeCount=count;
	}
	
	public void AddNodes(int count)
	{
		for(int i=0;i<count;i++){
			graph.Add(new pathNode());
			graph[i+NodeCount].ID=i+NodeCount;
		}

			NodeCount+=count;
	}

	//Add transition points
	//public List<GraphLink> GraphLinks = new List<GraphLink>();
	public List<GraphSimple> NeighborGraphs = new List<GraphSimple>();
	public List<int> TransitionNodes = new List<int>();
	public List<pathNode> Links = new List<pathNode>();

	public void LinkGraph(GraphSimple linkedGraph,int NodePos,pathNode Link)
	{
		foreach(GraphSimple link in NeighborGraphs){
			if(link==linkedGraph)
				return;
		}

		NeighborGraphs.Add(linkedGraph);
		TransitionNodes.Add(NodePos);
		Links.Add(Link);
	}


	public pathNode GetClosestNode(Vector3 position)
	{
		float distance=Mathf.Infinity;
		pathNode ClosestNode = new pathNode();
		foreach(pathNode Node in graph)
		{
			float currDist = Vector3.Distance(Node.position,position);
			if(distance>currDist)
			{
				distance=currDist;
				ClosestNode=Node;
			}
		}
		return ClosestNode;
	}

	//Build all possible paths between transition points. Also construct the graph that links them together
	public List<path> bakedPaths = new List<path>();
	public void BakePaths()
	{
		bakedPaths.Clear();
		for(int i=0;i<Links.Count;i++)
		{
			for(int j=i+1;j<Links.Count;j++)
			{
				int NodeA=TransitionNodes[i];
				int NodeB=TransitionNodes[j];
				Debug.Log("Baking path from node "+NodeA+"("+Links[i].ID+")"+" to node "+NodeB+"("+Links[j].ID+")"+" in graph "+GraphID);
				bakedPaths.Add(new path(Dijkstra.ShortestPath(this,NodeA,NodeB),Links[i].ID,Links[j].ID));
				Debug.Log ("Distance is :"+bakedPaths[bakedPaths.Count-1].distance);
				Links[i].neighbors.Add(Links[j]);
				Links[i].weight.Add(bakedPaths[bakedPaths.Count-1].distance);

				Links[j].neighbors.Add(Links[i]);
				Links[j].weight.Add(bakedPaths[bakedPaths.Count-1].distance);
			}
		}
		/*
		Debug.Log ("Baked data for Graph "+GraphID+" :");
		for(int i =0;i<Links.Count;i++)
		{
			Debug.Log ("Transition "+Links[i].ID+" at graph node "+TransitionNodes[i]+" connects to graph "+NeighborGraphs[i].GraphID);
			for(int j =0;j<Links[i].neighbors.Count;j++)
			{
				pathNode neighbor = Links[i].neighbors[j];
				Debug.Log ("Neighbor "+neighbor.ID+" Weight "+Links[i].weight[j]);
			}
		}
		*/
	}

	public List<pathNode> GetBakedPath(int A, int B)
	{
		bool reversed=false;
		foreach(path Path in bakedPaths){
			//Debug.Log("Checking Path from "+Path.Start+" to "+Path.End);
			if(Path.isConnecting(A,B,out reversed))
			{
				if(reversed){
					int End=Path.End;
					Path.End=Path.Start;
					Path.Start=End;
					Path.pathway.Reverse();
				}
				return Path.pathway;
			}
		}
		Debug.Log ("Invalid node values");
		return new List<pathNode>();
	
	}

	public bool ConnectNodes(int A, int B, float weight)
	{
		if(A>=NodeCount||B>=NodeCount)
			return false;

		if(A==B)
			return false;

		if(graph[A].neighbors.Contains(graph[B])&&graph[B].neighbors.Contains(graph[A])){
			graph[A].weight[graph[A].neighbors.IndexOf(graph[B])]=weight;
			graph[B].weight[graph[B].neighbors.IndexOf(graph[A])]=weight;
			return true;
		}
		else{
			graph[A].neighbors.Add (graph[B]);
			graph[A].weight.Add (weight);
			graph[B].neighbors.Add (graph[A]);
			graph[B].weight.Add (weight);
			return false;
		}

	}

	public List<int> GetNeighborsIDs(int A)
	{
		List<int> IDs = new List<int>();
		foreach(pathNode nodes in graph[A].neighbors)
		{
			IDs.Add(nodes.ID);
		}
		return IDs;
	}

	public List<pathNode> GetNeighborsNodes(int A)
	{
		return graph[A].neighbors;
	}
	//Migrate all neighbors first. Then delete the node.
	public void MergeNodes(int From, int To)
	{
		for(int i=0;i<graph[From].neighbors.Count;i++)
		{
			ConnectNodes(To,graph[From].neighbors[i].ID,graph[From].weight[i]);
		}
		DeleteNode(From);
	}
	//Used to remove a node from the graph. Takes care of all references to the node
	public void DeleteNode(int A)
	{
		for(int i=0;i<graph.Count;i++)
		{
			for(int j = 0;j<graph[i].neighbors.Count;j++){
				pathNode GraphLoc=graph[i].neighbors[j];
				//Remove all instances of the pathNode from all connections in the graph
				if(GraphLoc.ID==A){
					graph[i].neighbors.RemoveAt(j);
					graph[i].weight.RemoveAt(j);
				}
			}
		}

		for(int i=0;i<graph.Count;i++)
		{
			//Decrement the ID on each node that occurs AFTER the deleted node. 
			if(graph[i].ID>A)
				graph[i].ID-=1;
		}
		//Finally, remove the Node from the graph. 
		graph.RemoveAt(A);
		NodeCount--;
	}

	public List<pathNode> GetNodeList()
	{
		return graph;
	}

	public void Clear()
	{
		graph.Clear();
		NodeCount=0;
	}
/*
	public override string ToString ()
	{
		return string.Format ("Graph Count: "+graph.Count+"\n"
	}
	*/
}
