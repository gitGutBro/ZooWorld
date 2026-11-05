using System.Collections.Generic;
using R3;
using UnityEngine;

namespace ZooWorld.Animals.Core
{
    public interface IAnimalRegistry
    {
        IReadOnlyList<Animal> Animals { get; }
        Observable<Animal> Added { get; }

        bool TryGet(Rigidbody body, out Animal animal);
    }
}
