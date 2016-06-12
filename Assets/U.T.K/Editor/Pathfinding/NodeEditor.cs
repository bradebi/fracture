using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(pathNodeContainer))]
public class NodeEditor : Editor {
	SimpleAIGraphContainer GraphContainer;
	GraphSimple Graph;
	pathNodeContainer _target;
	// Use this for initialization

	void OnEnable()
	{
		_target = (pathNodeContainer)target;
		GraphContainer=_target.transform.parent.GetComponent(typeof(SimpleAIGraphContainer)) as SimpleAIGraphContainer;
		Graph=GraphContainer.GetGraph();
	}
bool destroyMe=false;
public override void OnInspectorGUI()
{
	
	GraphContainer=_target.transform.parent.GetComponent(typeof(SimpleAIGraphContainer)) as SimpleAIGraphContainer;
	
	if(_target.Node!=Graph.GetNodeList()[_target.Node.ID])
		_target.Node=Graph.GetNodeList()[_target.Node.ID];

	GUILayout.BeginVertical();
	if(_target.Node!=null){
		GUILayout.Label ("Node Information", EditorStyles.boldLabel);
		GUILayout.Label ("ID: "+_target.Node.ID);
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Passthrough?: ");
		if(_target.Node.passThrough){
			if(GUILayout.Button("True")){
					_target.Node.passThrough=false;
			}
		}
		else{
			if(GUILayout.Button("False")){
				_target.Node.passThrough=true;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Label ("Position: "+_target.Node.position);
		GUILayout.Label ("Neighbors: ", EditorStyles.boldLabel);
		for(int i=0;i<_target.Node.neighbors.Count;i++)
		{
			GUILayout.Label ("Node "+_target.Node.neighbors[i].ID);
		}
	}

	_target.Node.position=_target.transform.position;

	if(GUILayout.Button("Add Node")&&GraphContainer!=null)
	{
		Graph.AddNodes(1);
		Graph.GetNodeList()[GraphContainer.GetGraph().NodeCount-1].position=_target.transform.position+new Vector3(1,0,0);
		Graph.ConnectNodes(_target.Node.ID,Graph.NodeCount-1,1);
		Graph.ConnectNodes(Graph.NodeCount-1,_target.Node.ID,1);
		GameObject tempObj= new GameObject("");
		tempObj.transform.position=_target.transform.position+new Vector3(1,0,0);
		pathNodeContainer tempNode = tempObj.AddComponent(typeof(pathNodeContainer)) as pathNodeContainer;
		tempNode.Node=Graph.GetNodeList()[Graph.NodeCount-1];
		tempNode.GenerateName();
		tempObj.transform.parent=GraphContainer.transform;
	}
	
	if(GUILayout.Button("Merge Closest Node")&&GraphContainer!=null)
	{
		int closestID=_target.Node.ID;
		float minDist=Mathf.Infinity;
		for(int i=0;i<Graph.NodeCount;i++)
		{
			float dist=Vector3.Distance(Graph.GetNodeList()[i].position,_target.transform.position);
			if(dist<minDist&&Graph.GetNodeList()[i].ID!=_target.Node.ID)
			{
				minDist=dist;
				closestID=Graph.GetNodeList()[i].ID;
			}
		}
		if(closestID!=_target.Node.ID){
			if(EditorUtility.DisplayDialog("Merge nodes?","The closest node is node " + closestID+ ", would you like to merge these nodes together?", "Merge", "Cancel")){
				GraphContainer.MergeNodes(_target.Node.ID,closestID);
				destroyMe=true;
			}
		}
	}

	if(GUILayout.Button("Delete Node")&&GraphContainer!=null)
	{
		if(EditorUtility.DisplayDialog("Delete Selected Node?","Are you sure you want to delete node " + _target.Node.ID+ "?", "Delete", "Cancel")){
			GraphContainer.DeleteNode(_target.Node.ID);
			destroyMe=true;
		}
	}
	GUILayout.EndVertical();
	if(destroyMe)
			DestroyImmediate(_target.gameObject);
}
}
