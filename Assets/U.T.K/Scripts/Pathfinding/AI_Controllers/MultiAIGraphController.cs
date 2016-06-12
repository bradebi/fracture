using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiAIGraphController : MonoBehaviour {

	//TODO Should get graph from another location
	//TODO Data structure script with editor
	//TODO Take different method of indexing than assuming IDs correspond to positions in graph. ##May not be correct assumption## Simplification through references.
	
	//Meant to provide an interface for movement along a graph which is indicated as "NodePath"
	private MultiGraph NodePath;
	//private MultiGraph NodePath2;
	public GameObject GraphContainer;
	public int currentGraph=0, destinationGraph=0;
	public int currentNode=0, destinationNode=0;
	public int smoothing = 7;
	public float speed = 5;
	private bool traveling=false;
	public int patrolStart=0, patrolEnd=0;
	public float waitTime=0;
	public enum MoveState{Manual,RandomWalk,Patrol,Halt,Teleport};
	public MoveState SimpleAIState=MoveState.RandomWalk;
	// Use this for initialization
	void Start () {
		
	}

	bool start=true;
	
	void Update(){
		
		if(start){
			if(SimpleAIState==MoveState.RandomWalk){
				currentGraph=Random.Range(0,NodePath.Graphs.Count);
				currentNode=Random.Range(0,NodePath.Graphs[currentGraph].NodeCount);
				destinationGraph=currentGraph;
				destinationNode=currentNode;
			}
			TeleportToNode(currentGraph,currentNode);
			start=false;
		}
		
		
		if(currentGraph==destinationGraph&&currentNode==destinationNode&&SimpleAIState!=MoveState.Halt){
			if(SimpleAIState==MoveState.RandomWalk){
				int RandomGraph=Random.Range(0,NodePath.Graphs.Count-1);
				int RandomNode = Random.Range(0,NodePath.Graphs[RandomGraph].NodeCount-1);
				TravelToNode(RandomGraph,RandomNode,speed);
				
			}
			else if(currentNode!=destinationNode&&!traveling)
			{
				if(SimpleAIState==MoveState.Teleport){
					TeleportToNode(destinationGraph,destinationNode);
					destinationNode=currentNode;
					destinationGraph=currentGraph;
				}
				else if(SimpleAIState==MoveState.Manual)
					TravelToNode(destinationGraph,destinationNode,speed);
			}
		}
	}
	
	public void ChangeGraph(MultiGraph graph)
	{
		NodePath=graph;
	}
	
	public void TeleportToNode(int graphID,int nodeID)
	{
		transform.position=NodePath.Graphs[graphID].GetNodeList()[nodeID].position;
		currentGraph=graphID;
		currentNode=nodeID;
	}
	
	//Travel to Node
	public void TravelToNode(int GraphID,int NodeID, float time){
		destinationNode = NodeID;
		destinationGraph = GraphID;
		traveling=true;
		if(time<=0){
			TeleportToNode(GraphID,NodeID);
			currentGraph=destinationGraph;
			currentNode=destinationNode;
		}
		else
			StartCoroutine(PathTravel(GraphID,NodeID,time));
	}
	
	//TODO Change time to a constant speed based on rough estimations of distance.
	//TODO Add an interrupt variable so that the AI can leave the path or change paths. Maybe check to see if destination changes.
	IEnumerator PathTravel(int destGraphID,int destNodeID, float speed){
		Debug.Log ("Drawing path");
		List<pathNode> path = NodePath.ShortestPath(currentGraph,currentNode,destGraphID,destNodeID);
		if(path.Count>1)
		{
			//Add initial location twice to initialize spline cap
			path.Insert(0,path[0]);
			//Add destination twice for the end cap on spline
			path.Add (path[path.Count-1]);
			
			float timer =0;
			for(int i=0;i<path.Count-3;i++){
				timer =0;
				pathNode nextNode = path[i+2];
				float dist = Spline_Catmull_Rom.LengthofCurve(path[i].position,path[i+1].position,path[i+2].position,path[i+3].position,5);
				float time=dist/speed;
				while(Vector3.Distance(transform.position,nextNode.position)>Time.deltaTime)
				{
					timer+=Time.deltaTime;
					if(timer>time)
						break;
					transform.position = Spline_Catmull_Rom.PointOnCurve(path[i].position,path[i+1].position,path[i+2].position,path[i+3].position, timer/time);
					transform.LookAt(Spline_Catmull_Rom.PointOnCurve(path[i].position,path[i+1].position,path[i+2].position,path[i+3].position, (timer+Time.deltaTime)/time));
					yield return null;
				}
			}
			currentNode= destNodeID;
			currentGraph=destGraphID;
			traveling=false;
		}
	}
	
	private List<Vector3> SplinePts= new List<Vector3>();
	private List<Vector3> PathPts= new List<Vector3>();
	private int tempCurr=-1;

	void OnDrawGizmos(){
		if(NodePath!=null){
			if(NodePath.Graphs[0].GetNodeList().Count>0){
				if(currentNode!=tempCurr&&currentNode!=destinationNode){
					//Debug.Log ("Drawing spline between "+currentNode+" and "+destinationNode);
					List<pathNode> path = NodePath.ShortestPath(currentGraph,currentNode,destinationGraph,destinationNode);
					if(path.Count>0){
						path.Insert(0,path[0]);
						path.Add(path[path.Count-1]);
						PathPts.Clear();
						for(int i=0;i<path.Count;i++)
							PathPts.Add(path[i].position);
						
						SplinePts.Clear();
						if(PathPts.Count>0){
							tempCurr=currentNode;
							SplinePts=Spline_Catmull_Rom.GetPoints(PathPts.ToArray(),smoothing);
						}
					}
				}
				
				for(int i=0;i<SplinePts.Count-1;i++){
					Debug.DrawLine(SplinePts[i], SplinePts[i+1], Color.green, 0, false);
				}
				/*
			for(int i=0;i<NodePath.NodeCount;i++){
				Gizmos.color=Color.blue;
				Gizmos.DrawSphere(NodePath.GetNodeList()[i].position,0.1f);
			}
*/
				
			}
		}
	}

}
