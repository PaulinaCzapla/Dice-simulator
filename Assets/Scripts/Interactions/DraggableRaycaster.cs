using InputManagement;
using UnityEngine;

namespace Interactions
{
    public class DraggableRaycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask raycastMask;
        private IDraggable _currentDraggable;

        private InputHandler _inputHandler;

        private void Awake()
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            if (_inputHandler == null)
                enabled = false;
        }

        private void FixedUpdate()
        {
            if (_currentDraggable != null)
                _currentDraggable.Drag(_inputHandler.MouseScreenPosition, _inputHandler.MouseVelocity);
        }

        private void OnEnable()
        {
            _inputHandler.OnLeftMouseDown.AddListener(OnLeftMouseDown);
            _inputHandler.OnLeftMouseUp.AddListener(OnLeftMouseUp);
        }

        private void OnDisable()
        {
            _inputHandler.OnLeftMouseDown.RemoveListener(OnLeftMouseDown);
            _inputHandler.OnLeftMouseUp.RemoveListener(OnLeftMouseUp);
        }

        private void OnLeftMouseDown()
        {
            var ray = Camera.main.ScreenPointToRay(_inputHandler.MouseScreenPosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, raycastMask) &&
                hit.collider.TryGetComponent(out IDraggable selectable))
            {
                selectable.StartDrag();
                _currentDraggable = selectable;
            }
        }

        private void OnLeftMouseUp()
        {
            if (_currentDraggable != null)
            {
                _currentDraggable.Drop();
                _currentDraggable = null;
            }
        }
    }
}