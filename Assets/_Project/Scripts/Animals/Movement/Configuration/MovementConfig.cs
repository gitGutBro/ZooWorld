using UnityEngine;

namespace ZooWorld.Animals.Movement.Configuration
{
    public abstract class MovementConfig : ScriptableObject
    {
        [SerializeField, Min(0.1f)] private float _directionChangeSeconds = 3f;

        public float DirectionChangeSeconds => _directionChangeSeconds;
    }
}
