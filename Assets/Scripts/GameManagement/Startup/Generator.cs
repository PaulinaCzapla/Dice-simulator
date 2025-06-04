using System.Collections.Generic;
using DieSimulation.Components;
using GameManagement.Startup.Data;
using GameManagement.Startup.Models;
using UnityEngine;

namespace GameManagement.Startup
{
    public sealed class Generator : MonoBehaviour
    {
        [SerializeField] private List<DieController> dicePrefabs;
        [SerializeField] private Transform mapParent;
        [SerializeField] private LevelData levelData;

        public Map BuildMap()
        {
            return new MapBuilder(levelData.EnvironmentBounds, mapParent)
                .WithFloor(levelData.FloorPrefab)
                .WithWalls(levelData.WallPrefab)
                .WithCeiling(levelData.WallPrefab)
                .WithDice(levelData.StartPosition, dicePrefabs)
                .Build();
        }
    }
}