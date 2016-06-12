using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Phys_Rope_3D : MonoBehaviour {

	private GameObject start,center,end;
	private List<GameObject> RopeSegs;
	private Vector3 endPos;
	public float rope_length=20;
	public int interp=8;
	public int PhysSegments=1;
	
	private float distancelimit;
	private ConfigurableJoint startJoint,centerJoint,endJoint;
	//Set up rope given parameters
	void Start () {
		endPos=gameObject.transform.position-new Vector3(0,rope_length,0);
		GenerateRope();
	}

	private LineRenderer Rope;
	private List<Vector3> SplinePts= new List<Vector3>();
	private List<Vector3> RopePts= new List<Vector3>();
	void Update () {
		if(Rope==null){
			Rope=gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
			//Rope.materials[0].
			Rope.materials[0].shader=Shader.Find("Bumped Specular");
			Rope.materials[0].SetColor("_EmisColor",Color.white);
			Rope.SetWidth(0.1f,0.1f);
		}
		RopePts.Clear ();
		RopePts.Add (start.transform.position);
		RopePts.Add (start.transform.position);

		for(int i=0;i<RopeSegs.Count;i++)
		{
			RopePts.Add (RopeSegs[i].transform.position);
		}

		RopePts.Add (end.transform.position);
		RopePts.Add (end.transform.position);

		SplinePts=Spline_Catmull_Rom.GetPoints(RopePts.ToArray(),interp);
		UpdateRope();

	}

	void UpdateRope () {
		Rope.SetVertexCount(SplinePts.Count);

		for(int i=0;i<SplinePts.Count;i++)
		{
			Rope.SetPosition(i,SplinePts[i]);
		}
	}

	void GenerateRope() {
		SoftJointLimit softJointLimit = new SoftJointLimit();
		
		distancelimit=Vector3.SqrMagnitude((gameObject.transform.position+endPos)/2f);
		RopeSegs=new List<GameObject>();

		start=GameObject.CreatePrimitive(PrimitiveType.Cube);
		start.GetComponent<Renderer>().enabled=false;
		start.transform.position=gameObject.transform.position;
		start.AddComponent(typeof(Rigidbody));
		start.transform.localScale=start.transform.localScale*0.1f;
		start.name="Rope_Start";
		startJoint=start.AddComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;
		startJoint.xMotion=ConfigurableJointMotion.Locked;
		startJoint.yMotion=ConfigurableJointMotion.Locked;
		startJoint.zMotion=ConfigurableJointMotion.Locked;
		RopeSegs.Add(start);

		for(int i=0; i< PhysSegments;i++)
		{
			center=GameObject.CreatePrimitive(PrimitiveType.Cube);
			RopeSegs.Add (center);
			RopeSegs[RopeSegs.Count-1].transform.position=Vector3.Lerp(gameObject.transform.position,endPos,((i+1f)/(PhysSegments+1f)));
			RopeSegs[RopeSegs.Count-1].AddComponent(typeof(Rigidbody));
			RopeSegs[RopeSegs.Count-1].transform.localScale=RopeSegs[RopeSegs.Count-1].transform.localScale*0.1f;
			RopeSegs[RopeSegs.Count-1].name="Seg "+i;
			RopeSegs[RopeSegs.Count-1].GetComponent<Rigidbody>().drag=0.5f;
			RopeSegs[RopeSegs.Count-1].GetComponent<Renderer>().enabled=false;

			centerJoint=RopeSegs[RopeSegs.Count-1].AddComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;
			centerJoint.xMotion=ConfigurableJointMotion.Limited;
			centerJoint.yMotion=ConfigurableJointMotion.Limited;
			centerJoint.zMotion=ConfigurableJointMotion.Limited;

			centerJoint.connectedBody=RopeSegs[RopeSegs.Count-2].GetComponent<Rigidbody>();

			centerJoint.linearLimit=softJointLimit;
		}

		
		end=GameObject.CreatePrimitive(PrimitiveType.Cube);
		end.transform.position=endPos;
		end.AddComponent(typeof(Rigidbody));
		end.transform.localScale=end.transform.localScale*0.1f;
		end.name="Rope_End";
		end.GetComponent<Rigidbody>().drag=0.5f;
		//end.renderer.enabled=false;

		endJoint=end.AddComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;
		endJoint.xMotion=ConfigurableJointMotion.Limited;
		endJoint.yMotion=ConfigurableJointMotion.Limited;
		endJoint.zMotion=ConfigurableJointMotion.Limited;
		endJoint.connectedBody=RopeSegs[RopeSegs.Count-1].GetComponent<Rigidbody>();
		endJoint.linearLimit=softJointLimit;
		RopeSegs.Add(end);
	}

	public bool showJoints=false;
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "Rope.png");
		if(showJoints&&RopeSegs!=null){
			for(int i=0;i<RopeSegs.Count;i++)
			{
				Gizmos.color=Color.yellow;
				Gizmos.DrawSphere(RopeSegs[i].transform.position,0.1f);
			}
		}
	}
}
