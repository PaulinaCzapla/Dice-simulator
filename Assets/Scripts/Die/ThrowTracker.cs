using UnityEngine;
using UnityEngine.Events;

namespace Die
{
    public sealed class ThrowTracker
    {
        private const float STOP_THRESHOLD = 0.05f;
        private const float MIN_STOP_TIME = 0.5f;

        private ITrackable _trackable;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;
        private float _accumulatedDistance;
        private float _accumulatedRotation;
        private float _stillTime;
        private bool _tracking;
        
        public readonly UnityEvent<float, float> OnThrowFinished = new();

        public void StartTracking(ITrackable trackable)
        {
            _trackable = trackable;
            _accumulatedDistance = 0f;
            _accumulatedRotation = 0f;
            _stillTime = 0f;
            _tracking = true;

            _lastPosition = trackable.Position;
            _lastRotation = trackable.Rotation;
        }

        public void TrackUpdate()
        {
            if (!_tracking || _trackable == null)
            {
                return;
            }

            var currentPos = _trackable.Position;
            var currentRot = _trackable.Rotation;

            _accumulatedDistance += Vector3.Distance(currentPos, _lastPosition);
            _accumulatedRotation += Quaternion.Angle(currentRot, _lastRotation);

            _lastPosition = currentPos;
            _lastRotation = currentRot;

            var isStill = _trackable.Velocity.magnitude < STOP_THRESHOLD &&
                           _trackable.AngularVelocity.magnitude < STOP_THRESHOLD;

            if (isStill)
            {
                _stillTime += Time.deltaTime;
            }
            else
            {
                _stillTime = 0f;
            }

            if (_stillTime >= MIN_STOP_TIME)
            {
                _tracking = false;
                OnThrowFinished.Invoke(_accumulatedDistance, _accumulatedRotation);
            }
        }
    }
}