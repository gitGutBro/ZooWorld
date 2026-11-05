using System;
using UnityEngine;

namespace ZooWorld.Animals.FoodChain.Bounce
{
    [Serializable]
    public sealed class BounceConfig
    {
        [SerializeField, Min(0f)] private float _minSpeedChange = 2f;
        [SerializeField, Min(0f)] private float _speedChangePerClosingSpeed = 0.6f;
        [SerializeField, Min(0f)] private float _maxSpeedChange = 6f;
        [SerializeField, Range(0f, 1f)] private float _lift = 0.25f;
        [SerializeField, Min(0f)] private float _recoverySeconds = 0.3f;

        public float MinSpeedChange => _minSpeedChange;
        public float SpeedChangePerClosingSpeed => _speedChangePerClosingSpeed;
        public float MaxSpeedChange => _maxSpeedChange;
        public float Lift => _lift;
        public float RecoverySeconds => _recoverySeconds;
    }
}
