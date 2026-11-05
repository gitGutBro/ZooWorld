using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.World;
using ZooWorld.World.Obstacles.Runtime;

namespace ZooWorld.Animals.Direction
{
    public sealed class WanderDirectionProviderFactory : IDirectionProviderFactory
    {
        private readonly IScreenBounds _bounds;
        private readonly IObstacleMap _obstacles;
        private readonly IRandomService _random;

        public WanderDirectionProviderFactory(IScreenBounds bounds, IObstacleMap obstacles, IRandomService random)
        {
            _bounds = bounds;
            _obstacles = obstacles;
            _random = random;
        }

        public IDirectionProvider Create(MovementConfig config) =>
            new WanderDirectionProvider(_bounds, _obstacles, _random, config.DirectionChangeSeconds);
    }
}
