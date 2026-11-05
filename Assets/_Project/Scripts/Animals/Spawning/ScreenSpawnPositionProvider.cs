using UnityEngine;
using ZooWorld.World;
using ZooWorld.World.Obstacles.Runtime;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.Animals.Spawning
{
    public sealed class ScreenSpawnPositionProvider : ISpawnPositionProvider
    {
        private const float MaxPaddingShare = 0.5f;
        private const float ObstacleClearance = 0.8f;
        private const int MaxAttempts = 12;

        private readonly IScreenBounds _bounds;
        private readonly IObstacleMap _obstacles;
        private readonly IRandomService _random;
        private readonly ITerrain _terrain;
        private readonly SpawnConfig _config;

        public ScreenSpawnPositionProvider(
            IScreenBounds bounds,
            IObstacleMap obstacles,
            IRandomService random,
            ITerrain terrain,
            SpawnConfig config)
        {
            _bounds = bounds;
            _obstacles = obstacles;
            _random = random;
            _terrain = terrain;
            _config = config;
        }

        public Vector3 Next()
        {
            Vector3 candidate = PickCandidate();

            for (int attempt = 1; attempt < MaxAttempts && IsBlocked(candidate); attempt++)
                candidate = PickCandidate();

            return candidate;
        }

        private bool IsBlocked(Vector3 position) =>
            _obstacles.TryFindObstacle(position, ObstacleClearance, out _);

        private Vector3 PickCandidate()
        {
            Rect area = _bounds.Area;
            float padding = Mathf.Min(_config.EdgePadding, area.width * MaxPaddingShare, area.height * MaxPaddingShare);

            float x = _random.Range(area.xMin + padding, area.xMax - padding);
            float z = _random.Range(area.yMin + padding, area.yMax - padding);
            float ground = _terrain.HeightAt(new Vector3(x, 0f, z));

            return new Vector3(x, ground + _config.HeightOffset, z);
        }
    }
}
