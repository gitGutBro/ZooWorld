using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Statistics
{
    public interface IMealLedger
    {
        int GetMeals(IAnimal animal);
    }
}
