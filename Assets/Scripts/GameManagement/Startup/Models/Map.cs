using DieSimulation.Interfaces;

namespace GameManagement.Startup.Models
{
    public sealed class Map
    {
        public IDieProvider[] Dice { get; }

        public Map(IDieProvider[] dice)
        {
            Dice = dice;
        }
    }
}