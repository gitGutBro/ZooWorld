using System.Collections.Generic;
using UnityEngine;
using ZooWorld.World.Terrain.Configuration;

namespace ZooWorld.World.Terrain.MeshGeneration
{
    public static class TerrainPalette
    {
        private const float SteepBlendDegrees = 8f;

        public static Color Evaluate(TerrainConfig config, float normalizedHeight, float slopeDegrees, float jitter)
        {
            Color color = EvaluateStops(config.ColorStops, normalizedHeight);

            float steepness = Mathf.InverseLerp(config.SteepSlopeStart, config.SteepSlopeStart + SteepBlendDegrees, slopeDegrees);
            color = Color.Lerp(color, config.SteepColor, Mathf.SmoothStep(0f, 1f, steepness));

            float brightness = 1f + jitter * config.ColorJitter;
            return new Color(color.r * brightness, color.g * brightness, color.b * brightness, 1f);
        }

        private static Color EvaluateStops(IReadOnlyList<TerrainColorStop> stops, float position)
        {
            if (stops.Count == 0)
                return Color.white;

            if (position <= stops[0].Position)
                return stops[0].Color;

            for (int i = 1; i < stops.Count; i++)
                if (position <= stops[i].Position)
                    return Blend(stops[i - 1], stops[i], position);

            return stops[^1].Color;
        }

        private static Color Blend(TerrainColorStop from, TerrainColorStop to, float position)
        {
            float span = to.Position - from.Position;
            float blend = span > 0f ? (position - from.Position) / span : 1f;
            return Color.Lerp(from.Color, to.Color, blend);
        }
    }
}
