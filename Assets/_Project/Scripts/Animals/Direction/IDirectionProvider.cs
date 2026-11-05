using UnityEngine;

namespace ZooWorld.Animals.Direction
{
    public interface IDirectionProvider
    {
        Vector3 Current { get; }

        void Tick(float deltaTime, Vector3 position);
    }
}
