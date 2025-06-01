using UnityEngine;

namespace Startup.Data
{
    [CreateAssetMenu(fileName = nameof(LevelData), menuName = "Level/LevelData", order = 1)]
    public sealed class LevelData : ScriptableObject
    {
        [field: SerializeField] public Bounds EnvironmentBounds { get; }
        [field: SerializeField] public Vector3 StartPosition { get; }
        [field: SerializeField] public GameObject FloorPrefab { get;  }
        [field: SerializeField] public GameObject WallPrefab { get; }
    }
}