using System;
using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Movement.Strategies;

namespace ZooWorld.Animals.Movement.Builders
{
    public interface IMovementStrategyBuilder
    {
        Type ConfigType { get; }

        IMovementStrategy Build(MovementConfig config, MovementContext context);
    }
}
