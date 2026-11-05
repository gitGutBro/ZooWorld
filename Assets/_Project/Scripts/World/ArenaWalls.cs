using System;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace ZooWorld.World
{
    public sealed class ArenaWalls : IStartable, ITickable, IDisposable
    {
        public const string ObjectName = "ArenaWalls";

        private const float Thickness = 5f;
        private const float Height = 20f;
        private const float BaseY = -5f;
        private const int WallCount = 4;
        private const float Bounciness = 0.2f;

        private readonly IScreenBounds _bounds;
        private readonly BoxCollider[] _walls = new BoxCollider[WallCount];

        private GameObject _root;
        private Rect _appliedArea;

        public ArenaWalls(IScreenBounds bounds) =>
            _bounds = bounds;

        public void Start()
        {
            _root = new GameObject(ObjectName);
            PhysicMaterial material = CreateSlipperyMaterial();

            for (int i = 0; i < WallCount; i++)
            {
                _walls[i] = _root.AddComponent<BoxCollider>();
                _walls[i].sharedMaterial = material;
            }

            Apply();
        }

        public void Tick()
        {
            if (_bounds.Area != _appliedArea)
                Apply();
        }

        public void Dispose()
        {
            if (_root)
                Object.Destroy(_root);
        }

        private void Apply()
        {
            Rect area = _bounds.Area;
            _appliedArea = area;

            const float middleY = BaseY + Height * 0.5f;
            const float offset = Thickness * 0.5f;

            float alongX = area.width + Thickness * 2f;
            float alongZ = area.height + Thickness * 2f;

            Configure(_walls[0], new Vector3(area.xMin - offset, middleY, area.center.y), new Vector3(Thickness, Height, alongZ));
            Configure(_walls[1], new Vector3(area.xMax + offset, middleY, area.center.y), new Vector3(Thickness, Height, alongZ));
            Configure(_walls[2], new Vector3(area.center.x, middleY, area.yMin - offset), new Vector3(alongX, Height, Thickness));
            Configure(_walls[3], new Vector3(area.center.x, middleY, area.yMax + offset), new Vector3(alongX, Height, Thickness));
        }

        private static void Configure(BoxCollider wall, Vector3 center, Vector3 size)
        {
            wall.center = center;
            wall.size = size;
        }

        private static PhysicMaterial CreateSlipperyMaterial()
        {
            return new PhysicMaterial(ObjectName)
            {
                dynamicFriction = 0f,
                staticFriction = 0f,
                frictionCombine = PhysicMaterialCombine.Minimum,
                bounciness = Bounciness,
            };
        }
    }
}
