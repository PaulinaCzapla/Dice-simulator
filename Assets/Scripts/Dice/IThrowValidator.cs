using Dice.Data;

namespace Dice
{
    public interface IThrowValidator
    {
        public DiceValue Validate(float accumulatedDistance, float accumulatedRotation);
    }
}