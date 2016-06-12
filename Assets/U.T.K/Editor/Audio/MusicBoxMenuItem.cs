using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MusicBoxMenuItem : MonoBehaviour {

	[MenuItem("U.T.K./Audio/Music Box")]
	public static void MakeMusicBox(){
	    Selection.activeObject = SceneView.currentDrawingSceneView;
        Camera sceneCam = SceneView.currentDrawingSceneView.camera;
        Vector3 spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f,0.5f,10f));
		GameObject MusicBox = new GameObject();
		MusicBox.transform.localPosition=spawnPos;
		MusicBox.name="Music Box";
		MusicBox.AddComponent<Music_Box>();
		Selection.activeObject=MusicBox.gameObject;
	}
	
}