using System;
using System.Collections.Generic;
using R3;
using VContainer.Unity;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Predation;
using ZooWorld.Animals.Lifecycle;

namespace ZooWorld.Animals.FoodChain.Statistics
{
    public sealed class MealLedger : IMealLedger, IInitializable, IDisposable
    {
        private readonly IPredationService _predation;
        private readonly IAnimalDeathEvents _deathEvents;
        private readonly Dictionary<IAnimal, int> _meals = new();
        private readonly CompositeDisposable _disposables = new();

        public MealLedger(IPredationService predation, IAnimalDeathEvents deathEvents)
        {
            _predation = predation;
            _deathEvents = deathEvents;
        }

        public void Initialize()
        {
            _predation.Ate.Subscribe(OnAte).AddTo(_disposables);
            _deathEvents.Died.Subscribe(Forget).AddTo(_disposables);
        }

        public int GetMeals(IAnimal animal) =>
            _meals.GetValueOrDefault(animal, 0);

        public void Dispose() =>
            _disposables.Dispose();

        private void OnAte(PredationEvent predation) =>
            _meals[predation.Eater] = GetMeals(predation.Eater) + 1;

        private void Forget(IAnimal animal) =>
            _meals.Remove(animal);
    }
}
