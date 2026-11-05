using NUnit.Framework;
using UnityEngine;
using ZooWorld.World.Terrain.Configuration;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.Tests
{
    public sealed class HeightFieldTerrainTests
    {
        private const float Tolerance = 0.001f;
        private const float HillHeight = 1f;
        private const float ValleyHeight = -0.3f;
        private const float Slope = 0.25f;

        private TerrainConfig _config;

        [SetUp]
        public void SetUp() =>
            _config = ScriptableObject.CreateInstance<TerrainConfig>();

        [TearDown]
        public void TearDown() =>
            Object.DestroyImmediate(_config);

        [Test]
        public void GetSpeedMultiplier_Uphill_IsSlower()
        {
            HeightFieldTerrain terrain = CreateTerrain(HillHeight, Slope);

            Assert.AreEqual(0.75f, terrain.GetSpeedMultiplier(Vector3.zero, Vector3.right), Tolerance);
        }

        [Test]
        public void GetSpeedMultiplier_Downhill_IsFaster()
        {
            HeightFieldTerrain terrain = CreateTerrain(HillHeight, Slope);

            Assert.AreEqual(1.25f, terrain.GetSpeedMultiplier(Vector3.zero, Vector3.left), Tolerance);
        }

        [Test]
        public void GetSpeedMultiplier_AcrossSlope_IsUnchanged()
        {
            HeightFieldTerrain terrain = CreateTerrain(HillHeight, Slope);

            Assert.AreEqual(1f, terrain.GetSpeedMultiplier(Vector3.zero, Vector3.forward), Tolerance);
        }

        [Test]
        public void GetSpeedMultiplier_InValley_IsSlowed()
        {
            HeightFieldTerrain terrain = CreateTerrain(ValleyHeight, 0f);

            Assert.AreEqual(_config.ValleySpeedMultiplier, terrain.GetSpeedMultiplier(Vector3.zero, Vector3.forward), Tolerance);
        }

        [Test]
        public void GetSpeedMultiplier_VerySteepUphill_IsClamped()
        {
            HeightFieldTerrain terrain = CreateTerrain(HillHeight, 2f);

            Assert.AreEqual(_config.MinSpeedMultiplier, terrain.GetSpeedMultiplier(Vector3.zero, Vector3.right), Tolerance);
        }

        [Test]
        public void HeightAt_ReturnsFieldHeight()
        {
            HeightFieldTerrain terrain = CreateTerrain(HillHeight, 0f);

            Assert.AreEqual(HillHeight, terrain.HeightAt(new Vector3(1f, 5f, 1f)), Tolerance);
        }

        private HeightFieldTerrain CreateTerrain(float baseHeight, float slope) =>
            new(_config, TerrainTestData.CreatePlane(8, 1f, baseHeight, slope));
    }
}
