using UnityEngine;
using UnityEngine.Events;

namespace Dice
{
    public class ThrowTracker : MonoBehaviour
    {
        [SerializeField] private float stopThreshold = 0.05f;
        [SerializeField] private float minStopTime = 0.5f;

        private ITrackable _trackable;

        private Vector3 _lastPosition;
        private Quaternion _lastRotation;

        private float _accumulatedDistance;
        private float _accumulatedRotation;
        private float _stillTime;
        private bool _tracking;
        
        public UnityEvent<float, float> OnThrowFinished = new(); // totalDistance, totalRotation

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

        private void FixedUpdate()
        {
            if (!_tracking || _trackable == null)
                return;

            var currentPos = _trackable.Position;
            var currentRot = _trackable.Rotation;

            _accumulatedDistance += Vector3.Distance(currentPos, _lastPosition);
            _accumulatedRotation += Quaternion.Angle(currentRot, _lastRotation);

            _lastPosition = currentPos;
            _lastRotation = currentRot;

            bool isStill = _trackable.Velocity.magnitude < stopThreshold &&
                           _trackable.AngularVelocity.magnitude < stopThreshold;

            if (isStill)
                _stillTime += Time.deltaTime;
            else
                _stillTime = 0f;

            if (_stillTime >= minStopTime)
            {
                _tracking = false;
                Debug.Log("stopped "+ _accumulatedDistance +"  "+_accumulatedRotation);
                OnThrowFinished.Invoke(_accumulatedDistance, _accumulatedRotation);
            }
        }
    }
}