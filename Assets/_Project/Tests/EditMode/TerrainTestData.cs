using ZooWorld.World.Terrain.Data;

namespace ZooWorld.Tests
{
    internal static class TerrainTestData
    {
        public static HeightField CreatePlane(int cells, float cellSize, float baseHeight, float slopeAlongX)
        {
            int vertices = cells + 1;
            float[] heights = new float[vertices * vertices];

            for (int z = 0; z < vertices; z++)
            {
                for (int x = 0; x < vertices; x++)
                {
                    float worldX = (x - cells * 0.5f) * cellSize;
                    heights[z * vertices + x] = baseHeight + worldX * slopeAlongX;
                }
            }

            return new HeightField(cells, cells, cellSize, heights);
        }
    }
}
