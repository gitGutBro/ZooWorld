using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.Lifecycle
{
    public sealed class AnimalSimulation : IFixedTickable
    {
        private readonly IAnimalRegistry _registry;

        public AnimalSimulation(IAnimalRegistry registry) =>
            _registry = registry;

        public void FixedTick()
        {
            float deltaTime = Time.fixedDeltaTime;
            IReadOnlyList<Animal> animals = _registry.Animals;

            for (int i = animals.Count - 1; i >= 0; i--)
                animals[i].Tick(deltaTime);
        }
    }
}
