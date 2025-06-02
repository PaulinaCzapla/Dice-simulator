using UnityEditor;
using UnityEngine;

namespace Dice.Editor
{
    [CustomEditor(typeof(DiceValuesHolder))]
    public class DiceValuesHolderEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var valuesHolder = (DiceValuesHolder) target;

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