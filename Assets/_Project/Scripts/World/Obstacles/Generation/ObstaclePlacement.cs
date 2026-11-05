using UnityEngine;
using ZooWorld.World.Obstacles.Runtime;

namespace ZooWorld.World.Obstacles.Generation
{
    public readonly struct ObstaclePlacement
    {
        public ObstaclePlacement(Obstacle prefab, Vector2 position, float rotationDegrees, float scale)
        {
            Prefab = prefab;
            Position = position;
            RotationDegrees = rotationDegrees;
            Scale = scale;
        }

        public Obstacle Prefab { get; }
        public Vector2 Position { get; }
        public float RotationDegrees { get; }
        public float Scale { get; }
        public float Radius => Prefab.FootprintRadius * Scale;
    }
}
