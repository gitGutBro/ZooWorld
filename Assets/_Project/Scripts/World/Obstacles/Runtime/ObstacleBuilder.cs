using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using ZooWorld.World.Obstacles.Configuration;
using ZooWorld.World.Obstacles.Generation;
using ZooWorld.World.Terrain.Runtime;
using Object = UnityEngine.Object;

namespace ZooWorld.World.Obstacles.Runtime
{
    public sealed class ObstacleBuilder : IStartable, IDisposable
    {
        private const string RootName = "Obstacles";

        private readonly ObstacleFieldConfig _config;
        private readonly IScreenBounds _bounds;
        private readonly ITerrain _terrain;
        private readonly IRandomService _random;
        private readonly IObstacleRegistry _registry;

        private Transform _root;

        public ObstacleBuilder(
            ObstacleFieldConfig config,
            IScreenBounds bounds,
            ITerrain terrain,
            IRandomService random,
            IObstacleRegistry registry)
        {
            _config = config;
            _bounds = bounds;
            _terrain = terrain;
            _random = random;
            _registry = registry;
        }

        public void Start()
        {
            _root = new GameObject(RootName).transform;

            IReadOnlyList<ObstaclePlacement> list =
                ObstacleLayoutGenerator.Generate(_config, _bounds.Area, _terrain, _random);

            for (int i = 0; i < list.Count; i++)
            {
                ObstaclePlacement placement = list[i];
                _registry.Add(Place(placement));
            }

            Physics.SyncTransforms();
        }

        public void Dispose()
        {
            if (_root)
                Object.Destroy(_root.gameObject);
        }

        private Obstacle Place(ObstaclePlacement placement)
        {
            float height = ObstacleFootprint.CalculateAnchorHeight(_terrain, placement) - _config.SinkDepth;
            Vector3 position = new(placement.Position.x, height, placement.Position.y);
            Quaternion rotation = Quaternion.Euler(0f, placement.RotationDegrees, 0f);

            Obstacle obstacle = Object.Instantiate(placement.Prefab, position, rotation, _root);
            obstacle.transform.localScale = Vector3.one * placement.Scale;
            return obstacle;
        }
    }
}
