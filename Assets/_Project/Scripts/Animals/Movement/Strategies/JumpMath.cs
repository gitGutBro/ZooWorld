using UnityEngine;

namespace ZooWorld.Animals.Movement.Strategies
{
    public static class JumpMath
    {
        public static Vector3 CalculateLaunchVelocity(Vector3 direction, float distance, float angleDegrees, float gravity)
        {
            float speed = CalculateLaunchSpeed(distance, angleDegrees, gravity);
            float angle = angleDegrees * Mathf.Deg2Rad;
            Vector3 horizontal = new Vector3(direction.x, 0f, direction.z).normalized;
            return horizontal * (speed * Mathf.Cos(angle)) + Vector3.up * (speed * Mathf.Sin(angle));
        }

        private static float CalculateLaunchSpeed(float distance, float angleDegrees, float gravity)
        {
            float doubleAngleSine = Mathf.Sin(2f * angleDegrees * Mathf.Deg2Rad);
            return Mathf.Sqrt(distance * gravity / doubleAngleSine);
        }
    }
}
