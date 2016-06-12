using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Pathfinding_Test_Dijkstra : MonoBehaviour {
	GraphSimple testGraph=new GraphSimple();
	Camera cam;
	// Use this for initialization
	void Start () {
		/*
		testGraph.AddNodes(1);
		testGraph.AddNodes(1);
		testGraph.AddNodes(1);
		testGraph.AddNodes(1);
		testGraph.AddNodes(1);
		testGraph.ConnectNodes(0,1,50);
		//testGraph.ConnectNodes(1,2,100);
		testGraph.ConnectNodes(1,2,30);

		foreach(pathNode node in Dijkstra.ShortestPath(testGraph,0,3))
		{
			Debug.Log(node.ID);
		}

		foreach(float dist in Dijkstra.AllDistancesFromNode(testGraph,0))
		{
			Debug.Log(dist);
		}
		testGraph.ConnectNodes(2,3,30);
		testGraph.ConnectNodes(3,4,30);
		foreach(float dist in Dijkstra.AllDistancesFromNode(testGraph,3))
		{
			Debug.Log(dist);
		}

		foreach(float dist in Dijkstra.AllDistancesFromNode(testGraph,4))
		{
			Debug.Log(dist);
		}
		GetDistances();
*/
	}

	bool PlaceNodes,ConnectNodes;
	List<GameObject> Nodes = new List<GameObject>();
	List<GameObject> Lines = new List<GameObject>();
	public List<GameObject> Text = new List<GameObject>();
	public List<GameObject> Labels = new List<GameObject>();
	int A,B;
	public Font WeightFont;
	public GameObject Textmesh;

	TextMesh tempMesh;

	int CurrentDistanceNodeIDInt,CurrentPathRootNodeIDInt,CurrentPathEndNodeIDInt;

	void Update () {
		if(CurrentDistanceNodeIDInt!=DistanceNodeIDInt){
			CurrentDistanceNodeIDInt=DistanceNodeIDInt;
			GetDistances();
		}
		if(CurrentPathRootNodeIDInt!=PathRootNodeIDInt||CurrentPathEndNodeIDInt!=PathEndNodeIDInt){
			CurrentPathRootNodeIDInt=PathRootNodeIDInt;
			CurrentPathEndNodeIDInt=PathEndNodeIDInt;
			GetPaths();
		}

		if(cam==null)
		cam=GameObject.FindGameObjectWithTag("MainCamera").GetComponent(typeof(Camera)) as Camera;
		if(!GUIMouseOver ()){
		GetPoint();
		if(PlaceNodes&&Input.GetMouseButtonDown(0)){
			Nodes.Add (GameObject.CreatePrimitive(PrimitiveType.Sphere));
			Nodes[Nodes.Count-1].transform.position=NodePoint;
			Nodes[Nodes.Count-1].name=(Nodes.Count-1).ToString();
			Nodes[Nodes.Count-1].layer=13;
			Labels.Add(Instantiate(Textmesh) as GameObject);
			Labels[Labels.Count-1].transform.position=(NodePoint);
			tempMesh = Labels[Labels.Count-1].GetComponent(typeof(TextMesh)) as TextMesh;
			tempMesh.text=(Labels.Count-1).ToString();
			tempMesh.color=Color.white;
			testGraph.AddNodes(1);
			GetDistances();
		}
		if(ConnectNodes){
			if(Input.GetMouseButtonDown(0)){
				A=IDPoint;
			}

			if(ConnectNodes&&Input.GetMouseButtonUp(0)){
				B=IDPoint;
			}

			if(A==B){
				A=-1;
				B=-1;
			}
		}

		if(A>=0&&B>=0&&B!=A){
			testGraph.ConnectNodes(A,B,weightF);
			bool copy=false;
			int index=0;
			foreach(GameObject obj in Lines){
				if(obj.name.Contains(A+" to "+B+" Connection")||obj.name.Contains(B+" to "+A+" Connection")){
					copy=true;
					break;
				}
				index++;
				copy=false;
			}
			if(copy==false){
				Lines.Add(new GameObject(A+" to "+B+" Connection"));
				Text.Add(Instantiate(Textmesh) as GameObject);
				LineRenderer tempLine = Lines[Lines.Count-1].AddComponent(typeof(LineRenderer)) as LineRenderer;
				tempLine.GetComponent<Renderer>().material=LineBlack;
				tempLine.SetWidth(0.3f,0.3f);
				tempLine.SetPosition(0,Nodes[A].transform.position);
				tempLine.SetPosition(1,Nodes[B].transform.position);

				Text[Text.Count-1].transform.position=(Nodes[A].transform.position+Nodes[B].transform.position)/2;
				
				tempMesh = Text[Text.Count-1].GetComponent(typeof(TextMesh)) as TextMesh;
				tempMesh.text=weightF.ToString();
				tempMesh.color=Color.red;
				GetDistances();
			}
			else
			{
				tempMesh = Text[index].GetComponent(typeof(TextMesh)) as TextMesh;
				tempMesh.text=weightF.ToString();
				GetDistances();
			}
			GetPaths();
			Debug.Log("Connect:"+A+" to "+B);
			A=-1;
			B=-1;
		}
		}
	}

	Texture2D snapshot;
	bool grab;
	
	void LateUpdate ()
	{
		if (grab) {
			RenderTexture rt = new RenderTexture (Screen.width, Screen.height, 24);
			cam.targetTexture = rt;
			Texture2D screenShot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
			cam.Render ();
			RenderTexture.active = rt;
			screenShot.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
			cam.targetTexture = null;
			RenderTexture.active = null; // JC: added to avoid errors
			snapshot = screenShot;
			grab = false;
			SaveUtility.SaveToImageAtDirectory ("C:/Users/Sakaki/Desktop/" + DateTime.Now.ToString().Replace("/","_").Replace(" ","").Replace (":","_"), snapshot);
		}
	}

	RaycastHit hit;
	Ray ray;
	LayerMask layerMask;
	Vector3 NodePoint;
	int IDPoint;
	Vector3 vec;
	void GetPoint()
	{
		if (Screen.lockCursor) {
			vec.x = (float)Screen.width / 2;
			vec.y = (float)Screen.height / 2;
			vec.z = 0;
		} else
			vec = Input.mousePosition;
		
		ray = cam.ScreenPointToRay (vec);
		
		layerMask = 1 << 12;
		
		if (Physics.Raycast (ray, out hit, 1000.0f, 1 << 12)){
			NodePoint = new Vector3 (hit.point.x, hit.point.y, hit.point.z-0.1f);
			IDPoint=-1;
		}
		if(Physics.Raycast (ray, out hit, 1000.0f, 1 << 13)){
			int.TryParse(hit.transform.gameObject.name,out IDPoint);
			//Debug.Log(IDPoint);
		}

	}

	bool GUIMouseOver ()
	{
			
		if (new Rect(0,0,Screen.width/10,Screen.height).Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
			return true;

		if (new Rect(Screen.width-Screen.width/10,0,Screen.width/10,Screen.height).Contains (new Vector2 (Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
			return true;


		return false;
	}


	string DistanceRootNodeID="0";
	int DistanceNodeIDInt=0;

	string PathRootNodeID="0";
	int PathRootNodeIDInt=0;

	string PathEndNodeID="0";
	int PathEndNodeIDInt=0;

	string weight="0";
	float weightF=0;

	void OnGUI() {
		GUI.Box(new Rect(0,0,Screen.width/10,Screen.height),"");
		GUILayout.BeginArea(new Rect(0,0,Screen.width/10,Screen.height));
		GUILayout.Label("Graph Generation:");
		if(GUILayout.Button("Place Node")){
			PlaceNodes=true;
			ConnectNodes=false;
		}

		if(GUILayout.Button ("Connect Nodes")){
			PlaceNodes=false;
			ConnectNodes=true;
		}

		weight=GUILayout.TextField(weight);
		float.TryParse (weight, out weightF);

		GUILayout.Label("Graph Calculations: ");

		if(GUILayout.Button ("Calculate Distances"))
			GetDistances();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Root Node:");
		DistanceRootNodeID=GUILayout.TextField(DistanceRootNodeID);
		GUILayout.EndHorizontal();
		int.TryParse (DistanceRootNodeID, out DistanceNodeIDInt);
	
		if(DistanceNodeIDInt>=testGraph.NodeCount)
			DistanceRootNodeID=(testGraph.NodeCount-1).ToString();
		if(DistanceNodeIDInt<0)
			DistanceRootNodeID=(0).ToString();
			
		if(GUILayout.Button ("Calculate Shortest Path"))
		{
			GetPaths();
		}

		GUILayout.BeginHorizontal();
		GUILayout.Label("Root Node:");
		PathRootNodeID=GUILayout.TextField(PathRootNodeID);
		GUILayout.EndHorizontal();
		int.TryParse (PathRootNodeID, out PathRootNodeIDInt);

		if(PathRootNodeIDInt>=testGraph.NodeCount)
			PathRootNodeID=(testGraph.NodeCount-1).ToString();
		if(PathRootNodeIDInt<0)
			PathRootNodeID=(0).ToString();

		GUILayout.BeginHorizontal();
		GUILayout.Label("End Node:");
		PathEndNodeID=GUILayout.TextField(PathEndNodeID);
		GUILayout.EndHorizontal();
		int.TryParse (PathEndNodeID, out PathEndNodeIDInt);

		if(PathEndNodeIDInt>=testGraph.NodeCount)
			PathEndNodeID=(testGraph.NodeCount-1).ToString();
		if(PathEndNodeIDInt<0)
			PathEndNodeID=(0).ToString();

		if(GUILayout.Button("Screenshot")){
			Application.CaptureScreenshot("C:/Users/Sakaki/Desktop/" +DateTime.Now.ToString().Replace("/","_").Replace(" ","").Replace (":","_")+".png");
		}
		if(GUILayout.Button("Exit"))
			Application.Quit();



		GUILayout.EndArea();

		GUI.Box(new Rect(Screen.width-Screen.width/10,0,Screen.width/10,Screen.height),"");
		GUILayout.BeginArea(new Rect(Screen.width-Screen.width/10,0,Screen.width/10,Screen.height));
		GUILayout.Label("Origin: Node "+DistanceNodeIDInt);
		if(distances.Count==testGraph.NodeCount){
			for(int i=0;i<testGraph.NodeCount;i++)
			{
				GUILayout.Label("Distance to "+i+" : "+distances[i]);
			}
		}

		GUILayout.Label("Shortest Path from Node "+PathRootNodeIDInt+" to "+PathEndNodeIDInt);
		if(shortestPath.Count>0){
			for(int i=1;i<shortestPath.Count;i++)
			{
				GUILayout.Label(shortestPath[i-1]+" -> "+shortestPath[i]);
			}
		}
		GUILayout.EndArea();
	}

	List<float> distances=new List<float>();
	void GetDistances()
	{
		distances.Clear();
		distances=Dijkstra.AllDistancesFromNode(testGraph,DistanceNodeIDInt);
		int index=0;

		foreach(pathNode node in testGraph.GetNodeList())
		{
			//Debug.Log(node.dist);
			foreach(pathNode neighbor in node.neighbors)
				Debug.Log ("Node "+index+" connected to:"+neighbor.ID);

			index++;
		}

	}
	List<int> shortestPath=new List<int>();
	void GetPaths()
	{
		shortestPath.Clear();
		foreach(pathNode node in Dijkstra.ShortestPath(testGraph,PathRootNodeIDInt,PathEndNodeIDInt))
			shortestPath.Add(node.ID);
		if(shortestPath.Count>0)
			ChangeLineColor();
	}
	public Material LineBlack,LineGreen;
	void ChangeLineColor()
	{
		for(int i=0;i<Lines.Count;i++)
		{
			LineRenderer tempLine=Lines[i].GetComponent(typeof(LineRenderer)) as LineRenderer;
			tempLine.GetComponent<Renderer>().material=LineBlack;
			for(int j=1;j<shortestPath.Count;j++)
				if(Lines[i].name.Contains(shortestPath[j-1]+" to "+shortestPath[j]+" Connection")||Lines[i].name.Contains(shortestPath[j]+" to "+shortestPath[j-1]+" Connection"))
					tempLine.GetComponent<Renderer>().material=LineGreen;
		}
	}
}
