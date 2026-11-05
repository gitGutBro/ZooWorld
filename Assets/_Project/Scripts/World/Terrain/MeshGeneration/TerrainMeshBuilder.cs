using UnityEngine;
using UnityEngine.Rendering;
using ZooWorld.World.Terrain.Configuration;
using ZooWorld.World.Terrain.Data;
using ZooWorld.World.Terrain.Grid;

namespace ZooWorld.World.Terrain.MeshGeneration
{
    public static class TerrainMeshBuilder
    {
        private const string MeshName = "Terrain";

        public static Mesh Build(HeightField field, TerrainConfig config)
        {
            int count = field.VerticesX * field.VerticesZ;
            Vector3[] vertices = new Vector3[count];
            Vector3[] normals = new Vector3[count];
            Color[] colors = new Color[count];

            FillVertexData(field, config, vertices, normals, colors);

            return CreateMesh(vertices, normals, colors, TerrainTriangulator.BuildTriangles(field));
        }

        private static void FillVertexData(
            HeightField field,
            TerrainConfig config,
            Vector3[] vertices,
            Vector3[] normals,
            Color[] colors)
        {
            for (int z = 0; z < field.VerticesZ; z++)
            {
                for (int x = 0; x < field.VerticesX; x++)
                {
                    int index = GridMath.CalculateHeightIndex(x, z, field.CellsX);
                    Vector3 position = field.VertexPosition(x, z);
                    Vector2 gradient = field.GradientAt(position.x, position.z);

                    vertices[index] = position;
                    normals[index] = CalculateNormal(gradient);
                    colors[index] = TerrainVertexColoring.CalculateColor(config, position, gradient);
                }
            }
        }

        private static Vector3 CalculateNormal(Vector2 gradient) =>
            new Vector3(-gradient.x, 1f, -gradient.y).normalized;

        private static Mesh CreateMesh(Vector3[] vertices, Vector3[] normals, Color[] colors, int[] triangles)
        {
            Mesh mesh = new Mesh
            {
                name = MeshName,
                indexFormat = IndexFormat.UInt32,
                vertices = vertices,
                normals = normals,
                colors = colors,
                triangles = triangles
            };

            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
