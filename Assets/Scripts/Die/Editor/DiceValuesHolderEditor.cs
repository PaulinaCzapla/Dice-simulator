using UnityEditor;
using UnityEngine;

namespace Die.Editor
{
    [CustomEditor(typeof(DieValuesHolder))]
    public sealed class DiceValuesHolderEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var valuesHolder = (DieValuesHolder) target;

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