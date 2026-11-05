using UnityEngine;
using ZooWorld.World.Terrain.Configuration;

namespace ZooWorld.World.Terrain.MeshGeneration
{
    internal static class TerrainVertexColoring
    {
        private const float CoarseJitterFrequency = 0.7f;
        private const float FineJitterFrequency = 2f;
        private const float CoarseJitterWeight = 0.6f;
        private const float JitterOffset = 321f;

        public static Color CalculateColor(TerrainConfig config, Vector3 position, Vector2 gradient)
        {
            float normalizedHeight = CalculateNormalizedHeight(config, position.y);
            float slopeDegrees = CalculateSlopeDegrees(gradient);
            return TerrainPalette.Evaluate(config, normalizedHeight, slopeDegrees, CalculateJitter(position));
        }

        private static float CalculateNormalizedHeight(TerrainConfig config, float height) =>
            Mathf.InverseLerp(config.MinHeight, config.MaxHeight, height);

        private static float CalculateSlopeDegrees(Vector2 gradient) =>
            Mathf.Atan(gradient.magnitude) * Mathf.Rad2Deg;

        private static float CalculateJitter(Vector3 position)
        {
            float coarse = SampleJitterNoise(position, CoarseJitterFrequency);
            float fine = SampleJitterNoise(position, FineJitterFrequency);
            return Mathf.Lerp(fine, coarse, CoarseJitterWeight) * 2f - 1f;
        }

        private static float SampleJitterNoise(Vector3 position, float frequency) =>
            Mathf.PerlinNoise(
                position.x * frequency + JitterOffset,
                position.z * frequency + JitterOffset);
    }
}
