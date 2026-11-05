using System;
using System.Threading;
using R3;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.Lifecycle
{
    public sealed class AnimalLifecycleService : IAnimalLifecycleService, IAnimalDeathEvents, IDisposable
    {
        private readonly IMutableAnimalRegistry _registry;
        private readonly IAnimalPool _pool;
        private readonly Subject<IAnimal> _died = new();
        private readonly CancellationTokenSource _lifetime = new();

        public AnimalLifecycleService(IMutableAnimalRegistry registry, IAnimalPool pool)
        {
            _registry = registry;
            _pool = pool;
        }

        public Observable<IAnimal> Died => _died;

        public void Track(Animal animal)
        {
            animal.Died
                .Take(1)
                .TakeUntil(_lifetime.Token)
                .Subscribe(OnDied);
        }

        public void Dispose()
        {
            _lifetime.Cancel();
            _lifetime.Dispose();
            _died.Dispose();
        }

        private void OnDied(Animal animal)
        {
            _registry.Remove(animal);
            _pool.Release(animal);
            _died.OnNext(animal);
        }
    }
}
