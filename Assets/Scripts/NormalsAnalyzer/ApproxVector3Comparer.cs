using System.Collections.Generic;
using UnityEngine;

namespace NormalsAnalyzer
{
    public class ApproxVector3Comparer : IEqualityComparer<Vector3>
    {
        private readonly float tolerance;

        public ApproxVector3Comparer(float tolerance)
        {
            this.tolerance = tolerance;
        }

        public bool Equals(Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < tolerance * tolerance;
        }

        public int GetHashCode(Vector3 obj)
        {
            return base.GetHashCode();
        }
    }
}