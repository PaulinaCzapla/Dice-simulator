using UnityEngine;
using UnityEngine.Events;

namespace InputManagement
{
    public class InputHandler : MonoBehaviour
    {
        public static bool InputBlocked { get; set; }
        
        private Vector2 _previousMousePosition;
        public UnityEvent OnLeftMouseDown { get; } = new();
        public UnityEvent OnLeftMouseUp { get; } = new();
        public UnityEvent OnLeftMouse { get; } = new();
        public Vector2 MouseScreenPosition { get; private set; }
        public Vector2 DeltaMove { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector2 Tendency { get; private set; }
        public Vector2 MouseVelocity { get; private set; }

        private void Update()
        {
            if(InputBlocked)
                return;
            
            MouseScreenPosition = Input.mousePosition;
            DeltaMove = MouseScreenPosition - _previousMousePosition;
            Direction = DeltaMove.normalized;

            if (Direction != Vector2.zero) Tendency = Direction;

            MouseVelocity = new Vector2(Input.GetAxis("Mouse X") / Time.deltaTime,
                Input.GetAxis("Mouse Y") / Time.deltaTime);
            _previousMousePosition = MouseScreenPosition;

            if (Input.GetMouseButtonDown(0))
                OnLeftMouseDown?.Invoke();

            if (Input.GetMouseButtonUp(0))
                OnLeftMouseUp?.Invoke();

            if (Input.GetMouseButton(0))
                OnLeftMouse?.Invoke();
        }
    }
}