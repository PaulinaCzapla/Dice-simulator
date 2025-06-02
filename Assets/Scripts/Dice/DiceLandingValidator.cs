using System.Linq;
using Dice.Data;
using UnityEngine;

namespace Dice
{
    [RequireComponent(typeof(DiceValuesHolder))]
    public class DiceLandingValidator : MonoBehaviour, IThrowValidator
    {
        [SerializeField] private float minDistance = 0.5f;
        [SerializeField] private float minAngularMovement = 90f;
        [SerializeField] private float faceUpThreshold = 0.9f;

        private DiceValuesHolder _valuesHolder;

        private void Awake()
        {
            _valuesHolder = GetComponent<DiceValuesHolder>();
        }

        public DiceValue Validate(float accumulatedDistance, float accumulatedAngularMovement)
        {
            if (accumulatedDistance < minDistance || accumulatedAngularMovement < minAngularMovement)
            {
                return null;
            }

            Vector3 up = Vector3.up;
            var bestMatch = _valuesHolder.DiceFaces
                .Select(face => new {face, alignment = Vector3.Dot(transform.TransformDirection(face.Normal), up)})
                .OrderByDescending(x => x.alignment)
                .First();
            
            Debug.Log(bestMatch.alignment);
            if (bestMatch.alignment < faceUpThreshold)
            {
                return null;
            }
            
            return bestMatch.face.Value;
        }
    }
}