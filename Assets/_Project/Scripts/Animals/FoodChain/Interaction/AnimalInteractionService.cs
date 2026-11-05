using System;
using System.Collections.Generic;
using System.Threading;
using R3;
using VContainer.Unity;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Interaction
{
    public sealed class AnimalInteractionService : IInitializable, IDisposable
    {
        private readonly IAnimalRegistry _registry;
        private readonly IReadOnlyList<IInteractionRule> _rules;
        private readonly CompositeDisposable _disposables = new();
        private readonly CancellationTokenSource _lifetime = new();

        public AnimalInteractionService(IAnimalRegistry registry, IReadOnlyList<IInteractionRule> rules)
        {
            _registry = registry;
            _rules = rules;
        }

        public void Initialize()
        {
            _registry.Added
                .Subscribe(ObserveCollisions)
                .AddTo(_disposables);
        }

        public void Resolve(IAnimal first, IAnimal second)
        {
            if (!first.IsAlive || !second.IsAlive)
                return;

            IInteractionRule rule = FindRule(first, second);
            rule?.Apply(first, second);
        }

        public void Dispose()
        {
            _lifetime.Cancel();
            _lifetime.Dispose();
            _disposables.Dispose();
        }

        private void ObserveCollisions(Animal animal)
        {
            animal.Collided
                .TakeUntil(animal.Died)
                .TakeUntil(_lifetime.Token)
                .Subscribe(OnCollided);
        }

        private void OnCollided(AnimalCollision collision)
        {
            if (!_registry.TryGet(collision.Other, out Animal other))
                return;

            Resolve(collision.Self, other);
        }

        private IInteractionRule FindRule(IAnimal first, IAnimal second)
        {
            for (int i = 0; i < _rules.Count; i++)
            {
                IInteractionRule rule = _rules[i];

                if (rule.CanApply(first, second))
                    return rule;
            }

            return null;
        }
    }
}
