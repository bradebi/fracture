using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//TODO Should get graph from another location
//TODO Data structure script with editor
//TODO Take different method of indexing than assuming IDs correspond to positions in graph. ##May not be correct assumption## Simplification through references.

//Meant to provide an interface for movement along a graph which is indicated as "NodePath"
public class SimpleAIGraphController : MonoBehaviour {
	private GraphSimple NodePath;
	public GameObject GraphContainer;
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
		if (NodePath==null){
			(GraphContainer.GetComponent(typeof(SimpleAIGraphContainer)) as SimpleAIGraphContainer).RefreshReferences();
			NodePath=(GraphContainer.GetComponent(typeof(SimpleAIGraphContainer)) as SimpleAIGraphContainer).GetGraph();
		}
		if(SimpleAIState==MoveState.RandomWalk){
			currentNode=Random.Range(0,NodePath.NodeCount);
			destinationNode=currentNode;
		}
		TeleportToNode(currentNode);
		arrow=Resources.Load<GameObject>("Prefabs/Arrows/Arrow_Simple");
			start=false;

	}
		
		
		if(currentNode==destinationNode&&SimpleAIState!=MoveState.Halt){
			if(SimpleAIState==MoveState.RandomWalk)
				TravelToNode(Random.Range(0,NodePath.NodeCount),speed);
			else if(SimpleAIState==MoveState.Patrol){
				if(currentNode==patrolStart)
					TravelToNode(patrolEnd,speed);
				else
					TravelToNode(patrolStart,speed);

		}
		else if(currentNode!=destinationNode&&!traveling)
		{
			if(SimpleAIState==MoveState.Teleport){
				TeleportToNode(destinationNode);
				destinationNode=currentNode;
			}
			else if(SimpleAIState==MoveState.Manual)
				TravelToNode(destinationNode,speed);
			}
		}
	}

	public void ChangeGraph(GraphSimple graph)
	{
		NodePath=graph;
	}

	public void TeleportToNode(int ID)
	{
		transform.position=NodePath.GetNodeList()[currentNode].position;
		currentNode=ID;
	}
	private GameObject arrow;
	//Points in the direction of the path toward ID X
	public GameObject GetDirectionalArrow(int ID)
	{
		List<pathNode> path = Dijkstra.ShortestPath(NodePath,currentNode,ID);
		GameObject tempObj = Instantiate(arrow) as GameObject;
		//Add initial location twice to initialize spline cap
		path.Insert(0,path[0]);
		//Add destination twice for the end cap on spline
		path.Add (path[path.Count-1]);
		tempObj.transform.position=Spline_Catmull_Rom.PointOnCurve(path[0].position,path[1].position,path[2].position,path[3].position, 0.3f);
		tempObj.transform.LookAt(Spline_Catmull_Rom.PointOnCurve(path[0].position,path[1].position,path[2].position,path[3].position, 0.5f));
		return tempObj;
	}

	//Get node travel options
	public List<int> GetTravelOptions(int ID){
		List<int> IDs = new List<int>();
		List<pathNode> DeadNodes = new List<pathNode>();
		//Initialize neighbors
		DeadNodes.AddRange(NodePath.GetNeighborsNodes(ID));
		for(int i=0;i<NodePath.NodeCount;i++)
			NodePath.GetNodeList()[i].Visited=false;

		for(int i=0;i<DeadNodes.Count;i++)
		{
			if(DeadNodes[i].Visited){
				DeadNodes.RemoveAt(i);
				i--;
				continue;
			}
			else
				DeadNodes[i].Visited=true;

			if(DeadNodes[i].passThrough){
				DeadNodes.AddRange(NodePath.GetNeighborsNodes(DeadNodes[i].ID));
				DeadNodes.RemoveAt(i);
				i--;
			}
			else if(DeadNodes[i].ID!=ID)
				IDs.Add(DeadNodes[i].ID);
		}
		return IDs;
	}

	//Travel to Node
	public void TravelToNode(int ID, float time){
		destinationNode = ID;
		traveling=true;
		if(time<=0){
			TeleportToNode(ID); 
			currentNode=destinationNode;
		}
		else
			StartCoroutine(PathTravel(NodePath.GetNodeList()[currentNode],NodePath.GetNodeList()[ID],time));
	}

	//TODO Change time to a constant speed based on rough estimations of distance.
	//TODO Add an interrupt variable so that the AI can leave the path or change paths. Maybe check to see if destination changes.
	IEnumerator PathTravel(pathNode location, pathNode destination, float speed){

		List<pathNode> path = Dijkstra.ShortestPath(NodePath,location.ID,destination.ID);
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
			currentNode= destination.ID;
			traveling=false;
		}
	}

	private List<Vector3> SplinePts= new List<Vector3>();
	private List<Vector3> PathPts= new List<Vector3>();
	private int tempCurr=-1;
	void OnDrawGizmos(){
		if(NodePath!=null){
			if(NodePath.GetNodeList().Count>0){
				if(currentNode!=tempCurr&&currentNode!=destinationNode){
					//Debug.Log ("Drawing spline between "+currentNode+" and "+destinationNode);
					List<pathNode> path = Dijkstra.ShortestPath(NodePath,currentNode,destinationNode);
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
