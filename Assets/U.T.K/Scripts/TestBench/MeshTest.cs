using UnityEngine;
using System.Collections;

public class MeshTest : MonoBehaviour {
	public float width=0;
	public float height=0;
	public int segments=0;
	public Material GraphMat;
	public GameObject AI;
	public int AICount=0;
	GraphMesh Graph;
	// Use this for initialization
	void Start () {

		width=gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x;
		height=gameObject.GetComponent<MeshFilter>().mesh.bounds.size.z;

	
		//gameObject.AddComponent("MeshFilter");
		Destroy(gameObject.GetComponent("MeshCollider"));
		MeshRenderer renderer = gameObject.GetComponent("MeshRenderer") as MeshRenderer;
		Graph=gameObject.AddComponent<GraphMesh>() as GraphMesh;
		//float timer =Time.timeSinceLevelLoad;
		//Debug.Log ("Getting Plane: "+timer);
		//gameObject.GetComponent<MeshFilter>().mesh=NavMeshGenerator.CreatePlane(width,height,segments);
		//Debug.Log ("Plane Built: "+(Time.realtimeSinceStartup-timer));
		//timer =Time.realtimeSinceStartup;

		//Graph.meshGraph=gameObject.GetComponent<MeshFilter>().mesh;
		renderer.material=GraphMat;
		//gameObject.transform.localScale=new Vector3(2,2,2);
		//Debug.Log ("Constructing new Plane: "+timer);

		Graph.BuildPlaneGraph(width,height,segments);
		Destroy(gameObject.GetComponent<MeshFilter>().mesh);
		//Debug.Log ("Plane Built: "+(Time.realtimeSinceStartup-timer));
		for(int i=0;i<AICount;i++){
			GameObject tempObj = Instantiate(AI) as GameObject;
			(tempObj.GetComponent(typeof(SimpleAIGraphController)) as SimpleAIGraphController).ChangeGraph(Graph.Graph);
		}

	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "stepfinal2.png");
		if(Graph==null)
			return;
		if(Graph.Graph==null)
			return;
		if(Graph.Graph.NodeCount>1){
			for(int j=0;j<Graph.Graph.NodeCount;j++){
				for(int i=0;i<Graph.Graph.graph[j].neighbors.Count;i++){
					Debug.DrawLine(Graph.Graph.graph[j].position,Graph.Graph.graph[j].neighbors[i].position,Color.blue,0);
				}
			}
		}
	}
}
