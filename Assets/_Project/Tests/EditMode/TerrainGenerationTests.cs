using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.World;
using ZooWorld.World.Terrain.Configuration;
using ZooWorld.World.Terrain.Data;
using ZooWorld.World.Terrain.Generation;

namespace ZooWorld.Tests
{
    public sealed class TerrainGenerationTests
    {
        private const float FirstOffset = 500f;
        private const float SecondOffset = 700f;

        private TerrainConfig _config;

        [SetUp]
        public void SetUp() =>
            _config = ScriptableObject.CreateInstance<TerrainConfig>();

        [TearDown]
        public void TearDown() =>
            Object.DestroyImmediate(_config);

        [Test]
        public void GenerateHeightField_HeightsStayInConfiguredRange()
        {
            HeightField field = TerrainGenerator.GenerateHeightField(_config, CreateRandom(FirstOffset));

            for (int z = 0; z < field.VerticesZ; z++)
                for (int x = 0; x < field.VerticesX; x++)
                    Assert.That(field.VertexHeight(x, z), Is.InRange(_config.MinHeight, _config.MaxHeight));
        }

        [Test]
        public void GenerateHeightField_CoversConfiguredSize()
        {
            HeightField field = TerrainGenerator.GenerateHeightField(_config, CreateRandom(FirstOffset));

            Assert.GreaterOrEqual(field.CellsX * field.CellSize, _config.Size.x);
            Assert.GreaterOrEqual(field.CellsZ * field.CellSize, _config.Size.y);
        }

        [Test]
        public void GenerateHeightField_SameRandomValues_ProducesSameTerrain()
        {
            HeightField first = TerrainGenerator.GenerateHeightField(_config, CreateRandom(FirstOffset));
            HeightField second = TerrainGenerator.GenerateHeightField(_config, CreateRandom(FirstOffset));

            Assert.AreEqual(first.Sample(3f, -4f), second.Sample(3f, -4f));
        }

        [Test]
        public void GenerateHeightField_DifferentRandomValues_ProducesDifferentTerrain()
        {
            HeightField first = TerrainGenerator.GenerateHeightField(_config, CreateRandom(FirstOffset));
            HeightField second = TerrainGenerator.GenerateHeightField(_config, CreateRandom(SecondOffset));

            Assert.AreNotEqual(first.Sample(3f, -4f), second.Sample(3f, -4f));
        }

        [Test]
        public void GenerateHeightField_HasBothHillsAndValleys()
        {
            HeightField field = TerrainGenerator.GenerateHeightField(_config, CreateRandom(FirstOffset));
            float lowest = float.MaxValue;
            float highest = float.MinValue;

            for (int z = 0; z < field.VerticesZ; z++)
            {
                for (int x = 0; x < field.VerticesX; x++)
                {
                    lowest = Mathf.Min(lowest, field.VertexHeight(x, z));
                    highest = Mathf.Max(highest, field.VertexHeight(x, z));
                }
            }

            Assert.Greater(highest - lowest, (_config.MaxHeight - _config.MinHeight) * 0.5f);
        }

        private static IRandomService CreateRandom(float value)
        {
            IRandomService random = Substitute.For<IRandomService>();
            random.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(value);
            return random;
        }
    }
}
