using UnityEngine;
using UnityEngine.Events;

namespace Die
{
    public interface IThrowable
    {
        public  UnityEvent<ITrackable>  OnThrew { get; }
        public void Throw(Vector3 velocity, Vector3 angularVelocity);
        public void PrepareForThrow();
    }
}