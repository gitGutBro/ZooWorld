using UnityEngine;
using ZooWorld.Animals.Movement.Configuration;

namespace ZooWorld.Animals.Movement.Strategies
{
    public abstract class MovementStrategy<TConfig> : IMovementStrategy where TConfig : MovementConfig
    {
        private readonly MovementContext _context;

        protected MovementStrategy(TConfig config, MovementContext context)
        {
            Config = config;
            _context = context;
        }

        protected TConfig Config { get; }
        protected Rigidbody Body => _context.Body;
        protected Vector3 Direction => _context.Direction.Current;

        public void Tick(float deltaTime)
        {
            _context.Direction.Tick(deltaTime, Body.position);
            Move(deltaTime);
        }

        protected abstract void Move(float deltaTime);

        protected float CalculateTerrainSpeedFactor() =>
            _context.Terrain.GetSpeedMultiplier(Body.position, Direction);

        protected void FaceDirection()
        {
            if (Direction == Vector3.zero)
                return;

            Body.MoveRotation(Quaternion.LookRotation(Direction, Vector3.up));
        }
    }
}
