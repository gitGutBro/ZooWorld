using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.Lifecycle
{
    public sealed class AnimalPool : IAnimalPool
    {
        private const string RootName = "Animals";

        private readonly Dictionary<AnimalConfig, ObjectPool<Animal>> _pools = new();

        private Transform _root;

        public Animal Get(AnimalConfig config) =>
            GetPool(config).Get();

        public void Release(Animal animal) =>
            GetPool(animal.Config).Release(animal);

        private ObjectPool<Animal> GetPool(AnimalConfig config)
        {
            if (_pools.TryGetValue(config, out ObjectPool<Animal> pool))
                return pool;

            pool = new ObjectPool<Animal>(
                () => Object.Instantiate(config.Prefab, GetRoot()),
                actionOnGet:     animal => animal.gameObject.SetActive(true),
                actionOnRelease: animal => animal.gameObject.SetActive(false),
                actionOnDestroy: animal => Object.Destroy(animal.gameObject));
            _pools.Add(config, pool);

            return pool;
        }

        private Transform GetRoot()
        {
            if (!_root)
                _root = new GameObject(RootName).transform;

            return _root;
        }
    }
}
