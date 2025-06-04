using Interactions;
using UnityEngine;

namespace DieSimulation.Data
{
    [CreateAssetMenu(fileName = "DraggableConfig", menuName = "ScriptableObjects/DraggableConfig", order = 0)]
    public sealed class DraggableConfig : ScriptableObject
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
        
        [field: Header("Drop")]
        [field: SerializeField]
        public float DropForceMultiplier { get; private set; }
        
        [field: SerializeField] 
        public float DropAngularVelocityMultiplier { get; private set; }
    }
}