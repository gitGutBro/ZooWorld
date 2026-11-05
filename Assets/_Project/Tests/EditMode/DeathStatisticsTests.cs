using NSubstitute;
using NUnit.Framework;
using R3;
using UnityEngine;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Roles;
using ZooWorld.Animals.Lifecycle;
using ZooWorld.Statistics;

namespace ZooWorld.Tests
{
    public sealed class DeathStatisticsTests
    {
        private FoodChainRole _prey;
        private FoodChainRole _predator;
        private FoodChainRole _uncounted;
        private Subject<IAnimal> _died;
        private DeathStatistics _statistics;

        [SetUp]
        public void SetUp()
        {
            _prey = TestData.CreateRole("Prey");
            _predator = TestData.CreateRole("Predator");
            _uncounted = TestData.CreateRole("Uncounted");
            _died = new Subject<IAnimal>();

            var deathEvents = Substitute.For<IAnimalDeathEvents>();
            deathEvents.Died.Returns(_died);
            _statistics = new DeathStatistics(new[] { _prey, _predator }, deathEvents);
        }

        [TearDown]
        public void TearDown()
        {
            _statistics.Dispose();
            _died.Dispose();
            Object.DestroyImmediate(_prey);
            Object.DestroyImmediate(_predator);
            Object.DestroyImmediate(_uncounted);
        }

        [Test]
        public void Died_IncrementsCounterOfMatchingRoleOnly()
        {
            _died.OnNext(TestData.CreateAnimal(_prey));
            _died.OnNext(TestData.CreateAnimal(_prey));
            _died.OnNext(TestData.CreateAnimal(_predator));

            Assert.AreEqual(2, _statistics.GetDeaths(_prey).CurrentValue);
            Assert.AreEqual(1, _statistics.GetDeaths(_predator).CurrentValue);
        }

        [Test]
        public void Died_UncountedRole_IsIgnored()
        {
            _died.OnNext(TestData.CreateAnimal(_uncounted));

            Assert.AreEqual(0, _statistics.GetDeaths(_prey).CurrentValue);
            Assert.AreEqual(0, _statistics.GetDeaths(_predator).CurrentValue);
        }
    }
}
