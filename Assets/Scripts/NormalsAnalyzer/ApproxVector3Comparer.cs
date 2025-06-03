using System.Collections.Generic;
using UnityEngine;

namespace NormalsAnalyzer
{
    public sealed class ApproxVector3Comparer : IEqualityComparer<Vector3>
    {
        private readonly float _tolerance;

        public ApproxVector3Comparer(float tolerance)
        {
            _tolerance = tolerance;
        }

        public bool Equals(Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < _tolerance * _tolerance;
        }

        public int GetHashCode(Vector3 obj)
        {
            return base.GetHashCode();
        }
    }
}