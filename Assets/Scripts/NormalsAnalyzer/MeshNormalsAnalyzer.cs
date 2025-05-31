using System.Collections.Generic;
using UnityEngine;

namespace NormalsAnalyzer
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    public class MeshNormalsAnalyzer : MonoBehaviour
    {
        [Header("Analyze parameters")]
        [Tooltip("Minimal surface for given normal.")]
        [SerializeField]
        private float minSurface = 0.1f;

        [Tooltip("Tolerance when comparing normals .")]
        [SerializeField]
        private float normalTolerance = 0.01f;

        [Header("Found normals")]
        [SerializeField]
        private List<NormalEntry> foundNormals = new();
        
        [ContextMenu("Analyze Normals")]
        public void AnalyzeNormals()
        {
            foundNormals.Clear();

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            
            if (!mesh)
            {
                Debug.LogError("Mesh not assigned!");
                return;
            }

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            mesh.RecalculateNormals();
            Vector3[] normals = mesh.normals;

            var normalComparer = new ApproxVector3Comparer(normalTolerance);
            var normalsToTriangles = new Dictionary<Vector3, List<(Vector3, Vector3, Vector3)>>(normalComparer);

            for (int i = 0; i < triangles.Length; i += 3)
            {
                int i0 = triangles[i];
                int i1 = triangles[i + 1];
                int i2 = triangles[i + 2];

                Vector3 v0 = vertices[i0];
                Vector3 v1 = vertices[i1];
                Vector3 v2 = vertices[i2];

                Vector3 avgNormal = ((normals[i0] + normals[i1] + normals[i2]) / 3f).normalized;

                if (!normalsToTriangles.ContainsKey(avgNormal))
                    normalsToTriangles[avgNormal] = new();

                normalsToTriangles[avgNormal].Add((v0, v1, v2));
            }

            foreach (var normalToTriangle in normalsToTriangles)
            {
                var triangleList = normalToTriangle.Value;
                var vertexSet = new HashSet<Vector3>(new ApproxVector3Comparer(normalTolerance));
                var triangleCenters = new List<Vector3>();
                float totalArea = 0f;

                foreach (var (v0, v1, v2) in triangleList)
                {
                    vertexSet.Add(v0);
                    vertexSet.Add(v1);
                    vertexSet.Add(v2);
                    triangleCenters.Add((v0 + v1 + v2) / 3f);
                    totalArea += CalculateTriangleArea(v0, v1, v2);
                }

                if (totalArea >= minSurface)
                {
                    foundNormals.Add(new NormalEntry
                    (normalToTriangle.Key, new (vertexSet), totalArea, triangleCenters));
                }
            }
        }

        private float CalculateTriangleArea(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            return Vector3.Cross(v1 - v0, v2 - v0).magnitude * 0.5f;
        }
        
        private void OnDrawGizmosSelected()
        {
            if (foundNormals == null || foundNormals.Count == 0)
                return;

            Gizmos.color = Color.cyan;

            foreach (var entry in foundNormals)
                entry.DrawGizmos(transform);
        }
    }
}