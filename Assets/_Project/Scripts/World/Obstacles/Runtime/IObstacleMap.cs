using UnityEngine;

namespace ZooWorld.World.Obstacles.Runtime
{
    public interface IObstacleMap
    {
        bool TryFindObstacle(Vector3 point, float radius, out Vector3 closestPoint);
    }
}
