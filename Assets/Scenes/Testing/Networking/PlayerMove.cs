using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

    UTK.Physics.Prediction.DeadReckoning prediction;
    UTK.Physics.KinematicState kinematicState;
    float thrust = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        
    }

    Rigidbody rb;

    void Update()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * 0.1f;
        var z = Input.GetAxis("Vertical") * 0.1f;

        rb.AddForce(new Vector3(x, 0, z) * thrust);

        

        //transform.Translate(x, 0, z);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
