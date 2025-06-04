using DieSimulation.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions
{
    public interface IDraggable
    {
        public UnityEvent OnDragEnded { get; }
        public DraggableConfig GetConfig();
        public void StartDrag();
        public void Drop();
        public void Drag(Vector2 screenPosition, Vector2 velocity);
    }
}