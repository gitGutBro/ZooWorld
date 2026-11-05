using UnityEngine;

namespace ZooWorld.Animals.Spawning
{
    public interface ISpawnPositionProvider
    {
        Vector3 Next();
    }
}
