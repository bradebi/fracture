using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SimpleAIGraphContainer))]
public class PathEditorTool : Editor {
	SimpleAIGraphContainer _target;
	GraphSimple Graph;

	[MenuItem("U.T.K./Pathfinding/Simple A.I. Graph")]
	void OnEnable()
	{
		_target = (SimpleAIGraphContainer)target;
		Graph = _target.GetGraph();
		if(Graph==null)
			Graph=new GraphSimple();
		//Ensure that the root node is part of the path
		if(Graph.NodeCount<1)
		{
			Graph.AddNodes(1);
			Graph.GetNodeList()[0].position=_target.transform.position;
		}
	}
	public override void OnInspectorGUI()
	{
		GUILayout.BeginVertical();
		GUILayout.Label ("Path Information", EditorStyles.boldLabel);
		GUILayout.Label ("Number of Nodes: "+Graph.NodeCount);
		GUILayout.Label ("Root ID: "+Graph.graph[0].ID);
		GUILayout.Label ("Root Position : "+Graph.graph[0].position);
		GUILayout.Label ("Neighbors: ", EditorStyles.boldLabel);
		for(int j=0;j<Graph.graph.Count;j++)
		{
			GUILayout.Label ("Node #"+Graph.graph[j].ID+" at: "+Graph.graph[j].position, EditorStyles.boldLabel);
			for(int i=0;i<Graph.graph[j].neighbors.Count;i++)
			{
				GUILayout.Label ("Node "+Graph.graph[j].neighbors[i].ID+" at: "+Graph.graph[j].neighbors[i].position );
			}
		}

		if(GUILayout.Button("Add Node"))
		{
			Graph.AddNodes(1);
			Graph.GetNodeList()[Graph.NodeCount-1].position=_target.transform.position+new Vector3(1,0,0);
			Graph.ConnectNodes(0,Graph.NodeCount-1,1);
			GameObject tempObj= new GameObject("");
			tempObj.transform.position=_target.transform.position+new Vector3(1,0,0);
			pathNodeContainer tempNode = tempObj.AddComponent(typeof(pathNodeContainer)) as pathNodeContainer;
			tempNode.Node=Graph.GetNodeList()[Graph.NodeCount-1];
			tempNode.GenerateName();
			tempObj.transform.parent=_target.transform;
		}
		Graph.graph[0].position=_target.transform.position;
		GUILayout.EndVertical();

	}
}