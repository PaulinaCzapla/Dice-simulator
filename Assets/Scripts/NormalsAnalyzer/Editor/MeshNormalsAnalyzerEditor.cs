using NormalsAnalyzer;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshNormalsAnalyzer))]
public class MeshNormalsAnalyzerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeshNormalsAnalyzer analyzer = (MeshNormalsAnalyzer)target;

        if (GUILayout.Button("Analyze Normals"))
        {
            analyzer.AnalyzeNormals();
            EditorUtility.SetDirty(analyzer);
        }
    }
}