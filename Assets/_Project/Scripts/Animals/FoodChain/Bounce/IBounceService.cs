using R3;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Bounce
{
    public interface IBounceService
    {
        Observable<BounceEvent> Bounced { get; }

        void Bounce(IAnimal first, IAnimal second);
    }
}
