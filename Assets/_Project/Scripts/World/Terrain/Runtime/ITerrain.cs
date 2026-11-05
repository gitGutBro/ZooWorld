using UnityEngine;

namespace ZooWorld.World.Terrain.Runtime
{
    public interface ITerrain
    {
        float HeightAt(Vector3 position);

        float GetSpeedMultiplier(Vector3 position, Vector3 direction);
    }
}
