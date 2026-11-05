using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Movement.Strategies;

namespace ZooWorld.Animals.Movement.Builders
{
    public sealed class DashMovementBuilder : MovementStrategyBuilder<DashMovementConfig>
    {
        protected override IMovementStrategy Build(DashMovementConfig config, MovementContext context) =>
            new DashMovementStrategy(config, context);
    }
}
