using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Movement.Strategies;

namespace ZooWorld.Animals.Movement.Builders
{
    public sealed class LinearMovementBuilder : MovementStrategyBuilder<LinearMovementConfig>
    {
        protected override IMovementStrategy Build(LinearMovementConfig config, MovementContext context) =>
            new LinearMovementStrategy(config, context);
    }
}
