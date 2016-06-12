using UnityEngine;
using UnityEditor;
using System.Collections;

public class SimpleAIGraphMenuItem : MonoBehaviour {

	[MenuItem("U.T.K./Pathfinding/Simple A.I. Graph")]
	public static void MakeAIGraph(){
		Selection.activeObject = SceneView.currentDrawingSceneView;
		Camera sceneCam = SceneView.currentDrawingSceneView.camera;
		Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f,0.5f,10f));
		GameObject PathNode = new GameObject();
		PathNode.transform.localPosition=spawnPos;
		PathNode.name="Simple AI Graph Controller-Root Node";
		SimpleAIGraphContainer AI= PathNode.AddComponent<SimpleAIGraphContainer>();
		Selection.activeObject=PathNode.gameObject;
	}
}
