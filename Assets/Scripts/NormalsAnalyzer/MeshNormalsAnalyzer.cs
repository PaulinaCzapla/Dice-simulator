using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace NormalsAnalyzer
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    public sealed class MeshNormalsAnalyzer : MonoBehaviour
    {
        [Header("Analyze parameters")] [Tooltip("Minimal surface for given normal.")] [SerializeField]
        private float minSurface = 0.1f;

        [Tooltip("Tolerance when comparing normals .")] [SerializeField]
        private float normalTolerance = 0.01f;

        [Header("Found normals")] [SerializeField]
        private List<NormalEntry> foundNormals = new();

        public List<NormalEntry> FoundNormals => foundNormals;

        private void OnDrawGizmosSelected()
        {
            if (foundNormals == null || foundNormals.Count == 0)
            {
                return;
            }

            Gizmos.color = Color.cyan;

            foreach (var entry in foundNormals)
            {
                entry.OnDrawGizmos(transform);
            }
        }

        [ContextMenu("Analyze Normals")]
        public void AnalyzeNormals()
        {
            foundNormals.Clear();

            var meshFilter = GetComponent<MeshFilter>();
            var mesh = meshFilter.sharedMesh;

            if (!mesh)
            {
                Debug.LogError("Mesh not assigned!");
                return;
            }

            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

            mesh.RecalculateNormals();
            var normals = mesh.normals;

            var normalComparer = new ApproxVector3Comparer(normalTolerance);
            var normalsToTriangles = new Dictionary<Vector3, List<(Vector3, Vector3, Vector3)>>(normalComparer);

            for (var i = 0; i < triangles.Length; i += 3)
            {
                var i0 = triangles[i];
                var i1 = triangles[i + 1];
                var i2 = triangles[i + 2];

                var v0 = vertices[i0];
                var v1 = vertices[i1];
                var v2 = vertices[i2];

                var avgNormal = ((normals[i0] + normals[i1] + normals[i2]) / 3f).normalized;

                if (!normalsToTriangles.ContainsKey(avgNormal))
                {
                    normalsToTriangles[avgNormal] = new List<(Vector3, Vector3, Vector3)>();
                }

                normalsToTriangles[avgNormal].Add((v0, v1, v2));
            }

            foreach (var normalToTriangle in normalsToTriangles)
            {
                var triangleList = normalToTriangle.Value;
                var vertexSet = new HashSet<Vector3>(new ApproxVector3Comparer(normalTolerance));
                var totalArea = 0f;

                foreach (var (v0, v1, v2) in triangleList)
                {
                    vertexSet.Add(v0);
                    vertexSet.Add(v1);
                    vertexSet.Add(v2);
                    totalArea += Vector3Utils.CalculateTriangleArea(v0, v1, v2);
                }

                if (totalArea >= minSurface)
                {
                    foundNormals.Add(
                        new NormalEntry(normalToTriangle.Key.normalized, new List<Vector3>(vertexSet)));
                }
            }
        }
    }
}