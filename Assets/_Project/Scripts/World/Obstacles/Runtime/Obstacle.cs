using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.World.Obstacles.Runtime
{
    public sealed class Obstacle : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _footprintRadius = 1f;
        [SerializeField] private ObstacleGroundAnchor _groundAnchor = ObstacleGroundAnchor.Lowest;
        [SerializeField, Min(0f)] private float _maxGroundDrop = 0.25f;
        [SerializeField] private Collider[] _colliders = Array.Empty<Collider>();

        public float FootprintRadius => _footprintRadius;
        public ObstacleGroundAnchor GroundAnchor => _groundAnchor;
        public float MaxGroundDrop => _maxGroundDrop;
        public IReadOnlyList<Collider> Colliders => _colliders;

        private void OnValidate()
        {
            if (_colliders.Length > 0)
                return;

            _colliders = GetComponentsInChildren<Collider>();
        }
    }
}
