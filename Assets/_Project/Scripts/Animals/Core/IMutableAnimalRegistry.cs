namespace ZooWorld.Animals.Core
{
    public interface IMutableAnimalRegistry : IAnimalRegistry
    {
        void Add(Animal animal);
        void Remove(Animal animal);
    }
}
