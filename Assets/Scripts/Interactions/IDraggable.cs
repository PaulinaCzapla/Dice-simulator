using UnityEngine;

namespace Interactions
{
    public interface IDraggable
    {
        public void StartDrag();
        public void Drag(Vector2 screenPosition, Vector2 velocity);
        public void Drop();
    }
}