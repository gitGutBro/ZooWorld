using System;
using UnityEngine;
using ZooWorld.World.Obstacles.Runtime;

namespace ZooWorld.World.Obstacles.Configuration
{
    [Serializable]
    public sealed class ObstacleEntry
    {
        [SerializeField] private Obstacle _prefab;
        [SerializeField, Min(0f)] private float _weight = 1f;

        public Obstacle Prefab => _prefab;
        public float Weight => _weight;
    }
}
