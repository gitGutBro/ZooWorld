using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Predation
{
    public readonly struct PredationEvent
    {
        public PredationEvent(IAnimal eater, IAnimal victim)
        {
            Eater = eater;
            Victim = victim;
        }

        public IAnimal Eater { get; }
        public IAnimal Victim { get; }
    }
}
