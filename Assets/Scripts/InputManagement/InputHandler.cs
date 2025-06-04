using UnityEngine;
using UnityEngine.Events;

namespace InputManagement
{
    public sealed class InputHandler : MonoBehaviour
    {
        private const string HorizontalAxisName = "Mouse X";
        private const string VerticalAxisName = "Mouse Y";

        public static bool InputBlocked { get; set; }

        public UnityEvent OnLeftMouseDown { get; } = new();
        public UnityEvent OnLeftMouseUp { get; } = new();

        public Vector2 MouseScreenPosition => Input.mousePosition;
        public Vector2 MouseVelocity { get; private set; }

        private void Update()
        {
            if (InputBlocked)
            {
                return;
            }

            MouseVelocity = new Vector2(Input.GetAxis(HorizontalAxisName) / Time.deltaTime,
                Input.GetAxis(VerticalAxisName) / Time.deltaTime);

            if (Input.GetMouseButtonDown(0))
            {
                OnLeftMouseDown?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnLeftMouseUp?.Invoke();
            }
        }
    }
}