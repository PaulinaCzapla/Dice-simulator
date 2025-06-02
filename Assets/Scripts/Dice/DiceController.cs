using System;
using Dice.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Dice
{
    [RequireComponent(typeof(ThrowTracker), typeof(DiceLandingValidator))]
    public class DiceController : MonoBehaviour
    {
        private ThrowTracker _throwTracker;
        private DiceLandingValidator _diceLandingValidator;
        
        public UnityEvent<DiceValue> OnDiceRollSuccess { get; private set; } = new();
        public UnityEvent OnDiceRollFailed { get; private set; } = new();
        
        private void Awake()
        {
            _throwTracker = GetComponent<ThrowTracker>();
            _diceLandingValidator = GetComponent<DiceLandingValidator>();
        }

        private void OnEnable()
        {
            _throwTracker.OnThrowFinished.AddListener(ThrowFinished);
        }

        private void OnDisable()
        {
            _throwTracker.OnThrowFinished.RemoveListener(ThrowFinished);
        }

        private void ThrowFinished(float accumulatedVelocity, float accumulatedAngularVelocity)
        {
            var resultValue = _diceLandingValidator.Validate(accumulatedVelocity, accumulatedAngularVelocity);

           if (resultValue)
           {
               Debug.Log("success " + resultValue.DisplayName);
               OnDiceRollSuccess.Invoke(resultValue);
           }
           else
           {
               Debug.Log("fail ");
               OnDiceRollFailed.Invoke();
           }
        }
    }
}