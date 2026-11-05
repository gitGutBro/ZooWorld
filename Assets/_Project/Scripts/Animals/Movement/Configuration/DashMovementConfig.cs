using UnityEngine;

namespace ZooWorld.Animals.Movement.Configuration
{
    [CreateAssetMenu(fileName = "DashMovement", menuName = "ZooWorld/Movement/Dash")]
    public sealed class DashMovementConfig : MovementConfig
    {
        [SerializeField, Min(0.1f)] private float _dashCooldownSeconds = 1.4f;
        [SerializeField, Min(0f)] private float _dashSpeed = 8f;
        [SerializeField, Min(0f)] private float _glideDamping = 2.5f;

        public float DashCooldownSeconds => _dashCooldownSeconds;
        public float DashSpeed => _dashSpeed;
        public float GlideDamping => _glideDamping;
    }
}
