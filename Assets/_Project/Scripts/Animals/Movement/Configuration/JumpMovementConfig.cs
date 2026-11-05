using UnityEngine;

namespace ZooWorld.Animals.Movement.Configuration
{
    [CreateAssetMenu(fileName = "JumpMovement", menuName = "ZooWorld/Movement/Jump")]
    public sealed class JumpMovementConfig : MovementConfig
    {
        [SerializeField, Min(0.1f)] private float _jumpIntervalSeconds = 1f;
        [SerializeField, Min(0f)] private float _jumpDistance = 2f;
        [SerializeField, Range(1f, 89f)] private float _jumpAngle = 45f;

        public float JumpIntervalSeconds => _jumpIntervalSeconds;
        public float JumpDistance => _jumpDistance;
        public float JumpAngle => _jumpAngle;
    }
}
