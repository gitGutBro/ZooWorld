using System;
using UnityEngine;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.Spawning
{
    [Serializable]
    public sealed class SpawnEntry
    {
        [SerializeField] private AnimalConfig _animal;
        [SerializeField, Min(0f)] private float _weight = 1f;

        public AnimalConfig Animal => _animal;
        public float Weight => _weight;
    }
}
