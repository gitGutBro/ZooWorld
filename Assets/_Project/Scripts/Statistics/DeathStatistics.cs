using System;
using System.Collections.Generic;
using R3;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Roles;
using ZooWorld.Animals.Lifecycle;

namespace ZooWorld.Statistics
{
    public sealed class DeathStatistics : IDeathStatistics, IDisposable
    {
        private readonly Dictionary<FoodChainRole, ReactiveProperty<int>> _deaths = new();
        private readonly IDisposable _subscription;

        public DeathStatistics(IReadOnlyList<FoodChainRole> countedRoles, IAnimalDeathEvents deathEvents)
        {
            Roles = countedRoles;

            foreach (FoodChainRole role in countedRoles)
                _deaths.Add(role, new ReactiveProperty<int>(0));

            _subscription = deathEvents.Died.Subscribe(OnAnimalDied);
        }

        public IReadOnlyList<FoodChainRole> Roles { get; }

        public ReadOnlyReactiveProperty<int> GetDeaths(FoodChainRole role) =>
            _deaths[role];

        public void Dispose()
        {
            _subscription.Dispose();

            foreach (ReactiveProperty<int> counter in _deaths.Values)
                counter.Dispose();
        }

        private void OnAnimalDied(IAnimal animal)
        {
            if (_deaths.TryGetValue(animal.Role, out ReactiveProperty<int> counter))
                counter.Value++;
        }
    }
}
