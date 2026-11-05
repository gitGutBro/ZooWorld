using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZooWorld.World.Obstacles.Configuration;
using ZooWorld.World.Obstacles.Runtime;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.World.Obstacles.Generation
{
    public static class ObstacleLayoutGenerator
    {
        private const int AttemptsPerObstacle = 120;
        private const float FullTurnDegrees = 360f;

        public static IReadOnlyList<ObstaclePlacement> Generate(
            ObstacleFieldConfig config,
            Rect area,
            ITerrain terrain,
            IRandomService random)
        {
            List<ObstaclePlacement> placements = new();
            ObstacleEntry[] entries = config.Entries.Where(entry => entry.Weight > 0f && entry.Prefab).ToArray();

            if (entries.Length == 0)
                return placements;

            int targetCount = Mathf.RoundToInt(random.Range(config.MinCount, config.MaxCount));
            int attempts = targetCount * AttemptsPerObstacle;

            for (int i = 0; i < attempts && placements.Count < targetCount; i++)
            {
                ObstaclePlacement candidate = CreateCandidate(config, entries, area, random);

                if (Fits(candidate, config, area, terrain, placements))
                    placements.Add(candidate);
            }

            return placements;
        }

        private static ObstaclePlacement CreateCandidate(
            ObstacleFieldConfig config,
            IReadOnlyList<ObstacleEntry> entries,
            Rect area,
            IRandomService random)
        {
            Obstacle prefab = ChoosePrefab(entries, random);
            Vector2 position = new(random.Range(area.xMin, area.xMax), random.Range(area.yMin, area.yMax));
            float rotation = random.Range(0f, FullTurnDegrees);
            float scale = random.Range(config.ScaleRange.x, config.ScaleRange.y);

            return new ObstaclePlacement(prefab, position, rotation, scale);
        }

        private static Obstacle ChoosePrefab(IReadOnlyList<ObstacleEntry> entries, IRandomService random)
        {
            float roll = random.Range(0f, entries.Sum(entry => entry.Weight));

            foreach (ObstacleEntry entry in entries)
            {
                roll -= entry.Weight;

                if (roll <= 0f)
                    return entry.Prefab;
            }

            return entries[^1].Prefab;
        }

        private static bool Fits(ObstaclePlacement candidate, ObstacleFieldConfig config, Rect area,
            ITerrain terrain, IReadOnlyList<ObstaclePlacement> placed) =>
            IsInsideArea(candidate, config, area)
            && placed.All(other => IsSpacedFrom(candidate, other, config))
            && IsGroundEvenEnough(candidate, terrain);

        private static bool IsGroundEvenEnough(ObstaclePlacement candidate, ITerrain terrain) =>
            ObstacleFootprint.CalculateGroundDrop(terrain, candidate) <= candidate.Prefab.MaxGroundDrop;

        private static bool IsInsideArea(ObstaclePlacement candidate, ObstacleFieldConfig config, Rect area)
        {
            float margin = candidate.Radius + config.EdgePadding;
            Vector2 position = candidate.Position;

            return position.x - margin >= area.xMin && position.x + margin <= area.xMax
                && position.y - margin >= area.yMin && position.y + margin <= area.yMax;
        }

        private static bool IsSpacedFrom(ObstaclePlacement candidate, ObstaclePlacement other, ObstacleFieldConfig config) =>
            Vector2.Distance(candidate.Position, other.Position) >= candidate.Radius + other.Radius + config.Spacing;
    }
}
