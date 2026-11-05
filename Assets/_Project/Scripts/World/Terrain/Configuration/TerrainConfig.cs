using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace ZooWorld.World.Terrain.Configuration
{
    [CreateAssetMenu(fileName = "Terrain", menuName = "ZooWorld/Terrain")]
    public sealed class TerrainConfig : ScriptableObject
    {
        [Header("Geometry")]
        [SerializeField] private Vector2 _size = new(64f, 64f);
        [SerializeField, Min(0.1f)] private float _cellSize = 0.3f;
        [SerializeField] private float _minHeight = -0.4f;
        [SerializeField] private float _maxHeight = 2f;
        [Space]

        [Header("Noise")]
        [SerializeField, Min(0.1f)] private float _noiseScale = 10f;
        [SerializeField, Range(1, 6)] private int _octaves = 4;
        [SerializeField, Range(0.1f, 0.9f)] private float _persistence = 0.4f;
        [SerializeField, Min(1f)] private float _lacunarity = 2.1f;
        [SerializeField, Min(0.1f)] private float _redistribution = 1.2f;
        [Space]

        [Header("Seed")]
        [SerializeField] private bool _randomizeSeed = true;
        [SerializeField, HideIf(nameof(_randomizeSeed))] private int _seed = 1;
        [Space]

        [Header("Movement")]
        [SerializeField, Min(0f)] private float _slopeInfluence = 1f;
        [SerializeField, Range(0.1f, 1f)] private float _minSpeedMultiplier = 0.55f;
        [SerializeField, Range(1f, 2f)] private float _maxSpeedMultiplier = 1.4f;
        [SerializeField, Range(0f, 1f)] private float _valleyLevel = 0.25f;
        [SerializeField, Range(0.1f, 1f)] private float _valleySpeedMultiplier = 0.7f;
        [Space]

        [Header("Materials")]
        [SerializeField] private Material _material;
        [SerializeField] private PhysicMaterial _physicsMaterial;
        [Space]

        [Header("Colors")]
        [SerializeField] private Color _steepColor = new(0.45f, 0.36f, 0.27f);
        [SerializeField, Range(5f, 60f)] private float _steepSlopeStart = 24f;
        [SerializeField, Range(0f, 0.3f)] private float _colorJitter = 0.14f;

        [Space]
        [SerializeField] private TerrainColorStop[] _colorStops =
        {
            new(position: 0f,    new Color(0.24f, 0.21f, 0.12f)),
            new(position: 0.2f,  new Color(0.3f, 0.36f, 0.15f)),
            new(position: 0.4f,  new Color(0.28f, 0.47f, 0.18f)),
            new(position: 0.62f, new Color(0.42f, 0.58f, 0.22f)),
            new(position: 0.82f, new Color(0.62f, 0.62f, 0.32f)),
            new(position: 1f,    new Color(0.7f, 0.66f, 0.52f)),
        };

        public Vector2 Size => _size;
        public float CellSize => _cellSize;
        public float MinHeight => _minHeight;
        public float MaxHeight => _maxHeight;

        public float NoiseScale => _noiseScale;
        public int Octaves => _octaves;
        public float Persistence => _persistence;
        public float Lacunarity => _lacunarity;
        public float Redistribution => _redistribution;

        public bool RandomizeSeed => _randomizeSeed;
        public int Seed => _seed;

        public float SlopeInfluence => _slopeInfluence;
        public float MinSpeedMultiplier => _minSpeedMultiplier;
        public float MaxSpeedMultiplier => _maxSpeedMultiplier;
        public float ValleyLevel => _valleyLevel;
        public float ValleySpeedMultiplier => _valleySpeedMultiplier;

        public Material Material => _material;
        public PhysicMaterial PhysicsMaterial => _physicsMaterial;

        public Color SteepColor => _steepColor;
        public float SteepSlopeStart => _steepSlopeStart;
        public float ColorJitter => _colorJitter;
        public IReadOnlyList<TerrainColorStop> ColorStops => _colorStops;
    }
}
