using System;
using ZooWorld.World;

namespace ZooWorld.Tests
{
    internal sealed class SeededRandomService : IRandomService
    {
        private readonly Random _random;

        public SeededRandomService(int seed) =>
            _random = new Random(seed);

        public float Value => (float)_random.NextDouble();

        public float Range(float minInclusive, float maxInclusive) =>
            minInclusive + Value * (maxInclusive - minInclusive);
    }
}
