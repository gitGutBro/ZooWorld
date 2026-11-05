using ZooWorld.World.Terrain.Data;
using ZooWorld.World.Terrain.Grid;

namespace ZooWorld.World.Terrain.MeshGeneration
{
    internal static class TerrainTriangulator
    {
        private const int IndicesPerCell = 6;

        public static int[] BuildTriangles(HeightField field)
        {
            int[] triangles = new int[field.CellsX * field.CellsZ * IndicesPerCell];
            int cursor = 0;

            for (int z = 0; z < field.CellsZ; z++)
                for (int x = 0; x < field.CellsX; x++)
                    AddCellTriangles(triangles, ref cursor, field, x, z);

            return triangles;
        }

        private static void AddCellTriangles(int[] triangles, ref int cursor, HeightField field, int x, int z)
        {
            int bottomLeft = GridMath.CalculateHeightIndex(x, z, field.CellsX);
            int bottomRight = bottomLeft + 1;
            int topLeft = bottomLeft + field.VerticesX;
            int topRight = topLeft + 1;

            AddTriangle(triangles, ref cursor, bottomLeft, topLeft, bottomRight);
            AddTriangle(triangles, ref cursor, bottomRight, topLeft, topRight);
        }

        private static void AddTriangle(int[] triangles, ref int cursor, int a, int b, int c)
        {
            triangles[cursor++] = a;
            triangles[cursor++] = b;
            triangles[cursor++] = c;
        }
    }
}
