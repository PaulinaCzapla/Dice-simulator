using DieSimulation.Data;
using DieSimulation.Interfaces;
using Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace DieSimulation
{
    public sealed class DieThrowable : IThrowable
    {
        private const float DROP_Y_VELOCITY_MULTIPLIER = 0.35f;

        private readonly DraggableConfig _draggableDieConfig;
        private readonly Vector3 _initialPosition;
        private readonly Quaternion _initialRotation;
        private readonly Vector2 _lastVelocity;
        private readonly Rigidbody _rigidbody;
        
        public UnityEvent OnThrew { get; } = new();

        public DieThrowable(Rigidbody rigidbody, DraggableConfig draggableDieConfig)
        {
            _rigidbody = rigidbody;
            _draggableDieConfig = draggableDieConfig;
            _initialPosition = _rigidbody.position;
            _initialRotation = _rigidbody.rotation;
        }
        
        public void ThrowSingle(Vector3 velocity, Vector3 angularVelocity)
        {
            _rigidbody.velocity = velocity;
            _rigidbody.angularVelocity = angularVelocity;

            OnThrew?.Invoke();
        }

        public void ThrowAdditive()
        {
            var currentVelocity = _rigidbody.velocity;
            var velocity = currentVelocity * _draggableDieConfig.DropForceMultiplier +
                           new Vector3(0, currentVelocity.z * DROP_Y_VELOCITY_MULTIPLIER, 0);
            var angularVelocity = currentVelocity * _draggableDieConfig.DropAngularVelocityMultiplier;

            ThrowSingle(velocity, angularVelocity);
        }

        public void Reset()
        {
            _rigidbody.position = _initialPosition;
            _rigidbody.rotation = _initialRotation;
        }
    }
}