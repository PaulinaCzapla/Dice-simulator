using System.Collections.Generic;
using System.Linq;
using DieSimulation.Interfaces;
using DieSimulation.Models;
using UnityEngine;
using UnityEngine.Events;

namespace DieSimulation.Components
{
    [RequireComponent(typeof(DieValuesHandler))]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class DieResolvable : MonoBehaviour, IResolvable
    {
        private const float MIN_DISTANCE = 0.5f;
        private const float MIN_ANGULAR_VELOCITY = 90f;
        private const float FACE_UP_TRESHOLD = 0.85f;
        private const float STOP_THRESHOLD = 0.05f * 0.05f;
        private const float MIN_STOP_TIME = 0.5f;
        private const float MAX_TIME = 20f;
        
        private float _accumulatedDistance;
        private float _accumulatedRotation;

        private IReadOnlyCollection<DieFace> _dieFaces;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;
        private Rigidbody _rigidbody;
        private float _stillTime;
        private bool _tracking;
        private float _time;

        public UnityEvent<int?> OnResolved { get; } = new();

        private void Awake()
        {
            var dieValuesHandler = GetComponent<DieValuesHandler>();
            _dieFaces = dieValuesHandler.DieFaces;
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void StartResolving()
        {
            _accumulatedDistance = 0f;
            _accumulatedRotation = 0f;
            _stillTime = 0f;
            _time = 0;
            _tracking = true;

            _lastPosition = _rigidbody.position;
            _lastRotation = _rigidbody.rotation;
        }

        private void FixedUpdate()
        {
            if (!_tracking)
            {
                return;
            }

            _time += Time.deltaTime;

            if (_time > MAX_TIME)
            {
                OnResolved.Invoke(null);
            }

            var currentPos = _rigidbody.position;
            var currentRot = _rigidbody.rotation;

            _accumulatedDistance += Vector3.Distance(currentPos, _lastPosition);
            _accumulatedRotation += Quaternion.Angle(currentRot, _lastRotation);

            _lastPosition = currentPos;
            _lastRotation = currentRot;

            var isStill = _rigidbody.velocity.sqrMagnitude < STOP_THRESHOLD &&
                          _rigidbody.angularVelocity.sqrMagnitude < STOP_THRESHOLD;

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
                ThrowFinished(_accumulatedDistance, _accumulatedRotation);
            }
        }

        private void ThrowFinished(float accumulatedVelocity, float accumulatedAngularVelocity)
        {
            var result = Validate(accumulatedVelocity, accumulatedAngularVelocity);

            OnResolved.Invoke(result);

            _tracking = false;
        }

        private int? Validate(float accumulatedDistance, float accumulatedAngularMovement)
        {
            if (accumulatedDistance < MIN_DISTANCE || accumulatedAngularMovement < MIN_ANGULAR_VELOCITY)
            {
                return null;
            }

            var bestMatch = _dieFaces
                .Select(face => new
                {
                    face, alignment =
                        Vector3.Dot(transform.TransformDirection(face.Normal), Vector3.up)
                })
                .OrderByDescending(x => x.alignment)
                .First();

            if (bestMatch.alignment < FACE_UP_TRESHOLD)
            {
                return null;
            }

            return bestMatch.face.FaceValue.Value;
        }
    }
}