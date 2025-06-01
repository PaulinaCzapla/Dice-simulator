using System;
using DiceSystem;
using Startup.Data;
using UnityEngine;

namespace Startup
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject dicePrefab;
        [SerializeField] private Transform mapParent;
        [SerializeField] private LevelData levelData;

        private void Awake()
        {
            new MapBuilder(levelData.EnvironmentBounds, mapParent)
                .WithFloor(levelData.FloorPrefab)
                .WithWalls(levelData.WallPrefab)
                .WithCeiling(levelData.WallPrefab)
                .Build();
            
            var dice = Instantiate(dicePrefab, levelData.StartPosition, Quaternion.identity);

            if (dice.TryGetComponent<DiceBoundsLimiter>(out var diceBoundsLimiter))
            {
                diceBoundsLimiter.SetBounds(levelData.EnvironmentBounds);
            }
        }
    }
}
