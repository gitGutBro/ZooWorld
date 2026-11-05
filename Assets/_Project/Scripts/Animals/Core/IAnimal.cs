using UnityEngine;
using ZooWorld.Animals.FoodChain.Roles;

namespace ZooWorld.Animals.Core
{
    public interface IAnimal
    {
        AnimalConfig Config { get; }
        FoodChainRole Role { get; }
        bool IsAlive { get; }
        bool IsKnockedBack { get; }
        Vector3 Position { get; }
        Vector3 Velocity { get; }

        void Kill();
        void Push(Vector3 velocityChange, float recoverySeconds);
    }
}
