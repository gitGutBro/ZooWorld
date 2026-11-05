using UnityEngine;

namespace ZooWorld.Animals.Movement.Configuration
{
    [CreateAssetMenu(fileName = "LinearMovement", menuName = "ZooWorld/Movement/Linear")]
    public sealed class LinearMovementConfig : MovementConfig
    {
        [SerializeField, Min(0f)] private float _speed = 3f;

        public float Speed => _speed;
    }
}
