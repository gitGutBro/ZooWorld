using UnityEngine;
using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Timing;

namespace ZooWorld.Animals.Movement.Strategies
{
    public sealed class JumpMovementStrategy : MovementStrategy<JumpMovementConfig>
    {
        private const float GroundedVerticalSpeed = 0.05f;

        private readonly CountdownTimer _cooldown = new();

        public JumpMovementStrategy(JumpMovementConfig config, MovementContext context) : base(config, context) =>
            _cooldown.Restart(config.JumpIntervalSeconds);

        private bool IsGrounded => Mathf.Abs(Body.velocity.y) < GroundedVerticalSpeed;

        protected override void Move(float deltaTime)
        {
            _cooldown.Tick(deltaTime);

            if (!_cooldown.IsFinished)
                return;

            TryJump();
        }

        private void TryJump()
        {
            if (!IsGrounded)
                return;

            _cooldown.Restart(Config.JumpIntervalSeconds);
            FaceDirection();
            Body.velocity = CalculateLaunchVelocity();
        }

        private Vector3 CalculateLaunchVelocity() =>
            JumpMath.CalculateLaunchVelocity(
                Direction,
                Config.JumpDistance * CalculateTerrainSpeedFactor(),
                Config.JumpAngle,
                Physics.gravity.magnitude);
    }
}
