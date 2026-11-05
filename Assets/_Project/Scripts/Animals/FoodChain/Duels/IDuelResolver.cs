using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Duels
{
    public interface IDuelResolver
    {
        IAnimal ResolveWinner(IAnimal first, IAnimal second);
    }
}
