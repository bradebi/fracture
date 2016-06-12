using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour {
    public List<GameObject> roomComponents = new List<GameObject>();

    NavMesh roomNavmesh;

    /// <summary>
    /// Generates an inverse convex-hull mesh containing the room's volume. 
    /// This is particularly useful for understanding paths for decompression
    /// and other events.
    /// </summary>
    public void SolveRoomVolume()
    {

    }
}
