using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Dijkstra{
	//public List<pathNode> shortestPath = new List<pathNode>();
	
	public static List<pathNode> ShortestPath(GraphSimple graphIn,int SourceIndex,int DestinationIndex)
   {
		//Create temp list for shortest path
		List<pathNode> shortestPath = new List<pathNode>();
		List<pathNode> graph = graphIn.GetNodeList();

		if(SourceIndex==DestinationIndex){
			shortestPath.Add (graph[SourceIndex]);
			Debug.Log ("Indices the same!");
			return shortestPath;
		}
		//Reset distances
		//Find source and destination nodes
		for(int i=0;i<graph.Count;i++){
			graph[i].dist=Mathf.Infinity;
			graph[i].previous=null;
			graph[i].Removed=false;
		}

		//Set the initial node as a distance of 0
		graph[SourceIndex].dist=0;
		//While nodes are not empty
		int nodesLeft=graph.Count;

		while(nodesLeft>0)
		{
			pathNode min=new pathNode();
			//Grab shortest distance point. Accounts for ALL nodes
			for(int i=0;i<graph.Count;i++){
				if((graph[i].dist<min.dist)&&!graph[i].Removed){
					min=graph[i];
				}
			}

			//Re-reference min to a new name. Combine them?
			pathNode smallestpathNode=min;
			//Remove node
			graph[smallestpathNode.ID].Removed=true;
			nodesLeft--;
			//Exit condition. Destination Reached!
			if((smallestpathNode.ID==DestinationIndex))
			{
				//Debug.Log ("Found!");
				return getPath(smallestpathNode);
			}
			//We can't reach the node!
			else if(smallestpathNode.dist==Mathf.Infinity){
				//Debug.Log("No connection!");
				return shortestPath;
			}
			
			//List<pathNode> neighbors = smallestpathNode.neighbors;
			for(int i=0;i<smallestpathNode.neighbors.Count;i++)
			{

				float alt = smallestpathNode.dist + smallestpathNode.weight[i];

				if(alt < smallestpathNode.neighbors[i].dist)
				{
					smallestpathNode.neighbors[i].dist=alt;
					smallestpathNode.neighbors[i].previous=smallestpathNode;
				}
			}
		}
		Debug.Log("All Nodes Ventured");
		return shortestPath;
   }

	public static List<float> AllDistancesFromNode(GraphSimple graphIn,int SourceIndex)
	{
		ShortestPath(graphIn,SourceIndex,graphIn.NodeCount);
		return GetDistances(graphIn);
	}

	private static List<float> GetDistances(GraphSimple graphIn)
	{
		List<float> distances= new List<float>();
		List<pathNode> graph = graphIn.GetNodeList();
		for(int i=0;i<graph.Count;i++){
			distances.Add(graph[i].dist);
		}

		return distances;
	}

	private static List<pathNode> getPath(pathNode Destination)
	{
		pathNode CurrentNode = Destination;
		List<pathNode> Path= new List<pathNode>();
		Path.Add (CurrentNode);
		do { CurrentNode=CurrentNode.previous; Path.Insert(0,CurrentNode);}
		while(CurrentNode.previous!=null);
		return Path;

	}
}