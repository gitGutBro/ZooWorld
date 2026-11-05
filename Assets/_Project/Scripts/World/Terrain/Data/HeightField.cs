using UnityEngine;
using ZooWorld.World.Terrain.Grid;

namespace ZooWorld.World.Terrain.Data
{
    public sealed class HeightField
    {
        private readonly float[] _heights;
        private readonly Vector2 _origin;

        public HeightField(int cellsX, int cellsZ, float cellSize, float[] heights)
        {
            CellsX = cellsX;
            CellsZ = cellsZ;
            CellSize = cellSize;
            _heights = heights;
            _origin = new Vector2(
                GridMath.ConvertGridIndexToWorldCoordinate(0, cellsX, cellSize),
                GridMath.ConvertGridIndexToWorldCoordinate(0, cellsZ, cellSize));
        }

        public int CellsX { get; }
        public int CellsZ { get; }
        public float CellSize { get; }

        public int VerticesX => GridMath.CalculateNodeCount(CellsX);
        public int VerticesZ => GridMath.CalculateNodeCount(CellsZ);

        public float VertexHeight(int x, int z) =>
            _heights[GridMath.CalculateHeightIndex(x, z, CellsX)];

        public Vector3 VertexPosition(int x, int z) =>
            new(GridMath.ConvertGridIndexToWorldCoordinate(x, CellsX, CellSize),
                VertexHeight(x, z),
                GridMath.ConvertGridIndexToWorldCoordinate(z, CellsZ, CellSize));

        public float Sample(float worldX, float worldZ)
        {
            float fx = Mathf.Clamp((worldX - _origin.x) / CellSize, 0f, CellsX);
            float fz = Mathf.Clamp((worldZ - _origin.y) / CellSize, 0f, CellsZ);
            int x = Mathf.Min((int)fx, CellsX - 1);
            int z = Mathf.Min((int)fz, CellsZ - 1);

            float near = Mathf.Lerp(VertexHeight(x, z), VertexHeight(x + 1, z), fx - x);
            float far = Mathf.Lerp(VertexHeight(x, z + 1), VertexHeight(x + 1, z + 1), fx - x);
            return Mathf.Lerp(near, far, fz - z);
        }

        public Vector2 GradientAt(float worldX, float worldZ)
        {
            float alongX = Sample(worldX + CellSize, worldZ) - Sample(worldX - CellSize, worldZ);
            float alongZ = Sample(worldX, worldZ + CellSize) - Sample(worldX, worldZ - CellSize);
            return new Vector2(alongX, alongZ) / (2f * CellSize);
        }
    }
}
