using NUnit.Framework;
using UnityEngine;
using ZooWorld.World.Terrain.Data;

namespace ZooWorld.Tests
{
    public sealed class HeightFieldTests
    {
        private const float Tolerance = 0.001f;
        private const float Slope = 0.5f;

        private HeightField _plane;

        [SetUp]
        public void SetUp() =>
            _plane = TerrainTestData.CreatePlane(4, 1f, 0f, Slope);

        [Test]
        public void Sample_AtVertex_ReturnsVertexHeight()
        {
            Assert.AreEqual(1f, _plane.Sample(2f, 0f), Tolerance);
            Assert.AreEqual(-1f, _plane.Sample(-2f, 1f), Tolerance);
        }

        [Test]
        public void Sample_BetweenVertices_Interpolates() =>
            Assert.AreEqual(0.125f, _plane.Sample(0.25f, 0.3f), Tolerance);

        [Test]
        public void Sample_OutsideField_ClampsToEdge()
        {
            Assert.AreEqual(_plane.Sample(2f, 0f), _plane.Sample(50f, 0f), Tolerance);
            Assert.AreEqual(_plane.Sample(-2f, 0f), _plane.Sample(-50f, 0f), Tolerance);
        }

        [Test]
        public void GradientAt_Plane_ReturnsSlope()
        {
            Vector2 gradient = _plane.GradientAt(0f, 0f);

            Assert.AreEqual(Slope, gradient.x, Tolerance);
            Assert.AreEqual(0f, gradient.y, Tolerance);
        }

        [Test]
        public void VertexPosition_IsCenteredAroundOrigin()
        {
            Assert.AreEqual(new Vector3(-2f, -1f, -2f), _plane.VertexPosition(0, 0));
            Assert.AreEqual(new Vector3(2f, 1f, 2f), _plane.VertexPosition(4, 4));
        }
    }
}
