using UnityEngine;

namespace Utilities
{
    public static class Vector3Utils
    {
        public static Vector3 Clamp(this Vector3 vector, Vector3 vectorMin, Vector3 vectorMax)
        {
            return new Vector3(
                Mathf.Clamp(vector.x, vectorMin.x, vectorMax.x),
                Mathf.Clamp(vector.y, vectorMin.y, vectorMax.y),
                Mathf.Clamp(vector.z, vectorMin.z, vectorMax.z)
            );
        }

        public static Vector3 Subtract(this Vector3 vector, float value)
        {
            return new Vector3(vector.x - value, vector.y - value, vector.z - value);
        }

        public static float CalculateTriangleArea(Vector3 vector0, Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Cross(vector1 - vector0, vector2 - vector0).magnitude * 0.5f;
        }
    }
}