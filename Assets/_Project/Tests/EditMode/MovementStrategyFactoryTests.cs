using System;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.Animals.Movement.Builders;
using ZooWorld.Animals.Movement.Configuration;
using ZooWorld.Animals.Movement.Factory;
using ZooWorld.Animals.Movement.Strategies;
using Object = UnityEngine.Object;

namespace ZooWorld.Tests
{
    public sealed class MovementStrategyFactoryTests
    {
        private MovementStrategyFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new MovementStrategyFactory(new IMovementStrategyBuilder[]
            {
                new JumpMovementBuilder(),
                new LinearMovementBuilder(),
                new DashMovementBuilder(),
            });
        }

        [Test]
        public void Create_DashConfig_ReturnsDashStrategy() =>
            AssertCreates<DashMovementConfig, DashMovementStrategy>();

        [Test]
        public void Create_JumpConfig_ReturnsJumpStrategy() =>
            AssertCreates<JumpMovementConfig, JumpMovementStrategy>();

        [Test]
        public void Create_LinearConfig_ReturnsLinearStrategy() =>
            AssertCreates<LinearMovementConfig, LinearMovementStrategy>();

        [Test]
        public void Create_UnknownConfig_ThrowsDescriptiveException()
        {
            UnknownMovementConfig config = ScriptableObject.CreateInstance<UnknownMovementConfig>();

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => _factory.Create(config, null));
            StringAssert.Contains(nameof(UnknownMovementConfig), exception.Message);

            Object.DestroyImmediate(config);
        }

        private void AssertCreates<TConfig, TStrategy>() where TConfig : MovementConfig
        {
            TConfig config = ScriptableObject.CreateInstance<TConfig>();

            IMovementStrategy strategy = _factory.Create(config, null);

            Assert.IsInstanceOf<TStrategy>(strategy);
            Object.DestroyImmediate(config);
        }

        private sealed class UnknownMovementConfig : MovementConfig
        {
        }
    }
}
