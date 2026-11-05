using UnityEngine;

namespace ZooWorld.World.Terrain.Grid
{
    internal static class GridMath
    {
        public static int CalculateCellCount(float length, float cellSize) =>
            Mathf.CeilToInt(length / cellSize);

        public static int CalculateNodeCount(int cells) =>
            cells + 1;

        public static float ConvertGridIndexToWorldCoordinate(int index, int cells, float cellSize) =>
            (index - cells * 0.5f) * cellSize;

        public static int CalculateHeightIndex(int x, int z, int cellsX) =>
            z * CalculateNodeCount(cellsX) + x;
    }
}
