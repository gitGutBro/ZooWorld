using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.Animals.Spawning;
using ZooWorld.Infrastructure;
using ZooWorld.World;
using ZooWorld.World.Obstacles.Runtime;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.Tests
{
    public sealed class ScreenSpawnPositionProviderTests
    {
        private const float BlockedCoordinate = -2f;
        private const float FreeCoordinate = 3f;

        private static readonly Rect Area = Rect.MinMaxRect(-10f, -6f, 10f, 6f);

        private GameConfig _config;
        private IRandomService _random;
        private IObstacleMap _obstacles;
        private ScreenSpawnPositionProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _config = ScriptableObject.CreateInstance<GameConfig>();

            IScreenBounds bounds = Substitute.For<IScreenBounds>();
            bounds.Area.Returns(Area);

            _random = Substitute.For<IRandomService>();
            _obstacles = Substitute.For<IObstacleMap>();
            _obstacles.TryFindObstacle(default, default, out Vector3 _)
                .ReturnsForAnyArgs(call => call.ArgAt<Vector3>(0).x < 0f);

            _provider = new ScreenSpawnPositionProvider(bounds, _obstacles, _random, Substitute.For<ITerrain>(), _config.Spawn);
        }

        [TearDown]
        public void TearDown() =>
            Object.DestroyImmediate(_config);

        [Test]
        public void Next_FirstCandidateInsideObstacle_PicksAnotherOne()
        {
            _random.Range(Arg.Any<float>(), Arg.Any<float>())
                .Returns(BlockedCoordinate, BlockedCoordinate, FreeCoordinate, FreeCoordinate);

            Vector3 position = _provider.Next();

            Assert.AreEqual(FreeCoordinate, position.x);
            Assert.AreEqual(FreeCoordinate, position.z);
        }

        [Test]
        public void Next_FreeCandidate_ReturnsItImmediately()
        {
            _random.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(FreeCoordinate);

            _provider.Next();

            _obstacles.ReceivedWithAnyArgs(1).TryFindObstacle(default, default, out Vector3 _);
        }
    }
}
