using System;
using System.Collections.Generic;
using UnityEngine;
using ZooWorld.Animals.FoodChain.Bounce;
using ZooWorld.Animals.FoodChain.Roles;
using ZooWorld.Animals.Spawning;
using ZooWorld.Effects.Bounce;
using ZooWorld.World.Obstacles.Configuration;
using ZooWorld.World.Terrain.Configuration;

namespace ZooWorld.Infrastructure
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ZooWorld/Game Config")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("Spawning")]
        [SerializeField] private SpawnConfig _spawn = new();
        [Space]

        [Header("Terrain")]
        [SerializeField] private TerrainConfig _terrain;
        [Space]

        [Header("Obstacles")]
        [SerializeField] private ObstacleFieldConfig _obstacles;
        [Space]

        [Header("Food Chain")]
        [SerializeField] private FoodChainRole[] _countedRoles = Array.Empty<FoodChainRole>();
        [Space]

        [Header("Bounce")]
        [SerializeField] private BounceConfig _bounce = new();
        [SerializeField] private BounceEffectView _bounceEffectPrefab;
        [Space]

        [Header("Tasty Label")]
        [SerializeField, Min(0f)] private float _tastyLabelDurationSeconds = 1f;

        public SpawnConfig Spawn => _spawn;
        public TerrainConfig Terrain => _terrain;
        public ObstacleFieldConfig Obstacles => _obstacles;
        public IReadOnlyList<FoodChainRole> CountedRoles => _countedRoles;
        public BounceConfig Bounce => _bounce;
        public BounceEffectView BounceEffectPrefab => _bounceEffectPrefab;
        public TimeSpan TastyLabelDuration => TimeSpan.FromSeconds(_tastyLabelDurationSeconds);
    }
}
