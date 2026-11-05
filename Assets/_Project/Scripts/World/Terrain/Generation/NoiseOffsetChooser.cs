using UnityEngine;
using ZooWorld.World.Terrain.Configuration;

namespace ZooWorld.World.Terrain.Generation
{
    internal static class NoiseOffsetChooser
    {
        private const float MinNoiseOffset = 100f;
        private const float NoiseOffsetSpan = 900f;
        private const float SeedStrideX = 37.7f;
        private const float SeedStrideZ = 91.3f;

        public static Vector2 ChooseNoiseOffset(TerrainConfig config, IRandomService random) =>
            config.RandomizeSeed
                ? ChooseRandomNoiseOffset(random)
                : ChooseSeededNoiseOffset(config.Seed);

        private static Vector2 ChooseRandomNoiseOffset(IRandomService random) =>
            new(ChooseRandomOffsetAxis(random),
                ChooseRandomOffsetAxis(random));

        private static Vector2 ChooseSeededNoiseOffset(int seed) =>
            new(ChooseSeededOffsetAxis(seed, SeedStrideX),
                ChooseSeededOffsetAxis(seed, SeedStrideZ));

        private static float ChooseRandomOffsetAxis(IRandomService random) =>
            random.Range(MinNoiseOffset, MinNoiseOffset + NoiseOffsetSpan);

        private static float ChooseSeededOffsetAxis(int seed, float stride) =>
            MinNoiseOffset + Mathf.Abs(seed * stride) % NoiseOffsetSpan;
    }
}
