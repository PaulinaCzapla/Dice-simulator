using DieSimulation.Data;
using Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace DieSimulation.Components
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(DieDragLimiter))]
    public sealed class DieDraggable : MonoBehaviour, IDraggable
    {
        private const float MIN_MAGNITUDE_FOR_TORQUE = 30f;
        private const float MAX_MAGNITUDE_FOR_TORQUE = 60f;

        [SerializeField] 
        private DraggableDieConfig dieConfig;

        private DieDragLimiter _dieDragLimiter;
        private Quaternion _initialRotation;
        private Vector2 _lastVelocity;
        private Camera _mainCamera;
        private Rigidbody _rigidbody;
        private Vector3 _smoothedTorque = Vector3.zero;
        private Vector3 _torqueVelocity = Vector3.zero;
        
        public UnityEvent OnDragEnded { get; } = new();

        private void Awake()
        {
            _dieDragLimiter = GetComponent<DieDragLimiter>();
            _rigidbody = GetComponent<Rigidbody>();
            _initialRotation = _rigidbody.rotation;
            _mainCamera = Camera.main;
        }
        
        public DraggableConfig GetConfig()
        {
            return dieConfig;
        }

        public void StartDrag()
        {
            _rigidbody.useGravity = false;
            _rigidbody.MoveRotation(_initialRotation);
        }

        public void Drop()
        {
            _rigidbody.useGravity = true;
            OnDragEnded?.Invoke();
        }

        public void Drag(Vector2 screenPosition, Vector2 inputVelocity)
        {
            if (inputVelocity.magnitude > MIN_MAGNITUDE_FOR_TORQUE)
            {
                SetTorque(inputVelocity);
            }

            var screenPos = new Vector3(screenPosition.x, screenPosition.y,
                _mainCamera.WorldToScreenPoint(_rigidbody.position).z);
            var worldPosition = _mainCamera.ScreenToWorldPoint(screenPos);

            var desiredPosition = new Vector3(worldPosition.x, dieConfig.StartDragPosition.y, worldPosition.z);

            if (_dieDragLimiter.IsLimitedByWalls(desiredPosition, out var clampedPosition))
            {
                desiredPosition = clampedPosition;
            }

            var toTarget = desiredPosition - _rigidbody.position;
            _rigidbody.velocity = toTarget * dieConfig.DragSmoothnessMultiplier;
        }

        private void SetTorque(Vector2 inputVelocity)
        {
            var targetTorque = new Vector3(-inputVelocity.y, -inputVelocity.x, inputVelocity.x);

            if (inputVelocity.magnitude < MAX_MAGNITUDE_FOR_TORQUE)
            {
                targetTorque = dieConfig.TorqueStrengthMultiplier *
                            (targetTorque.normalized *
                             Mathf.Max(targetTorque.magnitude - MIN_MAGNITUDE_FOR_TORQUE, 0));
            }
            else
            {
                targetTorque = dieConfig.TorqueStrengthMultiplier *
                            (_rigidbody.GetAccumulatedTorque().normalized * targetTorque.magnitude);
            }

            _smoothedTorque = Vector3.SmoothDamp(_smoothedTorque, targetTorque,
                ref _torqueVelocity, dieConfig.TorqueSmoothTime);

            _rigidbody.AddTorque(_smoothedTorque, ForceMode.Acceleration);
        }
    }
}