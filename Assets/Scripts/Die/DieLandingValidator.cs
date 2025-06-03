using System.Linq;
using Die.Data;
using UnityEngine;

namespace Die
{
    public sealed class DieLandingValidator
    {
        private const float MIN_DISTANCE = 0.5f;
        private const float MIN_ANGULAR_VELOCITY = 90f;
        private const  float FACE_UP_TRESHOLD = 0.85f;

        private readonly DieValuesHolder _valuesHolder;

        public DieLandingValidator(DieValuesHolder holder)
        {
            _valuesHolder = holder;
        }

        public int? Validate(Transform transform, float accumulatedDistance, float accumulatedAngularMovement)
        {
            if (accumulatedDistance < MIN_DISTANCE || accumulatedAngularMovement < MIN_ANGULAR_VELOCITY)
            {
                return null;
            }
            
            var bestMatch = _valuesHolder.DiceFaces
                .Select(face => new {face, alignment = 
                    Vector3.Dot(transform.TransformDirection(face.Normal), Vector3.up)})
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