using UnityEngine;

namespace DieSimulation.Data
{
    [CreateAssetMenu(fileName = "DraggableConfig", menuName = "ScriptableObjects/DraggableConfig", order = 0)]
    public class DraggableConfig : ScriptableObject
    {
        [field: Header("Drop")]
        [field: SerializeField]
        public float DropForceMultiplier { get; private set; }
        
        [field: SerializeField] 
        public float DropAngularVelocityMultiplier { get; private set; }
    }
}