using UnityEngine;

namespace Die
{
    public interface ITrackable
    {
       public Vector3 Position { get; }
       public Quaternion Rotation { get; }
       public Vector3 Velocity { get; }
       public Vector3 AngularVelocity { get; }
    }
}