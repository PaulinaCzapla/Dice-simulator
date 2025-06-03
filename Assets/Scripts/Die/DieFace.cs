using System;
using Attributes;
using Die.Data;
using UnityEditor;
using UnityEngine;

namespace Die
{
    [Serializable]
    public sealed class DieFace
    {
        [SerializeField, ReadOnly] private string identifier;
        [SerializeField] private DieFaceValue faceValue;
        
        [SerializeField, HideInInspector] private Vector3 centroid;
        [SerializeField, HideInInspector] private Vector3 normal;
        [SerializeField, HideInInspector] private GameObject spawnedPresentation;

        public DieFaceValue FaceValue => faceValue;
        public Vector3 Normal => normal;
        public Vector3 Centroid => centroid;

        public GameObject SpawnedPresentation
        {
            get => spawnedPresentation;
            set => spawnedPresentation = value;
        }

        public DieFace(Vector3 normal, Vector3 centroid, string identifier)
        {
            this.normal = normal;
            this.centroid = centroid;
            this.identifier = identifier;
        }

        public void OnDrawGizmos()
        {
            Handles.Label(centroid, identifier);
        }
    }
}