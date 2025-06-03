using Die.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Die
{
    [RequireComponent(typeof(DieValuesHolder), typeof(IThrowable))]
    public sealed class DieController : MonoBehaviour
    {
        private DieValuesHolder _valuesHolder;
        private ThrowTracker _throwTracker;
        private DieLandingValidator _dieLandingValidator;

        public UnityEvent<DieController> OnThrew { get; } = new();
        public UnityEvent<int, DieController> OnDieRollSuccess { get; } = new();
        public UnityEvent<DieController> OnDieRollFailed { get; } = new();
        public IThrowable Throwable { get; private set; }

        private void Awake()
        {
            Throwable = GetComponent<IThrowable>();
            _valuesHolder = GetComponent<DieValuesHolder>();
        }

        private void OnEnable()
        {
            Throwable.OnThrew.AddListener(Threw);
        }

        private void OnDisable()
        {
            Throwable.OnThrew.AddListener(Threw);
        }

        private void FixedUpdate()
        {
            _throwTracker?.TrackUpdate();
        }

        private void Threw(ITrackable trackable)
        {
            OnThrew.Invoke(this);
            _dieLandingValidator = new DieLandingValidator(_valuesHolder);
            _throwTracker = new ThrowTracker();
            _throwTracker.StartTracking(trackable);
            _throwTracker.OnThrowFinished.AddListener(ThrowFinished);
        }

        private void ThrowFinished(float accumulatedVelocity, float accumulatedAngularVelocity)
        {
            var resultValue = _dieLandingValidator.Validate(transform, 
                accumulatedVelocity, accumulatedAngularVelocity);

            if (resultValue.HasValue)
            {
                OnDieRollSuccess.Invoke(resultValue.Value, this);
            }
            else
            {
                OnDieRollFailed.Invoke(this);
            }

            _throwTracker = null;
        }
    }
}