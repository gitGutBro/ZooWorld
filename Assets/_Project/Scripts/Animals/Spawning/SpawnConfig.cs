using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.Animals.Spawning
{
    [Serializable]
    public sealed class SpawnConfig
    {
        [SerializeField, Min(0f)] private float _minIntervalSeconds = 1f;
        [SerializeField, Min(0f)] private float _maxIntervalSeconds = 2f;
        [Space]
        [SerializeField, Min(0f)] private float _edgePadding = 1f;
        [SerializeField] private float _heightOffset = 0.5f;
        [Space]
        [SerializeField] private SpawnEntry[] _entries = Array.Empty<SpawnEntry>();

        public float MinIntervalSeconds => _minIntervalSeconds;
        public float MaxIntervalSeconds => _maxIntervalSeconds;
        public float EdgePadding => _edgePadding;
        public float HeightOffset => _heightOffset;

        public IReadOnlyList<SpawnEntry> Entries => _entries;
    }
}
