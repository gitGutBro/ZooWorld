using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using ZooWorld.Animals.Lifecycle;
using ZooWorld.World;

namespace ZooWorld.Animals.Spawning
{
    public sealed class AnimalSpawner : IAsyncStartable
    {
        private readonly SpawnConfig _config;
        private readonly ISpawnSelector _selector;
        private readonly ISpawnPositionProvider _positions;
        private readonly IAnimalFactory _factory;
        private readonly IRandomService _random;

        public AnimalSpawner(
            SpawnConfig config,
            ISpawnSelector selector,
            ISpawnPositionProvider positions,
            IAnimalFactory factory,
            IRandomService random)
        {
            _config = config;
            _selector = selector;
            _positions = positions;
            _factory = factory;
            _random = random;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            while (!cancellation.IsCancellationRequested)
            {
                bool cancelled = await UniTask
                    .Delay(NextDelay(), cancellationToken: cancellation)
                    .SuppressCancellationThrow();

                if (cancelled)
                    return;

                _factory.Create(_selector.Next(), _positions.Next());
            }
        }

        private TimeSpan NextDelay() =>
            TimeSpan.FromSeconds(_random.Range(_config.MinIntervalSeconds, _config.MaxIntervalSeconds));
    }
}
