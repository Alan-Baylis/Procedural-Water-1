using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

	public int xSize, ySize;
	public Light mainLight;

	private Mesh mesh;
	private Vector3[] vertices;
	private bool generated;

	private void Awake(){
		generated = false;
		Generate ();
	}

	private void Generate(){

		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedural Grid";

		//Applies vertices, uvs and tangents
		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(1f, 0, 0, -1f);

		for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices [i] = new Vector3 (x, y);
				uv [i] = new Vector2 ((float)x/xSize, (float)y/ySize);
				tangents [i] = tangent;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uv;

		//Applies triangles, and autocalculates normals
		int[] triangles = new int[ySize * xSize * 6];
		for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
			for (int x = 0; x < xSize; x++, ti += 6, vi++) {
				triangles [ti] = vi;
				triangles [ti + 3] = triangles [ti + 2] = vi + 1;
				triangles [ti + 4] = triangles [ti + 1] = vi + xSize + 1;
				triangles [ti + 5] = vi + xSize + 2;

				mesh.triangles = triangles;
				mesh.RecalculateNormals ();
				mesh.tangents = tangents;
			}
		}

		generated = true;
	}
	
	private void Update () {
		mainLight.transform.RotateAround (mainLight.transform.position, Vector3.forward, 3f);		

		if (generated) {
			for (int i = 0; i < vertices.Length; i++) {
				vertices [i].z = Mathf.Sin (Time.fixedTime + i%xSize);
			}

			mesh.vertices = vertices;
			mesh.RecalculateNormals();
		}
	}
}