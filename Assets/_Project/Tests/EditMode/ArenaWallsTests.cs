using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.World;

namespace ZooWorld.Tests
{
    public sealed class ArenaWallsTests
    {
        private static readonly Rect Area = Rect.MinMaxRect(-10f, -6f, 10f, 6f);

        private ArenaWalls _walls;
        private BoxCollider[] _colliders;

        [SetUp]
        public void SetUp()
        {
            IScreenBounds bounds = Substitute.For<IScreenBounds>();
            bounds.Area.Returns(Area);

            _walls = new ArenaWalls(bounds);
            _walls.Start();
            _colliders = GameObject.Find(ArenaWalls.ObjectName).GetComponents<BoxCollider>();
        }

        [TearDown]
        public void TearDown() =>
            Object.DestroyImmediate(GameObject.Find(ArenaWalls.ObjectName));

        [Test]
        public void Start_CreatesFourWalls() =>
            Assert.AreEqual(4, _colliders.Length);

        [TestCase(10.5f, 0f)]
        [TestCase(-10.5f, 0f)]
        [TestCase(0f, 6.5f)]
        [TestCase(0f, -6.5f)]
        public void PointJustBeyondEdge_IsInsideWall(float x, float z) =>
            Assert.IsTrue(IsInsideAnyWall(new Vector3(x, 1f, z)));

        [TestCase(0f, 0f)]
        [TestCase(9.9f, 5.9f)]
        [TestCase(-9.9f, -5.9f)]
        public void PointInsideArea_IsFree(float x, float z) =>
            Assert.IsFalse(IsInsideAnyWall(new Vector3(x, 1f, z)));

        private bool IsInsideAnyWall(Vector3 point) =>
            _colliders.Any(collider => collider.bounds.Contains(point));
    }
}
