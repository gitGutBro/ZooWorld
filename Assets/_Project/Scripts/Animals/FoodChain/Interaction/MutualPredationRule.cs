using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Duels;
using ZooWorld.Animals.FoodChain.Predation;

namespace ZooWorld.Animals.FoodChain.Interaction
{
    public sealed class MutualPredationRule : IInteractionRule
    {
        private readonly IDuelResolver _duel;
        private readonly IPredationService _predation;

        public MutualPredationRule(IDuelResolver duel, IPredationService predation)
        {
            _duel = duel;
            _predation = predation;
        }

        public bool CanApply(IAnimal first, IAnimal second) =>
            first.Role.CanEat(second.Role) && second.Role.CanEat(first.Role);

        public void Apply(IAnimal first, IAnimal second)
        {
            IAnimal winner = _duel.ResolveWinner(first, second);
            IAnimal loser = winner == first ? second : first;
            _predation.Eat(winner, loser);
        }
    }
}
