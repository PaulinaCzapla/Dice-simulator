using System;
using System.Collections.Generic;
using Attributes;
using Dice.Data;
using NormalsAnalyzer;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Dice
{
    [RequireComponent(typeof(MeshNormalsAnalyzer))]
    public class DiceValuesHolder : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private MeshNormalsAnalyzer meshNormalsAnalyzer;

        [SerializeField]
        private List<DiceFace> diceFaces = new();

        public List<DiceFace> DiceFaces => diceFaces;
        
        private void Reset()
        {
            meshNormalsAnalyzer = GetComponent<MeshNormalsAnalyzer>();
            UpdateNormals();
        }

        public void UpdateNormals()
        {
            if(meshNormalsAnalyzer == null)
                return;
            
            diceFaces.Clear();
            int i = 1;
            foreach (var normal in meshNormalsAnalyzer.FoundNormals)
            {
                diceFaces.Add(new DiceFace(normal.Normal, normal.Centroid, $"face {i}"));
                i++;
            }
            EditorUtility.SetDirty(this);
        }

        public void GeneratePresentation()
        {
            foreach (var face in diceFaces)
            {
                if(face.SpawnedPresentation)
                    DestroyImmediate(face.SpawnedPresentation);
                
                if (face.Value)
                {
                    var textObject = new GameObject();
                    textObject.transform.SetParent(transform);
                    textObject.name = face.Value.DisplayName;
                    textObject.transform.position = face.Centroid + face.Normal.normalized *0.01f;
                    textObject.transform.rotation = Quaternion.LookRotation(face.Normal);
                    var textMesh = textObject.AddComponent<TextMeshPro>();

                    textMesh.text = face.Value.Text;
                    textMesh.fontSize = face.Value.FontSize;
                    textMesh.alignment = TextAlignmentOptions.Center;
                    face.SpawnedPresentation = textObject;
                }
            }
            
            EditorUtility.SetDirty(this);
        }

        private void OnDrawGizmos()
        {
            foreach (var face in diceFaces)
            {
                face.DrawGizmos();
            }
        }
    }
}