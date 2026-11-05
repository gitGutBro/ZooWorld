using NUnit.Framework;
using UnityEngine;
using ZooWorld.World.Terrain.Configuration;
using ZooWorld.World.Terrain.Data;
using ZooWorld.World.Terrain.MeshGeneration;

namespace ZooWorld.Tests
{
    public sealed class TerrainMeshBuilderTests
    {
        private const int Cells = 4;
        private const int IndicesPerCell = 6;
        private const float Tolerance = 0.001f;

        private TerrainConfig _config;
        private HeightField _flat;
        private Mesh _mesh;

        [SetUp]
        public void SetUp()
        {
            _config = ScriptableObject.CreateInstance<TerrainConfig>();
            _flat = TerrainTestData.CreatePlane(Cells, 1f, 0.5f, 0f);
            _mesh = TerrainMeshBuilder.Build(_flat, _config);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_mesh);
            Object.DestroyImmediate(_config);
        }

        [Test]
        public void Build_CreatesVertexPerGridPointAndTwoTrianglesPerCell()
        {
            Assert.AreEqual((Cells + 1) * (Cells + 1), _mesh.vertexCount);
            Assert.AreEqual(Cells * Cells * IndicesPerCell, _mesh.triangles.Length);
        }

        [Test]
        public void Build_FlatField_HasUpwardNormals()
        {
            foreach (Vector3 normal in _mesh.normals)
                Assert.AreEqual(1f, normal.y, Tolerance);
        }

        [Test]
        public void Build_TrianglesFaceUpward()
        {
            int[] triangles = _mesh.triangles;
            Vector3[] vertices = _mesh.vertices;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 first = vertices[triangles[i]];
                Vector3 normal = Vector3.Cross(vertices[triangles[i + 1]] - first, vertices[triangles[i + 2]] - first);
                Assert.Greater(normal.y, 0f);
            }
        }

        [Test]
        public void Build_AssignsOpaqueVertexColors()
        {
            Color[] colors = _mesh.colors;

            Assert.AreEqual(_mesh.vertexCount, colors.Length);
            Assert.IsTrue(System.Array.TrueForAll(colors, color => Mathf.Approximately(color.a, 1f)));
        }
    }
}
