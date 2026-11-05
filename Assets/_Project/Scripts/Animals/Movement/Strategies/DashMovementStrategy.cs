using UnityEngine;
using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Timing;

namespace ZooWorld.Animals.Movement.Strategies
{
    public sealed class DashMovementStrategy : MovementStrategy<DashMovementConfig>
    {
        private readonly CountdownTimer _cooldown = new();

        public DashMovementStrategy(DashMovementConfig config, MovementContext context) : base(config, context) { }

        protected override void Move(float deltaTime)
        {
            _cooldown.Tick(deltaTime);

            if (!_cooldown.IsFinished)
            {
                Glide(deltaTime);
                return;
            }

            Dash();
        }

        private void Dash()
        {
            _cooldown.Restart(Config.DashCooldownSeconds);
            FaceDirection();

            Vector3 horizontal = Direction * (Config.DashSpeed * CalculateTerrainSpeedFactor());
            Body.velocity = new Vector3(horizontal.x, Body.velocity.y, horizontal.z);
        }

        private void Glide(float deltaTime)
        {
            float damping = Mathf.Exp(-Config.GlideDamping * deltaTime);
            Vector3 velocity = Body.velocity;
            Body.velocity = new Vector3(velocity.x * damping, velocity.y, velocity.z * damping);
        }
    }
}
