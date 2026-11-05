using UnityEngine;

namespace ZooWorld.World
{
    public interface IScreenBounds
    {
        Rect Area { get; }
        Vector3 Center { get; }

        bool Contains(Vector3 point);
    }
}
