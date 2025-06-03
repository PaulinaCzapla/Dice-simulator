using UnityEngine;

namespace Die.Data
{
    [CreateAssetMenu(fileName = "DiceDragConfig", menuName = "ScriptableObjects/DiceDragConfig", order = 0)]
    public class DraggableConfig : ScriptableObject
    {
        [field: Header("Start drag")]
        [field: SerializeField] public Vector3 StartDragPosition { get; private set; }

        [field: Header("Drag")]
        [field: SerializeField] public float DragSmoothnessMultiplier { get; private set; }

        [field: SerializeField] public float TorqueStrengthMultiplier { get; private set; }
        [field: SerializeField] public float TorqueSmoothTime { get; private set; }

        [field: Header("Drop")]
        [field: SerializeField] public float DropForceMultiplier { get; private set; }

        [field: SerializeField] public float DropAngularVelocityMultiplier { get; private set; }
    }
}