using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace ZooWorld.Animals.Core
{
    public sealed class AnimalRegistry : IMutableAnimalRegistry, IDisposable
    {
        private readonly List<Animal> _animals = new();
        private readonly Dictionary<Rigidbody, Animal> _animalsByBody = new();
        private readonly Subject<Animal> _added = new();

        public IReadOnlyList<Animal> Animals => _animals;
        public Observable<Animal> Added => _added;

        public void Add(Animal animal)
        {
            _animals.Add(animal);
            _animalsByBody.Add(animal.Body, animal);
            _added.OnNext(animal);
        }

        public void Remove(Animal animal)
        {
            _animals.Remove(animal);
            _animalsByBody.Remove(animal.Body);
        }

        public bool TryGet(Rigidbody body, out Animal animal) =>
            _animalsByBody.TryGetValue(body, out animal);

        public void Dispose() =>
            _added.Dispose();
    }
}
