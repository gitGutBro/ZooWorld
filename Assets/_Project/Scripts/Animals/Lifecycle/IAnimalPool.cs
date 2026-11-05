using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.Lifecycle
{
    public interface IAnimalPool
    {
        Animal Get(AnimalConfig config);
        void Release(Animal animal);
    }
}
