using UnityEngine;

namespace Utilities
{
    public static class BoundsUtils
    {
        public static Vector3 RandomPointInBounds(this Bounds bounds, float epsilon = float.Epsilon)
        {
            return new Vector3(
                Random.Range(bounds.min.x + epsilon, bounds.max.x - epsilon),
                Random.Range(bounds.min.y + epsilon, bounds.max.y - epsilon),
                Random.Range(bounds.min.z + epsilon, bounds.max.z - epsilon)
            );
        }
    }
}