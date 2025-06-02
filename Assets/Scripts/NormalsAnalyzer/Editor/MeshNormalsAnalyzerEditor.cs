using UnityEditor;
using UnityEngine;

namespace NormalsAnalyzer.Editor
{
    [CustomEditor(typeof(MeshNormalsAnalyzer))]
    public class MeshNormalsAnalyzerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var analyzer = (MeshNormalsAnalyzer) target;

            if (GUILayout.Button("Analyze Normals"))
            {
                analyzer.AnalyzeNormals();
                EditorUtility.SetDirty(analyzer);
            }
        }
    }
}