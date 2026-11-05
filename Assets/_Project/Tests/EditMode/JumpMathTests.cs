using NUnit.Framework;
using UnityEngine;
using ZooWorld.Animals.Movement.Strategies;

namespace ZooWorld.Tests
{
    public sealed class JumpMathTests
    {
        private const float Gravity = 9.81f;
        private const float Tolerance = 0.001f;

        [TestCase(2f, 45f)]
        [TestCase(3.5f, 30f)]
        [TestCase(1f, 70f)]
        public void LaunchVelocity_CoversRequestedDistance(float distance, float angle)
        {
            Vector3 velocity = JumpMath.CalculateLaunchVelocity(Vector3.forward, distance, angle, Gravity);

            float flightTime = 2f * velocity.y / Gravity;
            float horizontalSpeed = new Vector2(velocity.x, velocity.z).magnitude;

            Assert.AreEqual(distance, horizontalSpeed * flightTime, Tolerance);
        }

        [Test]
        public void LaunchVelocity_FollowsHorizontalDirection()
        {
            Vector3 direction = new Vector3(1f, 0f, 1f).normalized;

            Vector3 velocity = JumpMath.CalculateLaunchVelocity(direction, 2f, 45f, Gravity);
            Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z).normalized;

            Assert.AreEqual(1f, Vector3.Dot(direction, horizontal), Tolerance);
        }
    }
}
