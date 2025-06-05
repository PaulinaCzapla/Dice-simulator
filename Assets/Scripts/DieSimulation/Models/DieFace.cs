using System;
using Attributes;
using DieSimulation.Data;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace DieSimulation.Models
{
    [Serializable]
    public sealed class DieFace
    {
        [SerializeField, ReadOnly] 
        private string identifier;
        [SerializeField] 
        private DieFaceValue faceValue;
        [SerializeField, HideInInspector] 
        private Vector3 centroid;
        [SerializeField, HideInInspector] 
        private Vector3 normal;
        [SerializeField, HideInInspector] 
        private TextMeshPro spawnedPresentationTextText;
        
        public DieFaceValue FaceValue => faceValue;
        public Vector3 Normal => normal;
        public Vector3 Centroid => centroid;

        public TextMeshPro SpawnedPresentationText
        {
            get => spawnedPresentationTextText;
            set => spawnedPresentationTextText = value;
        }

        public DieFace(Vector3 normal, Vector3 centroid, string identifier)
        {
            this.normal = normal;
            this.centroid = centroid;
            this.identifier = identifier;
        }

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Handles.Label(centroid, identifier);
#endif
        }
    }
}