using UnityEngine;
using ZooWorld.Animals.Direction;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.Animals.Movement
{
    public sealed class MovementContext
    {
        public MovementContext(Rigidbody body, IDirectionProvider direction, ITerrain terrain)
        {
            Body = body;
            Direction = direction;
            Terrain = terrain;
        }

        public Rigidbody Body { get; }
        public IDirectionProvider Direction { get; }
        public ITerrain Terrain { get; }
    }
}
