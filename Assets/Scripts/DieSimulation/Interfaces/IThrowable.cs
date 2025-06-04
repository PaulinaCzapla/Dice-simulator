using UnityEngine;
using UnityEngine.Events;

namespace DieSimulation.Interfaces
{
    public interface IThrowable
    {
        public UnityEvent OnThrew { get; }
        public void ThrowSingle(Vector3 velocity, Vector3 angularVelocity);
        public void ThrowAdditive();
        public void Reset();
    }
}