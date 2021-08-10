using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BasicDecal : MonoBehaviour {

	[SerializeField] bool previewInEditMode = true;//if enabled makes the decal update in edit mode(can be slow)
	[SerializeField] bool resetOnPlay = false;//if enabled, makes the decal reset when starting a scene
	[SerializeField] float scale = 1;//determines the size of the decal
	[SerializeField] float decalOffset = 0.005f;//change this to make the decal come further off the wall if you are having zfighting issues.
	Bounds boxBounds;
	[SerializeField] MeshFilter mesh;
	public Vector3 rotation;
	Vector3 prevPosition;
	float prevDecalScale;
	float prevScale;
	Vector3 prevRotation;

	public void Awake() {
		mesh = GetComponentInChildren<MeshFilter>();
		boxBounds = new Bounds();
		boxBounds.size = scale * Vector3.one;
		boxBounds.center = transform.position;
	}

	// Use this for initialization
	void Start() {
		if(resetOnPlay) {
			ResetDecal();
		}
	}

	public void ResetDecal() {
		if(mesh.sharedMesh != null) {
			mesh.sharedMesh.Clear();
		}
		Collider[] intersected = Physics.OverlapBox(transform.position, Vector3.one * boxBounds.extents.x / 2, transform.rotation, Physics.DefaultRaycastLayers);
		List<MeshFilter> renderers = new List<MeshFilter>();
		for(int i = 0; i < intersected.Length; i++) {
			if(intersected[i].GetComponent<MeshFilter>()) {
				renderers.Add(intersected[i].GetComponent<MeshFilter>());
			}
		}
		if(renderers.Count > 0) {
			List<CombineInstance> combinedMesh = new List<CombineInstance>();
			for(int i = 0; i < renderers.Count; i++) {
				for(int x = 0; x < renderers[i].sharedMesh.subMeshCount; x++) {
					CombineInstance c = new CombineInstance();
					c.mesh = renderers[i].sharedMesh;
					c.transform = renderers[i].transform.localToWorldMatrix;
					c.subMeshIndex = x;
					combinedMesh.Add(c);
				}
			}
			mesh.sharedMesh = new Mesh();
			mesh.sharedMesh.CombineMeshes(combinedMesh.ToArray(), true);
			RemoveDoubles(mesh.sharedMesh, 1);
			List<Vector3> newVertices = new List<Vector3>();
			List<int> newTriangles = new List<int>();
			int currentTri = 0;
			float highest = Mathf.NegativeInfinity;
			float lowest = Mathf.Infinity;
			for(int i = 0; i < mesh.sharedMesh.triangles.Length; i += 3) {
				Bounds triBounds = new Bounds();
				triBounds.center = (mesh.sharedMesh.vertices[mesh.sharedMesh.triangles[i]] + mesh.sharedMesh.vertices[mesh.sharedMesh.triangles[i + 1]] + mesh.sharedMesh.vertices[mesh.sharedMesh.triangles[i + 2]]) / 3;
				triBounds.Encapsulate(mesh.sharedMesh.vertices[mesh.sharedMesh.triangles[i]]);
				triBounds.Encapsulate(mesh.sharedMesh.vertices[mesh.sharedMesh.triangles[i + 1]]);
				triBounds.Encapsulate(mesh.sharedMesh.vertices[mesh.sharedMesh.triangles[i + 2]]);
				if(boxBounds.Intersects(triBounds)) {
					Vector3 triNormal = (mesh.sharedMesh.normals[mesh.sharedMesh.triangles[i]] + mesh.sharedMesh.normals[mesh.sharedMesh.triangles[i + 1]] + mesh.sharedMesh.normals[mesh.sharedMesh.triangles[i + 2]]) / 3;
					float yAngle = (Quaternion.Inverse(Quaternion.Euler(rotation)) * triNormal).z;
					if(yAngle < lowest) {
						lowest = yAngle;
					}
					if(yAngle > highest) {
						highest = yAngle;
					}
					if(yAngle < 0) {
						//if any of the vertices of the triangle are contained in the bounds add them all
						newVertices.Add(transform.InverseTransformPoint(mesh.sharedMesh.vertices[mesh.sharedMesh.triangles[i]] + (mesh.sharedMesh.normals[mesh.sharedMesh.triangles[i]] * decalOffset)));
						newVertices.Add(transform.InverseTransformPoint(mesh.sharedMesh.vertices[mesh.sharedMesh.triangles[i + 1]] + (mesh.sharedMesh.normals[mesh.sharedMesh.triangles[i + 1]] * decalOffset)));
						newVertices.Add(transform.InverseTransformPoint(mesh.sharedMesh.vertices[mesh.sharedMesh.triangles[i + 2]] + (mesh.sharedMesh.normals[mesh.sharedMesh.triangles[i + 2]] * decalOffset)));
						newTriangles.Add(currentTri);
						newTriangles.Add(currentTri + 1);
						newTriangles.Add(currentTri + 2);
						currentTri += 3;
					}
				}
			}
			mesh.sharedMesh = new Mesh();
			mesh.sharedMesh.vertices = newVertices.ToArray();
			mesh.sharedMesh.triangles = newTriangles.ToArray();
			Vector2[] uv = new Vector2[mesh.sharedMesh.vertices.Length];
			for(int i = 0; i < mesh.sharedMesh.vertices.Length; i++) {
				Vector3 uvPos = (Quaternion.Inverse(Quaternion.Euler(rotation)) * mesh.sharedMesh.vertices[i]) + new Vector3(scale / 2, scale / 2, scale / 2);
				uv[i].x = uvPos.x / scale;
				uv[i].y = uvPos.y / scale;
			}
			mesh.sharedMesh.uv = uv;
			mesh.sharedMesh.Optimize();
			mesh.sharedMesh.RecalculateBounds();
		} else {
			Debug.Log("Decal System: No meshes found in bounding box");
		}
	}

	void RemoveDoubles(Mesh mesh, float bucketStep, float threshold = 0.0001f) {
		Vector3[] oldVertices = mesh.vertices;
		Vector3[] newVertices = new Vector3[oldVertices.Length];
		int[] old2new = new int[oldVertices.Length];
		int newSize = 0;

		// Find AABB
		Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		for(int i = 0; i < oldVertices.Length; i++) {
			if(oldVertices[i].x < min.x) min.x = oldVertices[i].x;
			if(oldVertices[i].y < min.y) min.y = oldVertices[i].y;
			if(oldVertices[i].z < min.z) min.z = oldVertices[i].z;
			if(oldVertices[i].x > max.x) max.x = oldVertices[i].x;
			if(oldVertices[i].y > max.y) max.y = oldVertices[i].y;
			if(oldVertices[i].z > max.z) max.z = oldVertices[i].z;
		}

		// Make cubic buckets, each with dimensions "bucketStep"
		int bucketSizeX = Mathf.FloorToInt((max.x - min.x) / bucketStep) + 1;
		int bucketSizeY = Mathf.FloorToInt((max.y - min.y) / bucketStep) + 1;
		int bucketSizeZ = Mathf.FloorToInt((max.z - min.z) / bucketStep) + 1;
		List<int>[,,] buckets = new List<int>[bucketSizeX, bucketSizeY, bucketSizeZ];

		// Make new vertices
		for(int i = 0; i < oldVertices.Length; i++) {
			// Determine which bucket it belongs to
			int x = Mathf.FloorToInt((oldVertices[i].x - min.x) / bucketStep);
			int y = Mathf.FloorToInt((oldVertices[i].y - min.y) / bucketStep);
			int z = Mathf.FloorToInt((oldVertices[i].z - min.z) / bucketStep);

			// Check to see if it's already been added
			if(buckets[x, y, z] == null)
				buckets[x, y, z] = new List<int>(); // Make buckets lazily

			for(int j = 0; j < buckets[x, y, z].Count; j++) {
				Vector3 to = newVertices[buckets[x, y, z][j]] - oldVertices[i];
				if(Vector3.SqrMagnitude(to) < threshold) {
					old2new[i] = buckets[x, y, z][j];
					goto skip; // Skip to next old vertex if this one is already there
				}
			}

			// Add new vertex
			newVertices[newSize] = oldVertices[i];
			buckets[x, y, z].Add(newSize);
			old2new[i] = newSize;
			newSize++;

			skip:;
		}

		// Make new triangles
		int[] oldTris = mesh.triangles;
		int[] newTris = new int[oldTris.Length];
		for(int i = 0; i < oldTris.Length; i++) {
			newTris[i] = old2new[oldTris[i]];
		}

		Vector3[] finalVertices = new Vector3[newSize];
		for(int i = 0; i < newSize; i++)
			finalVertices[i] = newVertices[i];

		mesh.Clear();
		mesh.vertices = finalVertices;
		mesh.triangles = newTris;
		mesh.RecalculateNormals();
	}

	void OnDrawGizmosSelected() {
		if(previewInEditMode) {
			if(!Application.isPlaying) {
				if(transform.position != prevPosition || prevScale != scale || prevRotation != rotation) {
					Awake();
					ResetDecal();
					prevPosition = transform.position;
					prevScale = scale;
					prevRotation = rotation;
				}
			}
		}
		Gizmos.color = Color.black;
		Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(rotation), scale * Vector3.one);
		Gizmos.DrawLine(Vector3.back / 2, Vector3.forward);
		Gizmos.DrawWireCube(Vector3.forward / 2, new Vector3(0.9f, 0.9f, 0));
		Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, scale * Vector3.one);
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}
}
