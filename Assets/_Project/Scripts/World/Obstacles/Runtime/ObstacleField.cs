using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.World.Obstacles.Runtime
{
    public sealed class ObstacleField : IObstacleMap, IObstacleRegistry
    {
        private const int MaxOverlaps = 32;

        private readonly HashSet<Collider> _colliders = new();
        private readonly Collider[] _overlaps = new Collider[MaxOverlaps];

        public void Add(Obstacle obstacle)
        {
            foreach (Collider collider in obstacle.Colliders)
                _colliders.Add(collider);
        }

        public bool TryFindObstacle(Vector3 point, float radius, out Vector3 closestPoint)
        {
            closestPoint = point;

            if (_colliders.Count == 0)
                return false;

            int count = Physics.OverlapSphereNonAlloc(
                point, radius, _overlaps, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                if (!_colliders.Contains(_overlaps[i]))
                    continue;

                closestPoint = _overlaps[i].ClosestPoint(point);
                return true;
            }

            return false;
        }
    }
}
