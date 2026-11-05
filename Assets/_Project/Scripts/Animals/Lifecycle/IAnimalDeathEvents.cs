using R3;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.Lifecycle
{
    public interface IAnimalDeathEvents
    {
        Observable<IAnimal> Died { get; }
    }
}
