using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using ZooWorld.World.Obstacles.Configuration;
using ZooWorld.World.Obstacles.Generation;
using ZooWorld.World.Obstacles.Runtime;
using ZooWorld.World.Terrain.Configuration;
using ZooWorld.World.Terrain.Generation;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.Tests
{
    public sealed class ObstacleGroundingTests
    {
        private const string TerrainConfigPath = "Assets/_Project/Configs/TerrainConfig.asset";
        private const string FieldConfigPath = "Assets/_Project/Configs/ObstacleFieldConfig.asset";
        private const int Layouts = 60;
        private const int MinFencesPlaced = 20;
        private const int SamplesPerEdge = 12;
        private const float MinVisibleHeight = 0.5f;
        private const float Tolerance = 0.01f;

        private static readonly Rect Area = new(-13.33f, -7.5f, 26.66f, 15f);

        [Test]
        public void Fences_OnRealTerrain_StayVisibleAndReachGround()
        {
            TerrainConfig terrainConfig = AssetDatabase.LoadAssetAtPath<TerrainConfig>(TerrainConfigPath);
            ObstacleFieldConfig fieldConfig = AssetDatabase.LoadAssetAtPath<ObstacleFieldConfig>(FieldConfigPath);
            int fencesPlaced = 0;

            for (int seed = 0; seed < Layouts; seed++)
            {
                SeededRandomService random = new SeededRandomService(seed);
                ITerrain terrain = new HeightFieldTerrain(terrainConfig, TerrainGenerator.GenerateHeightField(terrainConfig, random));

                foreach (ObstaclePlacement placement in ObstacleLayoutGenerator.Generate(fieldConfig, Area, terrain, random))
                {
                    if (placement.Prefab.GroundAnchor != ObstacleGroundAnchor.Highest)
                        continue;

                    fencesPlaced++;
                    AssertGrounded(placement, terrain, fieldConfig);
                }
            }

            Assert.GreaterOrEqual(fencesPlaced, MinFencesPlaced, "Fences are almost never placed.");
        }

        private static void AssertGrounded(ObstaclePlacement placement, ITerrain terrain, ObstacleFieldConfig fieldConfig)
        {
            float height = ObstacleFootprint.CalculateAnchorHeight(terrain, placement) - fieldConfig.SinkDepth;
            GameObject instance = Object.Instantiate(placement.Prefab.gameObject,
                new Vector3(placement.Position.x, height, placement.Position.y),
                Quaternion.Euler(0f, placement.RotationDegrees, 0f));
            instance.transform.localScale = Vector3.one * placement.Scale;

            try
            {
                foreach (BoxCollider box in instance.GetComponentsInChildren<BoxCollider>())
                {
                    foreach ((Vector3 bottom, Vector3 top) in SampleEdges(box))
                    {
                        float ground = terrain.HeightAt(bottom);
                        Assert.GreaterOrEqual(top.y - ground, MinVisibleHeight - Tolerance, $"{placement.Prefab.name} is buried.");
                        Assert.LessOrEqual(bottom.y, ground + Tolerance, $"{placement.Prefab.name} floats above ground.");
                    }
                }
            }
            finally
            {
                Object.DestroyImmediate(instance);
            }
        }

        private static IEnumerable<(Vector3 Bottom, Vector3 Top)> SampleEdges(BoxCollider box)
        {
            Vector3 half = box.size * 0.5f;

            for (int i = 0; i <= SamplesPerEdge; i++)
            {
                float x = Mathf.Lerp(-half.x, half.x, i / (float)SamplesPerEdge);

                foreach (float z in new[] { -half.z, half.z })
                {
                    Vector3 bottom = box.transform.TransformPoint(box.center + new Vector3(x, -half.y, z));
                    Vector3 top = box.transform.TransformPoint(box.center + new Vector3(x, half.y, z));
                    yield return (bottom, top);
                }
            }
        }
    }
}
