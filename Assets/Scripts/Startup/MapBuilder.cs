using Startup.Models;
using UnityEngine;

namespace Startup
{
    public sealed class MapBuilder
    {
        private const int WALLS_COUNT = 4;
        private const float WALS_THICKNESS = 1f;
        
        private readonly Bounds _environmentBounds;
        private readonly Transform _mapParent;

        private GameObject _floorInstance;
        private GameObject[] _wallsInstances;
        private GameObject _ceilingInstance;

        public MapBuilder(Bounds environmentBounds, Transform mapParent)
        {
            _environmentBounds = environmentBounds;
            _mapParent = mapParent;
        }

        public MapBuilder WithFloor(GameObject floorPrefab)
        {
            var boundsSize = _environmentBounds.size;
            
            _floorInstance = Object.Instantiate(floorPrefab, Vector3.zero, Quaternion.identity, _mapParent);
            var floorInstanceTransform = _floorInstance.transform;
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
                new (boundsCenter.x, boundsSize.y/2f, boundsMax.z + WALS_THICKNESS/2f),
                new (boundsCenter.x, boundsSize.y/2f, boundsMin.z - WALS_THICKNESS/2f),
                new (boundsMax.x + WALS_THICKNESS/2f, boundsSize.y/2f, boundsCenter.z),
                new (boundsMin.x - WALS_THICKNESS/2f, boundsSize.y/2f, boundsCenter.z)
            };
            
            var wallsScales = new Vector3[]
            {
                new (boundsSize.x, boundsSize.y, WALS_THICKNESS),
                new (boundsSize.x, boundsSize.y, WALS_THICKNESS),
                new (WALS_THICKNESS, boundsSize.y, boundsSize.z),
                new (WALS_THICKNESS, boundsSize.y, boundsSize.z)
            };
            
            _wallsInstances = new GameObject[WALLS_COUNT];

            for (var i = 0; i < wallsPositions.Length; i++)
            {
                var wallPosition = wallsPositions[i];
                _wallsInstances[i] = Object.Instantiate(wallPrefab, wallPosition, Quaternion.identity, _mapParent);
                var wallInstanceTransform = _wallsInstances[i].transform;
                wallInstanceTransform.localScale = wallsScales[i];
            }

            return this;
        }

        public MapBuilder WithCeiling(GameObject wallPrefab)
        {
            var boundsMax = _environmentBounds.max;
            var boundsSize = _environmentBounds.size;
            
            _ceilingInstance = Object.Instantiate(wallPrefab, new Vector3(0, boundsMax.y, 0), 
                Quaternion.identity, _mapParent);
            var floorInstanceTransform = _ceilingInstance.transform;
            floorInstanceTransform.localScale = new Vector3(boundsSize.x, WALS_THICKNESS, boundsSize.z);
            
            return this;
        }

        public Map Build()
        {
            return new Map(_floorInstance, _wallsInstances, _ceilingInstance);
        }
    }
}