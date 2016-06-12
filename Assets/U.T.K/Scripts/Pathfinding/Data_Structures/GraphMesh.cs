using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GraphMesh : MonoBehaviour {
	private Mesh meshGraph;
	public float width,height;
	private GraphSimple simpleGraph;
	//Vector3[] vertices = Graph.vertices;
	
	GameObject Sampler;
	//Not meant for real-time use
	public void ConstructGraph()
	{
		if(meshGraph==null){
			Debug.Log ("No mesh to operate on! Please set meshGraph before calling this function");
			return;
		}
		//Create a sampler Gameobject to perform raycasts to neighboring nodes
		Sampler=new GameObject("Sampler");
		List<int> neighbors;
		List<int> DeadVertices=new List<int>();
		List<pathNode> DeadNodes=new List<pathNode>();
		simpleGraph=new GraphSimple();
		simpleGraph.AddNodes(meshGraph.vertexCount);

		float dist=0;

		for(int i=0;i<meshGraph.vertexCount;i++)
		{
			//if(DeadVertices.Contains(i))
				//continue;

			neighbors=GetNeighbors(i);

			pathNode currentNode=simpleGraph.graph[i];
			currentNode.position=transform.TransformPoint(meshGraph.vertices[i]);


			for(int j=0;j<neighbors.Count;j++)
			{
				dist=Vector3.Distance(transform.TransformPoint(meshGraph.vertices[i]),transform.TransformPoint(meshGraph.vertices[neighbors[j]]));
				currentNode.neighbors.Add(simpleGraph.graph[neighbors[j]]);
				currentNode.weight.Add(dist);
				Sampler.transform.position=transform.TransformPoint(meshGraph.vertices[i]);
				if (Physics.Raycast(Sampler.transform.position, Vector3.Normalize(meshGraph.vertices[neighbors[j]]-meshGraph.vertices[i]), dist)){
					if(!DeadVertices.Contains(neighbors[j])) {DeadVertices.Add(neighbors[j]); DeadNodes.Add (simpleGraph.graph[neighbors[j]]); break;}
				}
			}
			//Debug.Log(i);
		}

		foreach(pathNode node in DeadNodes)
			simpleGraph.DeleteNode(node.ID);

		//Rebuild mesh
		NavMeshGenerator.RemoveTriangles(meshGraph,DeadVertices);

	}

	public GraphSimple Graph
	{
		
		get {return simpleGraph; }
		
		set {simpleGraph = value; }
		
	}
	//Faster Method
	public void BuildPlaneGraph(float width, float height, int segments)
	{
		this.width=width;
		this.height=height;
		simpleGraph= new GraphSimple();
		List<int> Neighbors = new List<int>();
		List<float> Weights = new List<float>();
		float deltaHeight=height/segments;
		float deltaWidth=width/segments;
		//transform.TransformPoint(new Vector3(j*(width/segments)-width/2, 0, i*(height/segments)-height/2));
		simpleGraph.AddNodes((segments+1)*(segments+1));
		List<pathNode> DeadNodes=new List<pathNode>();
		List<int> DeadVerts=new List<int>();

		pathNode currentNode;
		for(int i=0;i<=segments;i++){
			for(int j=0;j<=segments;j++){
				currentNode=simpleGraph.GetNodeList()[i*(segments+1)+j];

				currentNode.position = transform.TransformPoint(new Vector3(j*(deltaWidth)-width/2, 0, i*(deltaHeight)-height/2));
				//RaycastHit hit;
				//if (Physics.Raycast(currentNode.position, -Vector3.up, out hit))
				//{
				//	currentNode.position=transform.TransformPoint(new Vector3(j*(deltaWidth)-width/2, hit.point.y, i*(deltaHeight)-height/2));
				//}

				Neighbors.Clear();
				Weights.Clear();

				//Debug.Log ("Node #"+(i*(segments+1)+j));

				//Up and Down
				if(i>0){
					Neighbors.Add(((i-1)*(segments+1)+(j)));
					Weights.Add (deltaHeight);
					//Debug.Log ("Down");
				}
				if(i<segments){
					Neighbors.Add(((i+1)*(segments+1)+(j)));
					Weights.Add (deltaHeight);
					//Debug.Log ("Up");
				}

				//Left and Right
				if(j>0){
					Neighbors.Add(((i)*(segments+1)+(j-1)));
					Weights.Add (deltaWidth);
					//Debug.Log ("Left");
				}
				if(j<segments){
					Neighbors.Add(((i)*(segments+1)+(j+1)));
					Weights.Add (deltaWidth);
					//Debug.Log ("Right");
				}
				//Top-left to Bottom-right Diagonal
				if(i>0&&j>0)
				{
					Neighbors.Add((i-1)*(segments+1)+(j-1));
					Weights.Add (Mathf.Sqrt(deltaWidth*deltaWidth+deltaHeight*deltaHeight));
					//Debug.Log ("Bottom Left");
				}
				if(i<segments&&j<segments){
					Neighbors.Add((i+1)*(segments+1)+(j+1));
					Weights.Add (Mathf.Sqrt(deltaWidth*deltaWidth+deltaHeight*deltaHeight));
					//Debug.Log ("Top right");
				}

				//Top-right to Bottom-left Diagonal
				if((i<segments)&&j>0){
					Neighbors.Add(((i+1)*(segments+1)+(j-1)));
					Weights.Add (Mathf.Sqrt(deltaWidth*deltaWidth+deltaHeight*deltaHeight));
					//Debug.Log ("Top left");
				}
				if(i>0&&j<segments){
					Neighbors.Add((i-1)*(segments+1)+(j+1));
					Weights.Add (Mathf.Sqrt(deltaWidth*deltaWidth+deltaHeight*deltaHeight));
					//Debug.Log ("Bottom Right");
				}

				int index=0;
				foreach(int Neighbor in Neighbors){
					//Debug.Log (Neighbor);
					pathNode neighbor=simpleGraph.GetNodeList()[Neighbor];
					currentNode.neighbors.Add(neighbor);
					currentNode.weight.Add (Weights[index]);
					index++;
				}
				//Debug.Log(i);
			}

		}

		foreach(pathNode node in simpleGraph.GetNodeList()){
			node.dist=0;

		}

		//Scan physics graph
		foreach(pathNode node in simpleGraph.GetNodeList()){
			//Debug.Log (node.neighbors.Count);
			for(int i=0; i<node.neighbors.Count;i++){
				pathNode Neighbor = node.neighbors[i];
				//Debug.Log ("Ray cast from "+node.position+" to "+Neighbor.position+" with a distance of "+Vector3.Distance(Neighbor.position,node.position));
				//Debug.Log ("Node #"+node.ID+" Hit toward: "+Neighbor.ID);
				if (Physics.Raycast(node.position, Vector3.Normalize(Neighbor.position-node.position), Vector3.Distance(Neighbor.position,node.position))){
					//Debug.DrawLine(Neighbor.position,node.position);
					if(!Physics.Raycast(node.position, Vector3.Normalize(node.position-Neighbor.position), Vector3.Distance(Neighbor.position,node.position))){
						if(DeadVerts.Contains(Neighbor.ID)){DeadNodes.Add(Neighbor);DeadVerts.Add(Neighbor.ID);}}
					//removeCount++;
					node.neighbors.RemoveAt(i);
					node.weight.RemoveAt(i);
					i--;
					Neighbor.dist+=1;

				}
			}
		}

		//Dead Node Removal
		for(int i=0; i<DeadNodes.Count;i++){
			//Debug.Log (DeadNodes[i]);
			simpleGraph.DeleteNode(DeadNodes[i].ID);
		}
		

		//Filter nodes Remove count
		for(int i=0; i<simpleGraph.NodeCount;i++){

			pathNode node = simpleGraph.GetNodeList()[i];
			//Debug.Log("Hit count: "+node.dist);
			if(node.dist>=3){
				simpleGraph.DeleteNode(node.ID);
				i--;
			}
		}

		for(int i=0; i<simpleGraph.NodeCount;i++){
			pathNode node = simpleGraph.GetNodeList()[i];
			if(node.neighbors.Count<3)
				simpleGraph.DeleteNode(node.ID);
		}

	}

	/*
	//Hard code for planes for the moment
	public List<int> GetNeighborsPlane(int index){
		bool isEven;
		bool isBorderRight;
		bool isBorderLeft;
		bool isBorderTop;
		bool isBorderBottom;

		if((index+1)%2==0)
			isEven=true;

		if((index+1)%(segments+1)!=0)




	}
*/
	//NOTE ***Extremely expensive operation***
	public List<int> GetNeighbors(int index){

		bool found;

		int cur=0;

		List<int> verts = new List<int>();
		
		for(int i=0; i<meshGraph.triangles.Length / 3; i++){
			
			// see if the triangle contains the index	
			found = false;	
			for(int j=0; j<3; j++){
				
				cur = meshGraph.triangles[i * 3 + j];
				
				if(cur == index) found = true;
				
			}
			
			// if we found the index in the triangle, append the others.
			if(found){
				for(int j=0; j<3; j++){
					
					cur = meshGraph.triangles[i * 3 + j];
					
					if(verts.IndexOf(cur) == -1 && cur != index)
						verts.Add(cur);
				}
			}
		}
		return verts;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "stepfinal2.png");
		if(simpleGraph==null)
			return;

		if(simpleGraph.NodeCount>1){
			for(int j=0;j<simpleGraph.NodeCount;j++){
				for(int i=0;i<simpleGraph.graph[j].neighbors.Count;i++){
					Debug.DrawLine(simpleGraph.graph[j].position,simpleGraph.graph[j].neighbors[i].position,Color.blue,0);
				}
			}
		}
	}
}
