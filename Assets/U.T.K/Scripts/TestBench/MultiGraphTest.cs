using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MultiGraphTest : MonoBehaviour {
	List<int> Segments=new List<int>();
	GraphParam [] Parameters;
	MultiGraph GraphManager = new MultiGraph();
	public GameObject AI;
	public int AICount=0;
		// Use this for initialization
	void Start () {

		//Get all Gameobjects
		Parameters = gameObject.GetComponentsInChildren<GraphParam>();
		BuildGraphs();
		BuildConnections();
		//GraphManager.ConnectGraphs(0,7,1,100);
		//GraphManager.ConnectGraphs(1,10,2,0);
		//GraphManager.ConnectGraphs(1,50,3,100);
		//GraphManager.ConnectGraphs(2,50,3,35);
		GraphManager.BakeGraphs();

		foreach(pathNode node in GraphManager.sparseGraph.GetNodeList()){
			Debug.Log("Node "+node.ID+" at "+node.position);
			int index=0;
			foreach(pathNode neighbor in node.neighbors){
				Debug.Log ("Neighbor "+neighbor.ID+" at a distance of "+node.weight[index++]);
			}
		}

		//foreach(pathNode node in GraphManager.ShortestPath(0,10,1,3))
		//{
		//	Debug.Log(node.ID);
		//}

		//foreach(pathNode node in GraphManager.ShortestPath(1,10,1,3))
		//{
		//	Debug.Log(node.ID);
		//}
		for(int i=0;i<AICount;i++){
			GameObject tempObj = Instantiate(AI) as GameObject;
			(tempObj.GetComponent(typeof(MultiAIGraphController)) as MultiAIGraphController).ChangeGraph(GraphManager);
		}
		//
	}

	GraphMesh GraphA;
	GraphMesh GraphB;
	
	int GraphIDA;
	int GraphIDB;
	
	pathNode NodeA;
	pathNode NodeB;

	RaycastHit hit;
	Ray ray;

	string GraphAName;
	string GraphBName;

	void BuildConnections()
	{
		List<GameObject> Doors = new List<GameObject>();

		foreach (Transform child in transform)
		{
			if(child.name.Contains("Door"))
				Doors.Add(child.gameObject);
		}


		//Raycast twice to get graphs on each side
		foreach(GameObject door in Doors)
		{
			if(!door.activeSelf)
				continue;

			float width = door.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.size.x;
			float height = door.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.size.z;

			GraphA=null;
			GraphB=null;

			ray.direction=Vector3.down;
			if(width>height)
				ray.origin=door.transform.position+new Vector3(0,0,height/2);
			else
				ray.origin=door.transform.position+new Vector3(width/2,0,0);

			if(Physics.Raycast(ray,out hit)){
				GraphA=hit.collider.gameObject.GetComponent<GraphMesh>();
				GraphIDA=GraphA.Graph.GraphID;
				NodeA = GraphA.Graph.GetClosestNode(hit.point);
				GraphAName=hit.collider.name;
			}

			if(width>height)
				ray.origin=door.transform.position+new Vector3(0,0,-height/2);
			else
				ray.origin=door.transform.position+new Vector3(-width/2,0,0);

			if(Physics.Raycast(ray,out hit)){
				GraphB=hit.collider.gameObject.GetComponent<GraphMesh>();
				GraphIDB=GraphB.Graph.GraphID;
				NodeB = GraphB.Graph.GetClosestNode(hit.point);
				GraphBName=hit.collider.name;
			}

			if(GraphA!=null&&GraphB!=null)
			{
				Debug.Log ("Connecting Graph "+GraphAName+" at node "+NodeA.ID+" with "+GraphBName+" at node "+NodeB.ID);
				GraphManager.ConnectGraphs(GraphIDA,NodeA.ID,GraphIDB,NodeB.ID);
			}
		}

	}

	void BuildGraphs()
	{
		//Get width/Height of plane it is attached to.
		for(int i=0;i<Parameters.Length;i++)
		{
			float width=Parameters[i].GetComponent<MeshFilter>().mesh.bounds.size.x;
			float height=Parameters[i].GetComponent<MeshFilter>().mesh.bounds.size.z;
			//Debug.Log ("Width= "+width+" Height= "+height);
			//Debug.Log(Parameters[i].name);
			Destroy(Parameters[i].GetComponent("MeshCollider"));
			//MeshRenderer renderer = Parameters[i].GetComponent("MeshRenderer") as MeshRenderer;
			GraphMesh Graph=Parameters[i].gameObject.AddComponent<GraphMesh>() as GraphMesh;
			Graph.BuildPlaneGraph(width,height,Parameters[i].Segments);
			GraphManager.AddGraph(Graph.Graph);
			Parameters[i].GraphID=Graph.Graph.GraphID;
			Destroy(Parameters[i].GetComponent<MeshFilter>().mesh);
			//Destroy(Parameters[i].GetComponent("MeshCollider"));
			Parameters[i].gameObject.AddComponent<MeshCollider>();
		}
	}

	void BuildSparseGraph()
	{

	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "stepfinal2.png");
		if(GraphManager.sparseGraph==null){
			return;
		}

		if(GraphManager.sparseGraph.NodeCount>1){
			for(int j=0;j<GraphManager.sparseGraph.NodeCount;j++){
				for(int i=0;i<GraphManager.sparseGraph.graph[j].neighbors.Count;i++){
					Debug.DrawLine(GraphManager.sparseGraph.graph[j].position,GraphManager.sparseGraph.graph[j].neighbors[i].position,Color.red,0);
				}
				Gizmos.DrawCube(GraphManager.sparseGraph.graph[j].position,new Vector3(0.1f,0.1f,0.1f));
			}
		}
	}
}
