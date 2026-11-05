using UnityEngine;
using ZooWorld.World.Terrain.Configuration;

namespace ZooWorld.World.Terrain.Generation
{
    internal static class FractalNoise
    {
        private const float NoiseFloor = 0.2f;
        private const float NoiseCeiling = 0.8f;

        public static float SampleFractalNoise(TerrainConfig config, float x, float y)
        {
            float averageNoise = SumOctaves(config, x, y);
            return NormalizeNoise(averageNoise);
        }

        private static float SumOctaves(TerrainConfig config, float x, float y)
        {
            float sum = 0f;
            float total = 0f;
            float amplitude = 1f;
            float frequency = 1f;

            for (int octave = 0; octave < config.Octaves; octave++)
            {
                sum += SamplePerlinOctave(x, y, frequency, amplitude);
                total += amplitude;
                amplitude *= config.Persistence;
                frequency *= config.Lacunarity;
            }

            return sum / total;
        }

        private static float SamplePerlinOctave(float x, float y, float frequency, float amplitude) =>
            Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;

        private static float NormalizeNoise(float average) =>
            Mathf.Clamp01(Mathf.InverseLerp(NoiseFloor, NoiseCeiling, average));
    }
}
