using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class MultiGraphDijkstra {
	/*
	public static List<pathNode> ShortestPath(MultiGraph graphList,int SourceIndex,GraphSimple SourceGraph,int DestinationIndex, GraphSimple DestinationGraph)
	{

		//Set source Node to position and recalculate its weights/distances
		pathNode sourceSparseNode=sparseGraph.GetNodeList()[SourceGraph.GraphID];
		pathNode sourceNode = SourceGraph.GetNodeList()[SourceIndex];
		sourceSparseNode.position=sourceNode.position;
		int index=0;
		foreach(pathNode node in sourceSparseNode.neighbors){
			float dist=Vector3.Distance(node.position,sourceNode.position);
			sourceSparseNode.weight[index++]=dist;
			node.weight[node.neighbors.IndexOf(sourceSparseNode)]=dist;
		}

		//Set destination Node to position and recalculate its weights/distances
		pathNode destSparseNode=sparseGraph.GetNodeList()[DestinationGraph.GraphID];
		pathNode destNode = DestinationGraph.GetNodeList()[DestinationIndex];
		destSparseNode.position=destNode.position;
		index=0;
		foreach(pathNode node in destSparseNode.neighbors){
			float dist=Vector3.Distance(node.position,destNode.position);
			destSparseNode.weight[index++]=dist;
			node.weight[node.neighbors.IndexOf(destSparseNode)]=dist;
		}

		//Get shortest path with sparse graph
		List<pathNode> sparsePath=Dijkstra.ShortestPath(sparseGraph,sourceSparseNode.ID,destSparseNode.ID);

		List<pathNode> shortestPath=new List<pathNode>();
		List<int> traversedGraphs=new List<int>();
		//Begin search with path toward door labeled by sparsePath #1
		shortestPath.AddRange(Dijkstra.ShortestPath(SourceGraph,SourceIndex,));
		traversedGraphs.Add(SourceGraph.GraphID);
		//Find the graphs to traverse based on sparsePath
		for(int i=0; i<sparsePath.Count;i++)
		{


			//shortestPath.AddRange(
		}
	}
	*/
}
