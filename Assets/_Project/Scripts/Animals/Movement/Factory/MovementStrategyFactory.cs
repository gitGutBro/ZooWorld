using System;
using System.Collections.Generic;
using ZooWorld.Animals.Movement.Builders;
using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Movement.Strategies;

namespace ZooWorld.Animals.Movement.Factory
{
    public sealed class MovementStrategyFactory : IMovementStrategyFactory
    {
        private readonly Dictionary<Type, IMovementStrategyBuilder> _builders = new();

        public MovementStrategyFactory(IEnumerable<IMovementStrategyBuilder> builders)
        {
            foreach (IMovementStrategyBuilder builder in builders)
                _builders.Add(builder.ConfigType, builder);
        }

        public IMovementStrategy Create(MovementConfig config, MovementContext context)
        {
            if (!config)
                throw new ArgumentNullException(nameof(config));

            if (!_builders.TryGetValue(config.GetType(), out IMovementStrategyBuilder builder))
                throw new InvalidOperationException(
                    $"No {nameof(IMovementStrategyBuilder)} is registered for {config.GetType().Name}.");

            return builder.Build(config, context);
        }
    }
}
