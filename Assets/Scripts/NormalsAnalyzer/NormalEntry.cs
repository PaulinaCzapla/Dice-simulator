using System;
using System.Collections.Generic;
using UnityEngine;

namespace NormalsAnalyzer
{
    [Serializable]
    public class NormalEntry
    {
        [Attributes.ReadOnly]
        [SerializeField]
        private Vector3 normal;
        
        [HideInInspector]
        [SerializeField]
        private List<Vector3> vertices;
        
        [Attributes.ReadOnly]
        [SerializeField]
        private float surfaceArea;

        [HideInInspector] 
        [SerializeField]
        private List<Vector3> triangleCenters;

        [HideInInspector] 
        [SerializeField] 
        private Vector3 centroid;
        
        public Vector3 Normal
        {
            get => normal;
            set => normal = value;
        }
        
        public List<Vector3> Vertices 
        {
            get => vertices;
            set
            {
                vertices = value;
                RecalculateCentroid();
            }
        }

        public float SurfaceArea
        {
            get => surfaceArea;
            set => surfaceArea = value;
        }

        public List<Vector3> TriangleCenters
        {
            get => triangleCenters;
            set => triangleCenters = value;
        }

        public Vector3 Centroid => centroid;
        
        public NormalEntry(Vector3 normal, List<Vector3> vertices, float surfaceArea, List<Vector3> triangleCenters)
        {
            this.normal = normal;
            this.vertices = vertices;
            this.surfaceArea = surfaceArea;
            this.triangleCenters = triangleCenters;
            RecalculateCentroid();
        }

        private void RecalculateCentroid()
        {
            centroid = Vector3.zero;
            
            foreach (var v in Vertices)
                centroid += v;
            
            centroid /= Vertices.Count;
        }

        public void DrawGizmos(Transform transform)
        {
            if (Vertices == null || Vertices.Count == 0)
                return;
            
            Vector3 worldCentroid = transform.TransformPoint(Centroid);
            Vector3 worldNormal = transform.TransformDirection(Normal);
            
            Gizmos.DrawSphere(worldCentroid, 0.02f);
            Gizmos.DrawRay(worldCentroid, worldNormal * 0.1f);
        }
    }
}