using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZooWorld.World.Obstacles.Runtime;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.World.Obstacles.Generation
{
    public static class ObstacleFootprint
    {
        private const float Diagonal = 0.7071f;

        private static readonly Vector2[] Directions =
        {
            Vector2.right, Vector2.left, Vector2.up, Vector2.down,
            new(Diagonal, Diagonal), new(Diagonal, -Diagonal), new(-Diagonal, Diagonal), new(-Diagonal, -Diagonal),
        };

        private static readonly float[] RadiusShares = { 0.5f, 1f };

        public static float CalculateAnchorHeight(ITerrain terrain, ObstaclePlacement placement)
        {
            IEnumerable<float> heights = SampleGround(terrain, placement);

            return placement.Prefab.GroundAnchor == ObstacleGroundAnchor.Highest
                ? heights.Max()
                : heights.Min();
        }

        public static float CalculateGroundDrop(ITerrain terrain, ObstaclePlacement placement)
        {
            float[] heights = SampleGround(terrain, placement).ToArray();
            return heights.Max() - heights.Min();
        }

        private static IEnumerable<float> SampleGround(ITerrain terrain, ObstaclePlacement placement)
        {
            yield return HeightAt(terrain, placement.Position);

            foreach (float share in RadiusShares)
                foreach (Vector2 direction in Directions)
                    yield return HeightAt(terrain, placement.Position + direction * (placement.Radius * share));
        }

        private static float HeightAt(ITerrain terrain, Vector2 point) =>
            terrain.HeightAt(new Vector3(point.x, 0f, point.y));
    }
}
