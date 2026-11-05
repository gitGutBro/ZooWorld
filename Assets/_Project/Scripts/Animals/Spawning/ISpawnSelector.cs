using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.Spawning
{
    public interface ISpawnSelector
    {
        AnimalConfig Next();
    }
}
