using Die.Data;
using InputManagement;
using Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Die
{
    [RequireComponent(typeof(Rigidbody))]
    public class DieThrowable : MonoBehaviour, IThrowable,IDraggable, ITrackable
    {
        [SerializeField] private DraggableConfig config;
        [SerializeField] private DieBoundsLimiter boundsLimiter;

        private const float MIN_MAGNITUDE_FOR_TORQUE = 30f;
        private const float MAX_MAGNITUDE_FOR_TORQUE = 60f;
        private const float DROP_Y_VELOCITY_MULTIPLIER = 0.35f;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Rigidbody _rb;
        private Vector2 _lastVelocity;
        private Vector3 _smoothedTorque = Vector3.zero;
        private Vector3 _torqueVelocity = Vector3.zero;
        private UnityEvent<ITrackable> _onThrowed = new();
        
        public UnityEvent<ITrackable> OnThrew => _onThrowed;
        public Vector3 Position => _rb.position;
        public Quaternion Rotation => _rb.rotation;
        public Vector3 Velocity => _rb.velocity;
        public Vector3 AngularVelocity => _rb.angularVelocity;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _initialPosition = _rb.position;
            _initialRotation = _rb.rotation;
        }

        public void Throw(Vector3 velocity, Vector3 angularVelocity)
        {
            _rb.velocity = velocity;
            _rb.angularVelocity = angularVelocity;
            _onThrowed.Invoke(this);
        }

        public void StartDrag()
        {
            CursorController.Hide();
            _rb.useGravity = false;
            _rb.MoveRotation(_initialRotation);
        }

        public void Drop()
        {
            CursorController.Show();
            _rb.useGravity = true;
            var velocity = _rb.velocity * config.DropForceMultiplier + new Vector3(0, _rb.velocity.z * DROP_Y_VELOCITY_MULTIPLIER, 0);
            _rb.angularVelocity = Vector3.zero;
            var angularVelocity = _rb.velocity * config.DropAngularVelocityMultiplier;
            
            Throw(velocity, angularVelocity);
        }

        public void Drag(Vector2 screenPosition, Vector2 inputVelocity)
        {
            Vector3 screenPos = new Vector3(screenPosition.x, screenPosition.y,
                Camera.main.WorldToScreenPoint(_rb.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPos);

            Vector3 desiredPosition = new Vector3(worldPosition.x, config.StartDragPosition.y, worldPosition.z);

            if (boundsLimiter.IsLimitedByWalls(desiredPosition, out var clampedPosition))
            {
                desiredPosition = clampedPosition;
            }

            Vector3 toTarget = desiredPosition - _rb.position;
            _rb.velocity = toTarget * config.DragSmoothnessMultiplier;
            
            if (inputVelocity.magnitude > MIN_MAGNITUDE_FOR_TORQUE)
            {
                Vector3 targetTorque = new Vector3(-inputVelocity.y, -inputVelocity.x, inputVelocity.x);

                if (inputVelocity.magnitude < MAX_MAGNITUDE_FOR_TORQUE)
                    targetTorque = config.TorqueStrengthMultiplier * (targetTorque.normalized *
                                                                      (Mathf.Max(targetTorque.magnitude - MIN_MAGNITUDE_FOR_TORQUE, 0)));
                else
                    targetTorque = config.TorqueStrengthMultiplier *
                                   (_rb.GetAccumulatedTorque().normalized * (targetTorque.magnitude));

                _smoothedTorque =
                    Vector3.SmoothDamp(_smoothedTorque, targetTorque, ref _torqueVelocity, config.TorqueSmoothTime);
         
                _rb.AddTorque(_smoothedTorque, ForceMode.Acceleration);
            }
        }

        public void PrepareForThrow()
        {
            _rb.position = _initialPosition;
            _rb.rotation = _initialRotation;
        }
    }
}