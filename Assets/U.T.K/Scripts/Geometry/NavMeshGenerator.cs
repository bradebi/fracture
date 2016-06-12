using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NavMeshGenerator {

	public static Mesh CreatePlane(float width, float height, int segments) {	
		Mesh mesh = new Mesh();
		
		Vector3[] vertices = new Vector3[(segments+1)*(segments+1)];
		Vector3[] normals = new Vector3[(segments+1)*(segments+1)];
		Vector2[] uv = new Vector2[(segments+1)*(segments+1)];

		for(int i=0;i<=segments;i++){
			for(int j=0;j<=segments;j++){
				vertices[i*(segments+1)+j] = new Vector3(j*(width/segments)-width/2, 0, i*(height/segments)-height/2);
				normals[i*(segments+1)+j] = -Vector3.forward;
				uv[i*(segments+1)+j] = new Vector2((j/(float)segments), (i/(float)segments));

			}
		}
			mesh.vertices = vertices;
			
		int[] tri = new int[segments*segments*6];
		int index=0;
		for(int i=0;i<=segments*segments+(segments-2);i++){
			if((i+1)%(segments+1)!=0){
				if((i+1)%2==0)
				{
					tri[index++] = i;
					tri[index++] = i+segments+1;
					tri[index++] = i+1;
						
					tri[index++] = i+1;
					tri[index++] = i+segments+1;
					tri[index++] = i+segments+2;
				}
				else
				{
					tri[index++] = i;
					tri[index++] = i+segments+1;
					tri[index++] = i+segments+2;
					
					tri[index++] = i;
					tri[index++] = i+segments+2;
					tri[index++] = i+1;
				}
			}
		}
			mesh.triangles = tri;
			
			mesh.normals = normals;

			mesh.uv = uv;

		return mesh;
	}
	
	public static void RemoveTriangles(Mesh inMesh, List<int> DeadVertices)
	{
		List<int> indices = new List<int>(inMesh.triangles);
		int count = indices.Count / 3;
		for (int i = count-1; i >= 0; i--)
		{
			int V1 = indices[i*3 + 0];
			int V2 = indices[i*3 + 1];
			int V3 = indices[i*3 + 2];
			if (DeadVertices.Contains(V1)||DeadVertices.Contains(V2)||DeadVertices.Contains(V3))
			{
				indices.RemoveRange(i*3, 3);
			}
		}
		inMesh.triangles = indices.ToArray();

	}
}
