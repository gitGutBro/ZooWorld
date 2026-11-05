using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Interaction
{
    public interface IInteractionRule
    {
        bool CanApply(IAnimal first, IAnimal second);
        void Apply(IAnimal first, IAnimal second);
    }
}
