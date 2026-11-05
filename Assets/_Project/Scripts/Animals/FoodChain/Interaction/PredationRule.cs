using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Predation;

namespace ZooWorld.Animals.FoodChain.Interaction
{
    public sealed class PredationRule : IInteractionRule
    {
        private readonly IPredationService _predation;

        public PredationRule(IPredationService predation) =>
            _predation = predation;

        public bool CanApply(IAnimal first, IAnimal second) =>
            first.Role.CanEat(second.Role) != second.Role.CanEat(first.Role);

        public void Apply(IAnimal first, IAnimal second)
        {
            if (first.Role.CanEat(second.Role))
            {
                _predation.Eat(eater: first, victim: second);
                return;
            }

            _predation.Eat(eater: second, victim: first);
        }
    }
}
