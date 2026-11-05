using ZooWorld.Animals.Core;
using ZooWorld.World;

namespace ZooWorld.Animals.FoodChain.Duels
{
    public sealed class RandomDuelResolver : IDuelResolver
    {
        private const float FirstWinChance = 0.5f;

        private readonly IRandomService _random;

        public RandomDuelResolver(IRandomService random) =>
            _random = random;

        public IAnimal ResolveWinner(IAnimal first, IAnimal second) =>
            _random.Value < FirstWinChance ? first : second;
    }
}
