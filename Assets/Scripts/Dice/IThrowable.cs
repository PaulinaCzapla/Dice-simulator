using UnityEngine;

namespace Dice
{
    public interface IThrowable
    {
        public void Throw(Vector3 velocity, Vector3 angularVelocity);
    }
}