using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.Animals.Direction;
using ZooWorld.World;
using ZooWorld.World.Obstacles.Runtime;

namespace ZooWorld.Tests
{
    public sealed class WanderDirectionProviderTests
    {
        private const float ChangeInterval = 3f;
        private const float QuarterTurn = 90f;
        private const float Tolerance = 0.001f;

        private static readonly Rect Area = Rect.MinMaxRect(-10f, -6f, 10f, 6f);

        private IObstacleMap _obstacles;
        private WanderDirectionProvider _provider;

        [SetUp]
        public void SetUp()
        {
            IScreenBounds bounds = Substitute.For<IScreenBounds>();
            bounds.Area.Returns(Area);
            bounds.Center.Returns(Vector3.zero);
            bounds.Contains(Arg.Any<Vector3>()).Returns(call => Area.Contains(new Vector2(call.Arg<Vector3>().x, call.Arg<Vector3>().z)));

            IRandomService random = Substitute.For<IRandomService>();
            random.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(call => call.ArgAt<float>(0) == 0f ? QuarterTurn : 0f);

            _obstacles = Substitute.For<IObstacleMap>();
            _provider = new WanderDirectionProvider(bounds, _obstacles, random, ChangeInterval);
        }

        [Test]
        public void Tick_NearEdgeHeadingOutward_TurnsBackBeforeLeaving()
        {
            Assert.AreEqual(1f, Vector3.Dot(_provider.Current, Vector3.right), Tolerance);

            _provider.Tick(0.02f, new Vector3(9.5f, 0f, 0f));

            Assert.AreEqual(1f, Vector3.Dot(_provider.Current, Vector3.left), Tolerance);
        }

        [Test]
        public void Tick_InsideHeadingInward_KeepsDirection()
        {
            Vector3 before = _provider.Current;

            _provider.Tick(0.02f, new Vector3(-5f, 0f, 0f));

            Assert.AreEqual(before, _provider.Current);
        }

        [Test]
        public void Tick_ObstacleAhead_TurnsAway()
        {
            PlaceObstacleSurfaceAt(new Vector3(1.5f, 0f, 0f));

            _provider.Tick(0.02f, Vector3.zero);

            Assert.AreEqual(1f, Vector3.Dot(_provider.Current, Vector3.left), Tolerance);
        }

        [Test]
        public void Tick_ObstacleBehindProbe_KeepsDirection()
        {
            PlaceObstacleSurfaceAt(new Vector3(1f, 0f, 0f));
            Vector3 before = _provider.Current;

            _provider.Tick(0.02f, Vector3.zero);

            Assert.AreEqual(before, _provider.Current);
        }

        private void PlaceObstacleSurfaceAt(Vector3 closestPoint)
        {
            _obstacles.TryFindObstacle(default, default, out Vector3 _).ReturnsForAnyArgs(call =>
            {
                call[2] = closestPoint;
                return true;
            });
        }
    }
}
