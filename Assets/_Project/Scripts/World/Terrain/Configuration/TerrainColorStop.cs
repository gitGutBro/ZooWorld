using System;
using UnityEngine;

namespace ZooWorld.World.Terrain.Configuration
{
    [Serializable]
    public struct TerrainColorStop
    {
        [SerializeField, Range(0f, 1f)] private float _position;
        [SerializeField] private Color _color;

        public TerrainColorStop(float position, Color color)
        {
            _position = position;
            _color = color;
        }

        public float Position => _position;
        public Color Color => _color;
    }
}
