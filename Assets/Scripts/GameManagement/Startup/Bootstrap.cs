using System.Collections.Generic;
using Die;
using GameManagement.Startup.Data;
using UnityEngine;

namespace GameManagement.Startup
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private List<GameObject> dicePrefabs;
        [SerializeField] private Transform mapParent;
        [SerializeField] private LevelData levelData;

        public void BuildMap()
        {
            new MapBuilder(levelData.EnvironmentBounds, mapParent)
                .WithFloor(levelData.FloorPrefab)
                .WithWalls(levelData.WallPrefab)
                .WithCeiling(levelData.WallPrefab)
                .Build();

            Vector3 offset = Vector3.zero;
            foreach (var prefab in dicePrefabs)
            {
                var dice = Instantiate(prefab, levelData.StartPosition + offset, Quaternion.identity);
                if (dice.TryGetComponent<DieBoundsLimiter>(out var diceBoundsLimiter))
                                diceBoundsLimiter.SetBounds(levelData.EnvironmentBounds);
                offset += new Vector3(2, 0, 2);
            }
        }
    }
}