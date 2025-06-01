using UnityEngine;

namespace Startup.Models
{
    public sealed class Map
    {
        public GameObject Floor { get; }
        public GameObject[] Walls { get; }
        public GameObject Ceiling { get; }

        public Map(GameObject floor, GameObject[] walls, GameObject ceiling)
        {
            Floor = floor;
            Walls = walls;
            Ceiling = ceiling;
        }
    }
}