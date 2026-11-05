using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.World.Obstacles.Configuration;
using ZooWorld.World.Obstacles.Generation;
using ZooWorld.World.Obstacles.Runtime;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.Tests
{
    public sealed class ObstacleLayoutGeneratorTests
    {
        private const int Seed = 7;
        private const float Tolerance = 0.001f;

        private static readonly Rect Area = Rect.MinMaxRect(-13f, -7.5f, 13f, 7.5f);

        private ObstacleFieldConfig _config;
        private ITerrain _terrain;
        private Obstacle _rock;
        private Obstacle _fence;

        [SetUp]
        public void SetUp()
        {
            _config = ScriptableObject.CreateInstance<ObstacleFieldConfig>();
            _terrain = Substitute.For<ITerrain>();
            _rock = TestData.CreateObstacle(footprintRadius: 0.8f);
            _fence = TestData.CreateObstacle(footprintRadius: 1.6f);
            TestData.SetObstacleEntries(_config, (_rock, 2f), (_fence, 1f));
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_config);
            Object.DestroyImmediate(_rock.gameObject);
            Object.DestroyImmediate(_fence.gameObject);
        }

        [Test]
        public void Generate_CountWithinConfiguredRange()
        {
            IReadOnlyList<ObstaclePlacement> placements = Generate();

            Assert.GreaterOrEqual(placements.Count, _config.MinCount);
            Assert.LessOrEqual(placements.Count, _config.MaxCount);
        }

        [Test]
        public void Generate_PlacementsStayInsideAreaWithPadding()
        {
            foreach (ObstaclePlacement placement in Generate())
            {
                float margin = placement.Radius + _config.EdgePadding - Tolerance;
                Assert.GreaterOrEqual(placement.Position.x - margin, Area.xMin);
                Assert.LessOrEqual(placement.Position.x + margin, Area.xMax);
                Assert.GreaterOrEqual(placement.Position.y - margin, Area.yMin);
                Assert.LessOrEqual(placement.Position.y + margin, Area.yMax);
            }
        }

        [Test]
        public void Generate_PlacementsKeepSpacing()
        {
            IReadOnlyList<ObstaclePlacement> placements = Generate();

            for (int i = 0; i < placements.Count; i++)
            {
                for (int j = i + 1; j < placements.Count; j++)
                {
                    float distance = Vector2.Distance(placements[i].Position, placements[j].Position);
                    float required = placements[i].Radius + placements[j].Radius + _config.Spacing;
                    Assert.GreaterOrEqual(distance + Tolerance, required);
                }
            }
        }

        [Test]
        public void Generate_SlopedGround_RespectsEachPrefabGroundDrop()
        {
            _terrain.HeightAt(Arg.Any<Vector3>()).Returns(call => Mathf.Max(0f, -call.Arg<Vector3>().x));

            IReadOnlyList<ObstaclePlacement> placements = Generate();

            Assert.IsNotEmpty(placements);

            foreach (ObstaclePlacement placement in placements)
            {
                float drop = ObstacleFootprint.CalculateGroundDrop(_terrain, placement);
                Assert.LessOrEqual(drop, placement.Prefab.MaxGroundDrop + Tolerance);
            }
        }

        [Test]
        public void Generate_NoUsableEntries_ReturnsEmpty()
        {
            TestData.SetObstacleEntries(_config, (_rock, 0f), (null, 1f));

            Assert.IsEmpty(Generate());
        }

        private IReadOnlyList<ObstaclePlacement> Generate() =>
            ObstacleLayoutGenerator.Generate(_config, Area, _terrain, new SeededRandomService(Seed));
    }
}
