using UnityEngine;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.Lifecycle
{
    public interface IAnimalFactory
    {
        IAnimal Create(AnimalConfig config, Vector3 position);
    }
}
