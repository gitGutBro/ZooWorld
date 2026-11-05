using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.Spawning;
using ZooWorld.Infrastructure;
using ZooWorld.World.Obstacles.Configuration;
using Object = UnityEngine.Object;

namespace ZooWorld.Tests
{
    public sealed class ConfigAssetsTests
    {
        private const string ConfigsFolder = "Assets/_Project/Configs";

        [Test]
        public void AnimalConfigs_HaveAllReferences()
        {
            List<AnimalConfig> configs = LoadAssets<AnimalConfig>();
            Assert.IsNotEmpty(configs);

            foreach (AnimalConfig config in configs)
            {
                Assert.IsTrue(config.Prefab, $"{config.name}: prefab is missing.");
                Assert.IsTrue(config.Prefab.Body, $"{config.name}: prefab has no Rigidbody assigned.");
                Assert.IsTrue(config.Role, $"{config.name}: food chain role is missing.");
                Assert.IsTrue(config.Movement, $"{config.name}: movement config is missing.");
            }
        }

        [Test]
        public void AnimalConfigs_HaveUniqueIds()
        {
            List<string> ids = LoadAssets<AnimalConfig>().Select(config => config.Id).ToList();

            Assert.That(ids, Has.None.Empty);
            Assert.That(ids, Is.Unique);
        }

        [Test]
        public void GameConfigs_HaveValidSpawnSettings()
        {
            List<GameConfig> configs = LoadAssets<GameConfig>();
            Assert.IsNotEmpty(configs);

            foreach (GameConfig config in configs)
            {
                SpawnConfig spawn = config.Spawn;
                Assert.LessOrEqual(spawn.MinIntervalSeconds, spawn.MaxIntervalSeconds, $"{config.name}: min spawn interval exceeds max.");
                Assert.IsTrue(spawn.Entries.Any(entry => entry.Weight > 0f), $"{config.name}: no entry with positive weight.");
                Assert.IsTrue(spawn.Entries.All(entry => entry.Animal), $"{config.name}: spawn entry without animal.");
                Assert.IsTrue(config.Terrain, $"{config.name}: terrain config is missing.");
                Assert.LessOrEqual(config.Bounce.MinSpeedChange, config.Bounce.MaxSpeedChange, $"{config.name}: min bounce exceeds max.");
                Assert.IsTrue(config.BounceEffectPrefab, $"{config.name}: bounce effect prefab is missing.");
                Assert.IsTrue(config.Obstacles, $"{config.name}: obstacle field config is missing.");
            }
        }

        [Test]
        public void ObstacleFieldConfigs_HaveValidObstacles()
        {
            List<ObstacleFieldConfig> configs = LoadAssets<ObstacleFieldConfig>();
            Assert.IsNotEmpty(configs);

            foreach (ObstacleFieldConfig config in configs)
            {
                Assert.LessOrEqual(config.MinCount, config.MaxCount, $"{config.name}: min count exceeds max.");
                Assert.IsNotEmpty(config.Entries, $"{config.name}: no obstacles.");

                foreach (ObstacleEntry entry in config.Entries)
                {
                    Assert.IsTrue(entry.Prefab, $"{config.name}: obstacle entry without prefab.");
                    Assert.Greater(entry.Prefab.FootprintRadius, 0f, $"{entry.Prefab.name}: footprint radius is not set.");
                    Assert.IsNotEmpty(entry.Prefab.Colliders, $"{entry.Prefab.name}: no colliders.");
                }
            }
        }

        private static List<T> LoadAssets<T>() where T : Object =>
            AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { ConfigsFolder })
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<T>(path))
                .ToList();
    }
}
