using UnityEngine;
using Utilities;

namespace Die
{
    public sealed class DieBoundsLimiter : MonoBehaviour
    {
        [SerializeField] private float boundsOffset = 1f;

        private Bounds _limitBounds;

        public void SetBounds(Bounds environmentBounds)
        {
            var environmentBoundsSize = environmentBounds.size;

            _limitBounds = new Bounds(environmentBounds.center, environmentBoundsSize.Subtract(boundsOffset));
        }

        public bool IsLimitedByWalls(Vector3 nextPosition, out Vector3 clampedPosition)
        {
            if (!_limitBounds.Contains(nextPosition))
            {
                clampedPosition = nextPosition.Clamp(_limitBounds.min, _limitBounds.max);
                return true;
            }

            clampedPosition = Vector3.zero;
            return false;
        }
    }
}