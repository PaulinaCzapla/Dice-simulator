using System;
using System.Collections.Generic;
using NormalsAnalyzer;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Die
{
    [RequireComponent(typeof(MeshNormalsAnalyzer))]
    public sealed class DieValuesHolder : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private MeshNormalsAnalyzer meshNormalsAnalyzer;

        [SerializeField]
        private List<DieFace> diceFaces = new();

        public IEnumerable<DieFace> DiceFaces => diceFaces;
        
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
            diceFaces = diceFaces.GetRange(0, normalsCount);
        }

        public void UpdateNormals()
        {
            if (meshNormalsAnalyzer == null)
            {
                return;
            }
            
            diceFaces.Clear();
            var i = 1;
            foreach (var normal in meshNormalsAnalyzer.FoundNormals)
            {
                diceFaces.Add(new DieFace(normal.Normal, normal.Centroid, $"face {i}"));
                i++;
            }
            
            EditorUtility.SetDirty(this);
        }

        public void GeneratePresentation()
        {
            foreach (var face in diceFaces)
            {
                if (face.SpawnedPresentation)
                {
                    DestroyImmediate(face.SpawnedPresentation);
                }
                
                var textObject = new GameObject();
                textObject.transform.SetParent(transform);
                textObject.name = face.FaceValue.Value.ToString();
                textObject.transform.position = face.Centroid + face.Normal.normalized *0.01f;
                textObject.transform.rotation = Quaternion.LookRotation(face.Normal);
                var textMesh = textObject.AddComponent<TextMeshPro>();

                textMesh.text = face.FaceValue.Value.ToString();
                textMesh.fontSize = face.FaceValue.FontSize;
                textMesh.alignment = TextAlignmentOptions.Center;
                face.SpawnedPresentation = textObject;
            }
            
            EditorUtility.SetDirty(this);
        }

        private void OnDrawGizmos()
        {
            foreach (var face in diceFaces)
            {
                face.OnDrawGizmos();
            }
        }
    }
}