using System;
using Attributes;
using Dice.Data;
using UnityEditor;
using UnityEngine;

namespace Dice
{
    [Serializable]
    public class DiceFace
    {
        [SerializeField, ReadOnly] private string identifier;
        [SerializeField] private DiceValue value;
        
        [SerializeField] private Vector3 centroid;
        [SerializeField, HideInInspector] private Vector3 normal;
        [SerializeField, HideInInspector] private GameObject spawnedPresentation;

        public DiceValue Value => value;
        public Vector3 Normal => normal;

        public string Identifier => identifier;
        public Vector3 Centroid => centroid;

        public GameObject SpawnedPresentation
        {
            get => spawnedPresentation;
            set => spawnedPresentation = value;
        }

        public DiceFace(Vector3 normal, Vector3 centroid, string identifier)
        {
            this.normal = normal;
            this.centroid = centroid;
            this.identifier = identifier;
        }

        public void DrawGizmos()
        {
            Handles.Label(centroid, identifier);
        }
    }
}