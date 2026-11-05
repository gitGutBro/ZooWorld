using System;
using UnityEngine;
using VContainer.Unity;
using ZooWorld.World.Terrain.Configuration;
using ZooWorld.World.Terrain.Data;
using ZooWorld.World.Terrain.MeshGeneration;
using Object = UnityEngine.Object;

namespace ZooWorld.World.Terrain.Runtime
{
    public sealed class TerrainBuilder : IStartable, IDisposable
    {
        private const string ObjectName = "Terrain";

        private readonly TerrainConfig _config;
        private readonly HeightField _field;

        private GameObject _root;
        private Mesh _mesh;

        public TerrainBuilder(TerrainConfig config, HeightField field)
        {
            _config = config;
            _field = field;
        }

        public void Start()
        {
            _mesh = TerrainMeshBuilder.Build(_field, _config);

            _root = new GameObject(ObjectName);
            _root.AddComponent<MeshFilter>().sharedMesh = _mesh;
            _root.AddComponent<MeshRenderer>().sharedMaterial = _config.Material;

            MeshCollider collider = _root.AddComponent<MeshCollider>();
            collider.sharedMesh = _mesh;
            collider.sharedMaterial = _config.PhysicsMaterial;
        }

        public void Dispose()
        {
            if (_root)
                Object.Destroy(_root);

            if (_mesh)
                Object.Destroy(_mesh);
        }
    }
}
