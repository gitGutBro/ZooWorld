using UnityEngine;
using ZooWorld.World.Terrain.Configuration;
using ZooWorld.World.Terrain.Data;
using ZooWorld.World.Terrain.Grid;

namespace ZooWorld.World.Terrain.Generation
{
    public static class TerrainGenerator
    {
        public static HeightField GenerateHeightField(TerrainConfig config, IRandomService random)
        {
            Vector2 offset = NoiseOffsetChooser.ChooseNoiseOffset(config, random);
            int cellsX = GridMath.CalculateCellCount(config.Size.x, config.CellSize);
            int cellsZ = GridMath.CalculateCellCount(config.Size.y, config.CellSize);
            float[] heights = CalculateHeights(config, cellsX, cellsZ, offset);

            return new HeightField(cellsX, cellsZ, config.CellSize, heights);
        }

        private static float[] CalculateHeights(TerrainConfig config, int cellsX, int cellsZ, Vector2 offset)
        {
            int nodesX = GridMath.CalculateNodeCount(cellsX);
            int nodesZ = GridMath.CalculateNodeCount(cellsZ);

            float[] heights = new float[nodesX * nodesZ];

            for (int z = 0; z < nodesZ; z++)
            {
                for (int x = 0; x < nodesX; x++)
                {
                    float worldX = GridMath.ConvertGridIndexToWorldCoordinate(x, cellsX, config.CellSize);
                    float worldZ = GridMath.ConvertGridIndexToWorldCoordinate(z, cellsZ, config.CellSize);
                    int index = GridMath.CalculateHeightIndex(x, z, cellsX);

                    heights[index] = CalculateHeight(config, worldX, worldZ, offset);
                }
            }

            return heights;
        }

        private static float CalculateHeight(TerrainConfig config, float worldX, float worldZ, Vector2 offset)
        {
            float noise = SampleNoise(config, worldX, worldZ, offset);
            float redistributed = RedistributeNoise(noise, exponent: config.Redistribution);

            return ConvertNormalizedValueToHeight(redistributed, config.MinHeight, config.MaxHeight);
        }

        private static float SampleNoise(TerrainConfig config, float worldX, float worldZ, Vector2 offset)
        {
            float noiseX = ConvertWorldToNoiseCoordinate(worldX, config.NoiseScale, offset.x);
            float noiseY = ConvertWorldToNoiseCoordinate(worldZ, config.NoiseScale, offset.y);

            return FractalNoise.SampleFractalNoise(config, noiseX, noiseY);
        }

        private static float ConvertWorldToNoiseCoordinate(float world, float noiseScale, float offset) =>
            world / noiseScale + offset;

        private static float RedistributeNoise(float noise, float exponent) =>
            Mathf.Pow(noise, exponent);

        private static float ConvertNormalizedValueToHeight(float normalized, float minHeight, float maxHeight) =>
            Mathf.Lerp(minHeight, maxHeight, normalized);
    }
}
