using System;
using System.Collections.Generic;
using DieSimulation.Data;
using DieSimulation.Models;
using NormalsAnalyzer;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace DieSimulation.Components
{
    [RequireComponent(typeof(MeshNormalsAnalyzer))]
    public sealed class DieValuesHandler : MonoBehaviour
    {
        [SerializeField, HideInInspector] 
        private MeshNormalsAnalyzer meshNormalsAnalyzer;
        [SerializeField] 
        private List<DieFace> dieFaces = new();

        public IReadOnlyCollection<DieFace> DieFaces => dieFaces;

        private void Awake()
        {
            foreach (var die in dieFaces)
            {
                if (die.SpawnedPresentationText)
                    die.SpawnedPresentationText.text = die.FaceValue.Value.ToString();
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            meshNormalsAnalyzer ??= GetComponent<MeshNormalsAnalyzer>();
            UpdateNormals();
        }

        private void OnValidate()
        {
            meshNormalsAnalyzer ??= GetComponent<MeshNormalsAnalyzer>();
            ValidateFacesList();
        }

        private void ValidateFacesList()
        {
            var normalsCount = meshNormalsAnalyzer.FoundNormals.Count;
            dieFaces = dieFaces.GetRange(0, normalsCount);
        }
        
        public void UpdateNormals()
        {
            if (meshNormalsAnalyzer == null)
            {
                return;
            }
            DestroyPresentation();
            
            dieFaces.Clear();
            int nameIndex = 1;
            
            foreach (var normal in meshNormalsAnalyzer.FoundNormals)
            {
                dieFaces.Add(new DieFace(normal.Normal, normal.Centroid, $"face {nameIndex}"));
                nameIndex++;
            }

            EditorUtility.SetDirty(this);
        }

        public void GeneratePresentation()
        {
            int index = 1;
            foreach (var face in dieFaces)
            {
                GameObject textObject = null;

                if (face.SpawnedPresentationText)
                {
                    textObject = face.SpawnedPresentationText.gameObject;
                }
                else
                {
                    textObject = new GameObject();
                    textObject.transform.SetParent(transform);
                    textObject.transform.position = face.Centroid + face.Normal.normalized * 0.01f;
                    textObject.transform.rotation = Quaternion.LookRotation(face.Normal);
                }
                
                textObject.name = $"Face {face.FaceValue.Value.ToString()}";
                
                var textMesh = textObject.GetOrAddComponent<TextMeshPro>();
                textMesh.text = face.FaceValue.Value.ToString();
                textMesh.text = index.ToString();
                textMesh.fontSize = face.FaceValue.FontSize;
                textMesh.alignment = TextAlignmentOptions.Center;
                face.SpawnedPresentationText = textMesh;
                index++;
            }

            EditorUtility.SetDirty(this);
        }

        private void DestroyPresentation()
        {
            foreach (var face in dieFaces)
            {
                if(face.SpawnedPresentationText)
                    DestroyImmediate(face.SpawnedPresentationText.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var face in dieFaces)
            {
                face.OnDrawGizmos();
            }
        }
#endif
    }
}