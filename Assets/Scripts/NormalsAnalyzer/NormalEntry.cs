﻿using System;
using System.Collections.Generic;
using Attributes;
using UnityEngine;

namespace NormalsAnalyzer
{
    [Serializable]
    public sealed class NormalEntry
    {
        [SerializeField, ReadOnly] 
        private Vector3 normal;
        [SerializeField, HideInInspector] 
        private List<Vector3> vertices;
        [SerializeField, HideInInspector] 
        private Vector3 centroid;

        public NormalEntry(Vector3 normal, List<Vector3> vertices)
        {
            this.normal = normal;
            this.vertices = vertices;
            RecalculateCentroid();
        }

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

        public Vector3 Centroid => centroid;

        private void RecalculateCentroid()
        {
            centroid = Vector3.zero;

            foreach (var v in Vertices)
            {
                centroid += v;
            }

            centroid /= Vertices.Count;
        }

        public void OnDrawGizmos(Transform transform)
        {
            if (Vertices == null || Vertices.Count == 0)
            {
                return;
            }

            var worldCentroid = transform.TransformPoint(Centroid);
            var worldNormal = transform.TransformDirection(Normal);

            Gizmos.DrawSphere(worldCentroid, 0.02f);
            Gizmos.DrawRay(worldCentroid, worldNormal * 0.1f);
        }
    }
}