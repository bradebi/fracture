using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiGraph {
	//Generate this for the door ways/links.
	//Node 0 is reserved for start Node
	//Node N-1 is reserved for end Node
	public GraphSimple sparseGraph;

	//A list of all graphs in the system
	public List<GraphSimple> Graphs;

	//A count of the number of links that exist between graphs
	//Used to give a unique identifier between graphs
	int GraphCount=0;

	public MultiGraph(){
		sparseGraph=new GraphSimple(1);
		Graphs=new List<GraphSimple>();
	}

	//Build sparse Graph connections as well as Bake out paths for all graphs
	public void BakeGraphs(){
		foreach(GraphSimple graph in Graphs){
			graph.BakePaths();
		}

		return;
	}

	//Add graph to masterlist and assign an ID to the graph
	public void AddGraph(GraphSimple Graph){

		if(!Graphs.Contains(Graph)){
			Graph.GraphID=Graphs.Count;
			Graphs.Add(Graph);
			GraphCount++;
		}
	}

	//Create pathNode for sparse graph
	public void ConnectGraphs(int GraphIDA, int GraphNodeA, int GraphIDB,int GraphNodeB){
		sparseGraph.AddNodes(1);
		pathNode linker = sparseGraph.GetNodeList()[sparseGraph.NodeCount-1];
		//Place linker node over doorway
		linker.position=Graphs[GraphIDA].GetNodeList()[GraphNodeA].position;
		
		//Link A to B
		Graphs[GraphIDA].LinkGraph(Graphs[GraphIDB],GraphNodeA,linker);

		//Link B to A
		Graphs[GraphIDB].LinkGraph(Graphs[GraphIDA],GraphNodeB,linker);
	}

	public List<pathNode> ShortestPath(int SourceGraphIndex, int SourceNode, int DestGraphIndex, int DestNode)
	{
		List<pathNode> shortestPath = new List<pathNode>();



		if(SourceGraphIndex==DestGraphIndex)
		{
			if(SourceNode==DestNode)
				return shortestPath;

			return Dijkstra.ShortestPath(Graphs[SourceGraphIndex],SourceNode,DestNode);
		}

		//Set node = to position of Source Node in Source graph
		pathNode sparseSourceNode = sparseGraph.GetNodeList()[0];
		sparseSourceNode.neighbors.Clear();
		sparseSourceNode.weight.Clear();
		sparseSourceNode.position=Graphs[SourceGraphIndex].GetNodeList()[SourceNode].position;
		int NumSourceNeigbors=Graphs[SourceGraphIndex].Links.Count;

		//Calculate weights/connections for Source Node
		foreach(pathNode node in Graphs[SourceGraphIndex].Links){
			//Get distance to each door
			sparseSourceNode.neighbors.Add(node);
			node.neighbors.Add(sparseSourceNode);
			float dist=Vector3.Distance(sparseSourceNode.position,node.position);
			sparseSourceNode.weight.Add(dist);
			node.weight.Add(dist);
		}



		//Set last node = to position of Destination Node in Source graph
		sparseGraph.AddNodes(1);
		pathNode sparseDestNode = sparseGraph.GetNodeList()[sparseGraph.NodeCount-1];
		sparseDestNode.neighbors.Clear();
		sparseDestNode.weight.Clear();
		sparseDestNode.position=Graphs[DestGraphIndex].GetNodeList()[DestNode].position;
		int NumDestNeigbors=Graphs[DestGraphIndex].Links.Count;
		//Calculate weights/connections for Source Node
		foreach(pathNode node in Graphs[DestGraphIndex].Links){
			//Get distance to each door
			sparseDestNode.neighbors.Add(node);
			node.neighbors.Add(sparseDestNode);
			float dist=Vector3.Distance(sparseDestNode.position,node.position);
			sparseDestNode.weight.Add(dist);
			node.weight.Add(dist);
		}
		//Pathfinding on sparse graph
		//Debug.Log ("Calculating shortest Sparse Graph");
		List<pathNode> sparsePath=Dijkstra.ShortestPath(sparseGraph,0,sparseGraph.GetNodeList().Count-1);
		//Debug.Log ("sparsePath Calculated");
		//Debug.Log ("Path will travel through these transitions");
		//Debug.Log("Start at Graph "+SourceGraphIndex+" Node " + SourceNode);  
		foreach(pathNode node in sparsePath){
			if(node.ID==0){
				//Debug.Log("Node 0 is the starting point at graph node "+SourceNode);
				continue;
			}

			if(node.ID==sparsePath.Count){
				//Debug.Log("Node "+node.ID+" is the ending point at graph node "+DestNode);
				continue;
			}

			//Debug.Log ("Node: "+node.ID);
		}

		//Debug.Log("End at Graph "+DestGraphIndex+" Node " + DestNode);

		//Clean up so that there are no leftover neighbors/weights
		foreach(pathNode node in Graphs[SourceGraphIndex].Links){
			node.neighbors.RemoveAt(node.neighbors.Count-1);
			node.weight.RemoveAt(node.weight.Count-1);
		}

		foreach(pathNode node in Graphs[DestGraphIndex].Links){
			node.neighbors.RemoveAt(node.neighbors.Count-1);
			node.weight.RemoveAt(node.weight.Count-1);
		}

		GraphSimple SourceGraph=Graphs[SourceGraphIndex];
		GraphSimple DestGraph=Graphs[DestGraphIndex];

		int LinkIndex=SourceGraph.Links.IndexOf(sparsePath[1]);
		//SUCCESS!
		//Debug.Log ("Moving from "+SourceNode+" to "+SourceGraph.TransitionNodes[LinkIndex]);
		//Begin search with path toward door labeled by sparsePath #1
		shortestPath.AddRange(Dijkstra.ShortestPath(SourceGraph,SourceNode,SourceGraph.TransitionNodes[LinkIndex]));
		//Debug.Log ("Initial Path to transition Node: ");
		foreach(pathNode node in shortestPath)
		{
			//Debug.Log("Node "+node.ID);
		}
		//Debug.Log ("End first segments");

		GraphSimple CurrentGraph=SourceGraph.NeighborGraphs[LinkIndex];
		int Index=0;
		for(int i=1;i<sparsePath.Count-2;i++)
		{
			Index=shortestPath.Count;
			//Debug.Log ("In Graph # "+CurrentGraph.GraphID+" :");
			//Debug.Log ("New Segment from transition " +sparsePath[i].ID+ " to "+sparsePath[i+1].ID);
			//int StartIndex=CurrentGraph.Links.IndexOf(sparsePath[i]);
			int EndIndex=CurrentGraph.Links.IndexOf(sparsePath[i+1]);
			//Remove the last node so that there isn't doubled up vector3s during transition
			//Results in smoother transtions
			shortestPath.RemoveAt(shortestPath.Count-1);
			//shortestPath.AddRange(Dijkstra.ShortestPath(CurrentGraph,CurrentGraph.TransitionNodes[StartIndex],SourceGraph.TransitionNodes[EndIndex]));
			//Debug.Log(
			shortestPath.AddRange(CurrentGraph.GetBakedPath(sparsePath[i].ID,sparsePath[i+1].ID));
			foreach(pathNode node in shortestPath.GetRange(Index,shortestPath.Count-Index))
			{
				//Debug.Log("Node "+node.ID);
			}
			CurrentGraph=CurrentGraph.NeighborGraphs[EndIndex];
		}
		shortestPath.RemoveAt(shortestPath.Count-1);
		LinkIndex=DestGraph.Links.IndexOf(sparsePath[sparsePath.Count-2]);
		//Debug.Log ("Moving from "+DestGraph.TransitionNodes[LinkIndex]+" to "+DestNode);
		//End search with path toward point labeled by sparsePath #1
		shortestPath.AddRange(Dijkstra.ShortestPath(DestGraph,DestGraph.TransitionNodes[LinkIndex],DestNode));
		sparseGraph.DeleteNode(sparseGraph.NodeCount-1);
		return shortestPath;
	}
}
