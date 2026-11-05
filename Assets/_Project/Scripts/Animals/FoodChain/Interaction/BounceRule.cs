using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Bounce;

namespace ZooWorld.Animals.FoodChain.Interaction
{
    public sealed class BounceRule : IInteractionRule
    {
        private readonly IBounceService _bounce;

        public BounceRule(IBounceService bounce) =>
            _bounce = bounce;

        public bool CanApply(IAnimal first, IAnimal second) =>
            !first.Role.CanEat(second.Role) && !second.Role.CanEat(first.Role);

        public void Apply(IAnimal first, IAnimal second) =>
            _bounce.Bounce(first, second);
    }
}
