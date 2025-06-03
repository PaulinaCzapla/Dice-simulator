using Die.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Die
{
    [RequireComponent(typeof(DieValuesHolder), typeof(IThrowable))]
    public class DieController : MonoBehaviour
    {
        private DieValuesHolder _valuesHolder;
        private IThrowable _throwable;
        private ThrowTracker _throwTracker;
        private DieLandingValidator _dieLandingValidator;

        public UnityEvent<DieController> OnThrew { get; private set; } = new();
        public UnityEvent<DieValue, DieController> OnDieRollSuccess { get; private set; } = new();
        public UnityEvent<DieController> OnDieRollFailed { get; private set; } = new();
        public IThrowable Throwable => _throwable;

        private void Awake()
        {
            _throwable = GetComponent<IThrowable>();
            _valuesHolder = GetComponent<DieValuesHolder>();
        }

        private void OnEnable()
        {
            _throwable.OnThrew.AddListener(Threw);
        }

        private void OnDisable()
        {
            _throwable.OnThrew.AddListener(Threw);
        }

        private void FixedUpdate()
        {
            if (_throwTracker != null)
                _throwTracker.TrackUpdate();
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
            var resultValue = _dieLandingValidator.Validate(transform, accumulatedVelocity, accumulatedAngularVelocity);

            if (resultValue)
            {
                OnDieRollSuccess.Invoke(resultValue, this);
            }
            else
            {
                OnDieRollFailed.Invoke(this);
            }

            _throwTracker = null;
        }
    }
}