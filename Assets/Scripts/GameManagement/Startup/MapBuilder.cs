using System.Collections.Generic;
using System.Linq;
using DieSimulation.Components;
using DieSimulation.Interfaces;
using GameManagement.Startup.Models;
using UnityEngine;
using Utilities;

namespace GameManagement.Startup
{
    public sealed class MapBuilder
    {
        private const float WALS_THICKNESS = 1f;

        private readonly Bounds _environmentBounds;
        private readonly Transform _mapParent;

        private IDieProvider[] _dieInstances;

        public MapBuilder(Bounds environmentBounds, Transform mapParent)
        {
            _environmentBounds = environmentBounds;
            _mapParent = mapParent;
        }

        public MapBuilder WithFloor(GameObject floorPrefab)
        {
            var boundsSize = _environmentBounds.size;

            var floorInstance = Object.Instantiate(floorPrefab, Vector3.zero, Quaternion.identity, _mapParent);
            var floorInstanceTransform = floorInstance.transform;
            floorInstanceTransform.localScale = new Vector3(boundsSize.x, WALS_THICKNESS, boundsSize.z);

            return this;
        }

        public MapBuilder WithWalls(GameObject wallPrefab)
        {
            var boundsMax = _environmentBounds.max;
            var boundsMin = _environmentBounds.min;
            var boundsSize = _environmentBounds.size;
            var boundsCenter = _environmentBounds.center;

            var wallsPositions = new Vector3[]
            {
                new(boundsCenter.x, boundsSize.y / 2f, boundsMax.z + WALS_THICKNESS / 2f),
                new(boundsCenter.x, boundsSize.y / 2f, boundsMin.z - WALS_THICKNESS / 2f),
                new(boundsMax.x + WALS_THICKNESS / 2f, boundsSize.y / 2f, boundsCenter.z),
                new(boundsMin.x - WALS_THICKNESS / 2f, boundsSize.y / 2f, boundsCenter.z)
            };

            var wallsScales = new Vector3[]
            {
                new(boundsSize.x, boundsSize.y, WALS_THICKNESS),
                new(boundsSize.x, boundsSize.y, WALS_THICKNESS),
                new(WALS_THICKNESS, boundsSize.y, boundsSize.z),
                new(WALS_THICKNESS, boundsSize.y, boundsSize.z)
            };

            for (var i = 0; i < wallsPositions.Length; i++)
            {
                var wallPosition = wallsPositions[i];
                var wallInstance = Object.Instantiate(wallPrefab, wallPosition, Quaternion.identity, _mapParent);
                var wallInstanceTransform = wallInstance.transform;
                wallInstanceTransform.localScale = wallsScales[i];
            }

            return this;
        }

        public MapBuilder WithCeiling(GameObject wallPrefab)
        {
            var boundsMax = _environmentBounds.max;
            var boundsSize = _environmentBounds.size;

            var ceilingInstance = Object.Instantiate(wallPrefab, new Vector3(0, boundsMax.y, 0),
                Quaternion.identity, _mapParent);
            var floorInstanceTransform = ceilingInstance.transform;
            floorInstanceTransform.localScale = new Vector3(boundsSize.x, WALS_THICKNESS, boundsSize.z);

            return this;
        }

        public MapBuilder WithDice(Vector3 startPosition, IReadOnlyList<DieController> dicePrefabs)
        {
            var count = dicePrefabs.Count;

            var spawnedPositions = GetInitialPositions(_environmentBounds, startPosition,
                count, 2f);

            _dieInstances = new IDieProvider[count];

            for (var i = 0; i < count; i++)
            {
                var spawnPosition = spawnedPositions[i];
                var prefab = dicePrefabs[i];

                var dieInstance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity);

                if (dieInstance.TryGetComponent<DieDragLimiter>(out var dieDragLimiter))
                {
                    dieDragLimiter.Configure(_environmentBounds);
                }

                _dieInstances[i] = dieInstance;
            }

            return this;
        }

        public Map Build()
        {
            return new Map(_dieInstances);
        }

        private IReadOnlyList<Vector3> GetInitialPositions(Bounds bounds, Vector3 startPosition,
            int count, float minDistance)
        {
            const int maxAttempts = 10000;
            const float epsilon = 1f;

            var positions = new List<Vector3>(count)
            {
                startPosition
            };

            var minDistanceSqr = minDistance * minDistance;
            var attempts = 0;

            while (positions.Count < count && attempts < maxAttempts)
            {
                var candidate = bounds.RandomPointInBounds(epsilon);
                var valid = positions.All(t => !((candidate - t).sqrMagnitude < minDistanceSqr));

                if (valid)
                {
                    candidate.y = 1;
                    positions.Add(candidate);
                }

                attempts++;
            }

            return positions;
        }
    }
}