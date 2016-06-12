using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Phys_RopeMenuItem : MonoBehaviour {
	
	[MenuItem("U.T.K./Physics/3D Rope")]
	public static void BuildRope(){
		Selection.activeObject = SceneView.currentDrawingSceneView;
		Camera sceneCam = SceneView.currentDrawingSceneView.camera;
		Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f,0.5f,10f));
		GameObject Rope = new GameObject();
		Rope.transform.localPosition=spawnPos;
		Rope.name="Rope";
		Rope.AddComponent<Phys_Rope_3D>();
		Selection.activeObject=Rope.gameObject;
	}
	
}