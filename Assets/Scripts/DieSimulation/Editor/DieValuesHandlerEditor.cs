using DieSimulation.Components;
using UnityEditor;
using UnityEngine;

namespace DieSimulation.Editor
{
    [CustomEditor(typeof(DieValuesHandler))]
    public sealed class DieValuesHandlerEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var valuesHolder = (DieValuesHandler) target;

            if (GUILayout.Button("Reimport normals list"))
            {
                valuesHolder.UpdateNormals();
                EditorUtility.SetDirty(valuesHolder);
            }
            
            if (GUILayout.Button("Generate presentation"))
            {
                valuesHolder.GeneratePresentation();
                EditorUtility.SetDirty(valuesHolder);
            }
        }
    }
}