using System;
using System.Collections.Generic;
using System.Linq;
using ZooWorld.Animals.Core;
using ZooWorld.World;

namespace ZooWorld.Animals.Spawning
{
    public sealed class WeightedSpawnSelector : ISpawnSelector
    {
        private readonly IReadOnlyList<SpawnEntry> _entries;
        private readonly IRandomService _random;
        private readonly float _totalWeight;

        public WeightedSpawnSelector(SpawnConfig config, IRandomService random)
        {
            _entries = config.Entries.Where(entry => entry.Weight > 0f).ToArray();
            _random = random;
            _totalWeight = CalculateTotalWeight(_entries);

            if (_totalWeight <= 0f)
                throw new InvalidOperationException("Spawn config must contain at least one entry with positive weight.");

            if (_entries.Any(entry => !entry.Animal))
                throw new InvalidOperationException("Every spawn config entry with positive weight must reference an animal.");
        }

        public AnimalConfig Next()
        {
            float roll = _random.Range(0f, _totalWeight);

            foreach (SpawnEntry entry in _entries)
            {
                roll -= entry.Weight;

                if (roll <= 0f)
                    return entry.Animal;
            }

            return _entries[^1].Animal;
        }

        private static float CalculateTotalWeight(IReadOnlyList<SpawnEntry> entries) =>
            entries.Sum(entry => entry.Weight);
    }
}
