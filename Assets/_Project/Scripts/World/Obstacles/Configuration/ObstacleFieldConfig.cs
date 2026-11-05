using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZooWorld.World.Obstacles.Configuration
{
    [CreateAssetMenu(fileName = "ObstacleFieldConfig", menuName = "ZooWorld/Obstacle Field")]
    public sealed class ObstacleFieldConfig : ScriptableObject
    {
        [Header("Count")]
        [SerializeField, Min(0)] private int _minCount = 8;
        [SerializeField, Min(0)] private int _maxCount = 12;
        [Space]

        [Header("Placement")]
        [SerializeField, Min(0f)] private float _spacing = 1.8f;
        [SerializeField, Min(0f)] private float _edgePadding = 1f;
        [SerializeField] private Vector2 _scaleRange = new(0.85f, 1.2f);
        [SerializeField, Min(0f)] private float _sinkDepth = 0.08f;
        [Space]

        [Header("Obstacles")]
        [SerializeField] private ObstacleEntry[] _entries = Array.Empty<ObstacleEntry>();

        public int MinCount => _minCount;
        public int MaxCount => _maxCount;

        public float Spacing => _spacing;
        public float EdgePadding => _edgePadding;
        public Vector2 ScaleRange => _scaleRange;
        public float SinkDepth => _sinkDepth;

        public IReadOnlyList<ObstacleEntry> Entries => _entries;
    }
}
