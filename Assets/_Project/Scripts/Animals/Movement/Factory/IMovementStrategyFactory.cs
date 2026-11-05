using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Movement.Strategies;

namespace ZooWorld.Animals.Movement.Factory
{
    public interface IMovementStrategyFactory
    {
        IMovementStrategy Create(MovementConfig config, MovementContext context);
    }
}
