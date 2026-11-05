using R3;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Predation
{
    public interface IPredationService
    {
        Observable<PredationEvent> Ate { get; }

        void Eat(IAnimal eater, IAnimal victim);
    }
}
