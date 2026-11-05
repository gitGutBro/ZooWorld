using UnityEngine;
using ZooWorld.Animals.Movement.Configuration;

namespace ZooWorld.Animals.Movement.Strategies
{
    public sealed class LinearMovementStrategy : MovementStrategy<LinearMovementConfig>
    {
        public LinearMovementStrategy(LinearMovementConfig config, MovementContext context) : base(config, context) { }

        protected override void Move(float deltaTime)
        {
            Vector3 horizontal = Direction * (Config.Speed * CalculateTerrainSpeedFactor());
            Body.velocity = new Vector3(horizontal.x, Body.velocity.y, horizontal.z);
            FaceDirection();
        }
    }
}
