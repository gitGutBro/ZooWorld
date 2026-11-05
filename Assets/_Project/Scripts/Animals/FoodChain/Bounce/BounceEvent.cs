using UnityEngine;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Bounce
{
    public readonly struct BounceEvent
    {
        public BounceEvent(IAnimal first, IAnimal second, Vector3 point, float strength)
        {
            First = first;
            Second = second;
            Point = point;
            Strength = strength;
        }

        public IAnimal First { get; }
        public IAnimal Second { get; }
        public Vector3 Point { get; }
        public float Strength { get; }
    }
}
