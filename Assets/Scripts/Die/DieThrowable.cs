using Die.Data;
using InputManagement;
using Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Die
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class DieThrowable : MonoBehaviour, IThrowable, IDraggable, ITrackable
    {
        private const float MIN_MAGNITUDE_FOR_TORQUE = 30f;
        private const float MAX_MAGNITUDE_FOR_TORQUE = 60f;
        private const float DROP_Y_VELOCITY_MULTIPLIER = 0.35f;

        [SerializeField] private DraggableConfig config;
        [SerializeField] private DieBoundsLimiter boundsLimiter;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Rigidbody _rigidbody;
        private Vector2 _lastVelocity;
        private Vector3 _smoothedTorque = Vector3.zero;
        private Vector3 _torqueVelocity = Vector3.zero;

        public UnityEvent<ITrackable> OnThrew { get; } = new();

        public Vector3 Position => _rigidbody.position;
        public Quaternion Rotation => _rigidbody.rotation;
        public Vector3 Velocity => _rigidbody.velocity;
        public Vector3 AngularVelocity => _rigidbody.angularVelocity;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _initialPosition = _rigidbody.position;
            _initialRotation = _rigidbody.rotation;
        }

        public void Throw(Vector3 velocity, Vector3 angularVelocity)
        {
            _rigidbody.velocity = velocity;
            _rigidbody.angularVelocity = angularVelocity;
            OnThrew.Invoke(this);
        }

        public void StartDrag()
        {
            _rigidbody.useGravity = false;
            _rigidbody.MoveRotation(_initialRotation);
        }

        public void Drop()
        {
            _rigidbody.useGravity = true;
            var currentVelocity = _rigidbody.velocity;
            var velocity = currentVelocity * config.DropForceMultiplier + 
                           new Vector3(0, currentVelocity.z * DROP_Y_VELOCITY_MULTIPLIER, 0);
            _rigidbody.angularVelocity = Vector3.zero;
            var angularVelocity = _rigidbody.velocity * config.DropAngularVelocityMultiplier;
            
            Throw(velocity, angularVelocity);
        }

        public void Drag(Vector2 screenPosition, Vector2 inputVelocity)
        {
            var cameraMain = Camera.main;
            
            var screenPos = new Vector3(screenPosition.x, screenPosition.y,
                cameraMain.WorldToScreenPoint(_rigidbody.position).z);
            var worldPosition = cameraMain.ScreenToWorldPoint(screenPos);

            var desiredPosition = new Vector3(worldPosition.x, config.StartDragPosition.y, worldPosition.z);

            if (boundsLimiter.IsLimitedByWalls(desiredPosition, out var clampedPosition))
            {
                desiredPosition = clampedPosition;
            }

            var toTarget = desiredPosition - _rigidbody.position;
            _rigidbody.velocity = toTarget * config.DragSmoothnessMultiplier;

            if (!(inputVelocity.magnitude > MIN_MAGNITUDE_FOR_TORQUE))
            {
                return;
            }
            
            var targetTorque = new Vector3(-inputVelocity.y, -inputVelocity.x, inputVelocity.x);

            if (inputVelocity.magnitude < MAX_MAGNITUDE_FOR_TORQUE)
                targetTorque = config.TorqueStrengthMultiplier * 
                               (targetTorque.normalized * Mathf.Max(targetTorque.magnitude - MIN_MAGNITUDE_FOR_TORQUE, 0));
            else
                targetTorque = config.TorqueStrengthMultiplier *
                               (_rigidbody.GetAccumulatedTorque().normalized * targetTorque.magnitude);

            _smoothedTorque = Vector3.SmoothDamp(_smoothedTorque, targetTorque, 
                ref _torqueVelocity, config.TorqueSmoothTime);
         
            _rigidbody.AddTorque(_smoothedTorque, ForceMode.Acceleration);
        }

        public void PrepareForThrow()
        {
            _rigidbody.position = _initialPosition;
            _rigidbody.rotation = _initialRotation;
        }
    }
}