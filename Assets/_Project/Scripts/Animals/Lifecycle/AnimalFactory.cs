using UnityEngine;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.Direction;
using ZooWorld.Animals.Movement;
using ZooWorld.Animals.Movement.Factory;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.Animals.Lifecycle
{
    public sealed class AnimalFactory : IAnimalFactory
    {
        private readonly ITerrain _terrain;
        private readonly IAnimalPool _pool;
        private readonly IMovementStrategyFactory _movements;
        private readonly IDirectionProviderFactory _directions;
        private readonly IMutableAnimalRegistry _registry;
        private readonly IAnimalLifecycleService _lifecycle;

        public AnimalFactory(
            ITerrain terrain,
            IAnimalPool pool,
            IMovementStrategyFactory movements,
            IDirectionProviderFactory directions,
            IMutableAnimalRegistry registry,
            IAnimalLifecycleService lifecycle)
        {
            _terrain = terrain;
            _pool = pool;
            _movements = movements;
            _directions = directions;
            _registry = registry;
            _lifecycle = lifecycle;
        }

        public IAnimal Create(AnimalConfig config, Vector3 position)
        {
            Animal animal = _pool.Get(config);
            IDirectionProvider direction = _directions.Create(config.Movement);
            animal.Place(position, Quaternion.LookRotation(direction.Current, Vector3.up));

            MovementContext context = new MovementContext(animal.Body, direction, _terrain);
            animal.Initialize(config, _movements.Create(config.Movement, context));

            _registry.Add(animal);
            _lifecycle.Track(animal);
            return animal;
        }
    }
}
