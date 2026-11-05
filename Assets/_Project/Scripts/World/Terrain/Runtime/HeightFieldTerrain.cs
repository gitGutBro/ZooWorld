using UnityEngine;
using ZooWorld.World.Terrain.Configuration;
using ZooWorld.World.Terrain.Data;

namespace ZooWorld.World.Terrain.Runtime
{
    public sealed class HeightFieldTerrain : ITerrain
    {
        private const float ValleyBlend = 0.1f;

        private readonly TerrainConfig _config;
        private readonly HeightField _field;

        public HeightFieldTerrain(TerrainConfig config, HeightField field)
        {
            _config = config;
            _field = field;
        }

        public float HeightAt(Vector3 position) =>
            _field.Sample(position.x, position.z);

        public float GetSpeedMultiplier(Vector3 position, Vector3 direction) =>
            SlopeMultiplier(position, direction) * ValleyMultiplier(position);

        private float SlopeMultiplier(Vector3 position, Vector3 direction)
        {
            Vector2 gradient = _field.GradientAt(position.x, position.z);
            float rise = gradient.x * direction.x + gradient.y * direction.z;
            return Mathf.Clamp(1f - rise * _config.SlopeInfluence, _config.MinSpeedMultiplier, _config.MaxSpeedMultiplier);
        }

        private float ValleyMultiplier(Vector3 position)
        {
            float normalizedHeight = Mathf.InverseLerp(_config.MinHeight, _config.MaxHeight, HeightAt(position));
            float outOfValley = Mathf.InverseLerp(_config.ValleyLevel, _config.ValleyLevel + ValleyBlend, normalizedHeight);
            return Mathf.Lerp(_config.ValleySpeedMultiplier, 1f, Mathf.SmoothStep(0f, 1f, outOfValley));
        }
    }
}
