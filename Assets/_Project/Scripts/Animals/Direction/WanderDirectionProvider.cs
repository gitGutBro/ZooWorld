using UnityEngine;
using ZooWorld.Animals.Timing;
using ZooWorld.World;
using ZooWorld.World.Obstacles.Runtime;

namespace ZooWorld.Animals.Direction
{
    public sealed class WanderDirectionProvider : IDirectionProvider
    {
        private const float FullTurnDegrees = 360f;
        private const float ReturnSpreadDegrees = 30f;
        private const float ReturnAlignment = 0.5f;
        private const float EdgeLookAhead = 1.5f;
        private const float ObstacleLookAhead = 1.2f;
        private const float ObstacleProbeRadius = 0.35f;
        private const float AvoidSpreadDegrees = 25f;
        private const float MinAwaySqrMagnitude = 0.0001f;

        private readonly IScreenBounds _bounds;
        private readonly IObstacleMap _obstacles;
        private readonly IRandomService _random;
        private readonly float _changeSeconds;
        private readonly CountdownTimer _changeTimer = new();

        public WanderDirectionProvider(IScreenBounds bounds, IObstacleMap obstacles, IRandomService random, float changeSeconds)
        {
            _bounds = bounds;
            _obstacles = obstacles;
            _random = random;
            _changeSeconds = changeSeconds;
            PickRandomDirection();
        }

        public Vector3 Current { get; private set; }

        public void Tick(float deltaTime, Vector3 position)
        {
            if (IsLeavingScreen(position))
            {
                TurnBackToScreen(position);
                return;
            }

            if (TryAvoidObstacle(position))
                return;

            _changeTimer.Tick(deltaTime);

            if (!_changeTimer.IsFinished)
                return;

            PickRandomDirection();
        }

        private bool IsLeavingScreen(Vector3 position) =>
            !_bounds.Contains(position) || !_bounds.Contains(position + Current * EdgeLookAhead);

        private void PickRandomDirection()
        {
            Current = Rotate(Vector3.forward, _random.Range(0f, FullTurnDegrees));
            _changeTimer.Restart(_changeSeconds);
        }

        private void TurnBackToScreen(Vector3 position)
        {
            Vector3 toCenter = _bounds.Center - position;
            toCenter.y = 0f;
            toCenter.Normalize();

            if (Vector3.Dot(Current, toCenter) > ReturnAlignment)
                return;

            Current = Rotate(toCenter, _random.Range(-ReturnSpreadDegrees, ReturnSpreadDegrees));
            _changeTimer.Restart(_changeSeconds);
        }

        private bool TryAvoidObstacle(Vector3 position)
        {
            Vector3 probe = position + Current * ObstacleLookAhead;

            if (!_obstacles.TryFindObstacle(probe, ObstacleProbeRadius, out Vector3 closestPoint))
                return false;

            Vector3 normal = CalculateAwayNormal(probe, closestPoint);

            if (Vector3.Dot(Current, normal) >= 0f)
                return false;

            Vector3 direction = Vector3.Reflect(inDirection: Current, normal);
            float degrees = _random.Range(-AvoidSpreadDegrees, AvoidSpreadDegrees);
            Current = Rotate(direction, degrees);

            _changeTimer.Restart(_changeSeconds);
            return true;
        }

        private Vector3 CalculateAwayNormal(Vector3 probe, Vector3 closestPoint)
        {
            Vector3 away = probe - closestPoint;
            away.y = 0f;

            return away.sqrMagnitude > MinAwaySqrMagnitude ? away.normalized : -Current;
        }

        private static Vector3 Rotate(Vector3 direction, float degrees) =>
            Quaternion.Euler(0f, degrees, 0f) * direction;
    }
}
