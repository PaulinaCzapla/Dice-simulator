using InputManagement;
using UnityEngine;

namespace Interactions
{
    public sealed class InputRaycaster : MonoBehaviour
    {
        [SerializeField] 
        private LayerMask raycastMask;

        private IDraggable _currentDraggable;
        private InputHandler _inputHandler;
        private Camera _mainCamera;

        private void Awake()
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            _mainCamera = Camera.main;
            if (_inputHandler == null)
            {
                enabled = false;
            }
        }

        private void FixedUpdate()
        {
            _currentDraggable?.Drag(_inputHandler.MouseScreenPosition, _inputHandler.MouseVelocity);
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
            var ray = _mainCamera.ScreenPointToRay(_inputHandler.MouseScreenPosition);

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, raycastMask))
            {
                return;
            }

            if (hit.collider.TryGetComponent(out IDraggable draggable))
            {
                draggable.StartDrag();
                _currentDraggable = draggable;
            }
        }

        private void OnLeftMouseUp()
        {
            _currentDraggable?.Drop();

            _currentDraggable = null;
        }
    }
}