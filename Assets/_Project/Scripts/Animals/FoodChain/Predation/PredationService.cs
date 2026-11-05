using System;
using R3;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Predation
{
    public sealed class PredationService : IPredationService, IDisposable
    {
        private readonly Subject<PredationEvent> _ate = new();

        public Observable<PredationEvent> Ate => _ate;

        public void Eat(IAnimal eater, IAnimal victim)
        {
            victim.Kill();
            _ate.OnNext(new PredationEvent(eater, victim));
        }

        public void Dispose() =>
            _ate.Dispose();
    }
}
