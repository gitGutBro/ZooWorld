using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Statistics;

namespace ZooWorld.Animals.FoodChain.Duels
{
    public sealed class MostMealsDuelResolver : IDuelResolver
    {
        private readonly IMealLedger _ledger;
        private readonly IDuelResolver _tieBreaker;

        public MostMealsDuelResolver(IMealLedger ledger, IDuelResolver tieBreaker)
        {
            _ledger = ledger;
            _tieBreaker = tieBreaker;
        }

        public IAnimal ResolveWinner(IAnimal first, IAnimal second)
        {
            int firstMeals = _ledger.GetMeals(first);
            int secondMeals = _ledger.GetMeals(second);

            if (firstMeals == secondMeals)
                return _tieBreaker.ResolveWinner(first, second);

            return firstMeals > secondMeals ? first : second;
        }
    }
}
