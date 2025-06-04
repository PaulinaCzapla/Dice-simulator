using Interactions;
using UnityEngine;

namespace DieSimulation.Data
{
    [CreateAssetMenu(fileName = "DraggableDieConfig", menuName = "ScriptableObjects/DraggableDieConfig", order = 0)]
    public sealed class DraggableDieConfig : DraggableConfig
    {
        [field: Header("Start drag")]
        [field: SerializeField]
        public Vector3 StartDragPosition { get; private set; }

        [field: Header("Drag")]
        [field: SerializeField]
        public float DragSmoothnessMultiplier { get; private set; }
        
        [field: SerializeField] 
        public float TorqueStrengthMultiplier { get; private set; }
        
        [field: SerializeField] 
        public float TorqueSmoothTime { get; private set; }
    }
}