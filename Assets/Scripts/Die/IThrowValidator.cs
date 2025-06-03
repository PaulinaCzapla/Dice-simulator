using Die.Data;

namespace Die
{
    public interface IThrowValidator
    {
        public DieValue Validate(float accumulatedDistance, float accumulatedRotation);
    }
}