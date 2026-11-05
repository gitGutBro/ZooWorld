using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Movement.Strategies;

namespace ZooWorld.Animals.Movement.Builders
{
    public sealed class JumpMovementBuilder : MovementStrategyBuilder<JumpMovementConfig>
    {
        protected override IMovementStrategy Build(JumpMovementConfig config, MovementContext context) =>
            new JumpMovementStrategy(config, context);
    }
}
