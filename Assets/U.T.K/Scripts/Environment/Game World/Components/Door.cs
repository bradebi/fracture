using UnityEngine;
using System.Collections;
//NOTES
//-Physics-based doors, block or break by placing objects in way
//-Power outage means doors slide on their axis without resistance or frozen shut
//-Transfer pressure on open. Break door if pressure differential is too great. 
//-Door breaking visualization? Pressure break, blast break.
//
//LAYER- Layers to affect the door: Door, Player, Phys_Prop
public class Door : MonoBehaviour {
	public bool isWeldable,isBreakable; //Properties Flags
	public enum door_State{isOpen,isBroken,isSealed,isClosed} //Door State
	public door_State Door_State;
	public float Blast_Resistance,Pressure_Resistance;
	public int ID_Door;
	public GameObject Room_Connection1, Room_Connection2; //Store the rooms that are connected
	public float Door_portal_size; //Decides the transfer of atmospheric properties between rooms
	public float friction;

	private GameObject Door_Mesh;
	private 

	void Start(){
		//TODO: Generate Door_Portal_Size
		//Get mesh


	}

	void Update(){
		if(Door_Mesh.GetComponent<Rigidbody>().velocity.magnitude>0)
			//Constrain physics motion
			Phys_Constrain();

		//if(open||ButtonClose)
	}

	void Phys_Move_Door(){

	}

	public void Lock_Door(){

	}

	public void Unlock_Door(){

	}
	
	//TODO: Move to atmosphere manager
	void Transfer_Atm(){
	
	
	}
	
	void Transfer_Gas(){
	
	
	}

	void Phys_Constrain(){
		Vector3 localVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
		localVelocity.x = 0;
		localVelocity.z = 0;
		
		GetComponent<Rigidbody>().velocity = transform.TransformDirection(localVelocity);
	}
	
	}