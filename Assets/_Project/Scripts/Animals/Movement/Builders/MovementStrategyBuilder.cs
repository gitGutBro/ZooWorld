using System;
using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Movement.Strategies;

namespace ZooWorld.Animals.Movement.Builders
{
    public abstract class MovementStrategyBuilder<TConfig> : IMovementStrategyBuilder where TConfig : MovementConfig
    {
        public Type ConfigType => typeof(TConfig);

        public IMovementStrategy Build(MovementConfig config, MovementContext context) =>
            Build((TConfig)config, context);

        protected abstract IMovementStrategy Build(TConfig config, MovementContext context);
    }
}
