using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Duels;
using ZooWorld.Animals.FoodChain.Roles;
using ZooWorld.Animals.FoodChain.Statistics;
using ZooWorld.World;

namespace ZooWorld.Tests
{
    public sealed class MostMealsDuelResolverTests
    {
        private const float FirstWins = 0.1f;
        private const float SecondWins = 0.9f;

        private FoodChainRole _predator;
        private IMealLedger _ledger;
        private IRandomService _random;
        private MostMealsDuelResolver _resolver;
        private IAnimal _first;
        private IAnimal _second;

        [SetUp]
        public void SetUp()
        {
            _predator = TestData.CreateRole("Predator");
            _ledger = Substitute.For<IMealLedger>();
            _random = Substitute.For<IRandomService>();
            _resolver = new MostMealsDuelResolver(_ledger, new RandomDuelResolver(_random));
            _first = TestData.CreateAnimal(_predator);
            _second = TestData.CreateAnimal(_predator);
        }

        [TearDown]
        public void TearDown() =>
            Object.DestroyImmediate(_predator);

        [Test]
        public void ResolveWinner_FirstAteMore_FirstWins()
        {
            SetMeals(3, 1);
            _random.Value.Returns(SecondWins);

            Assert.AreSame(_first, _resolver.ResolveWinner(_first, _second));
        }

        [Test]
        public void ResolveWinner_SecondAteMore_SecondWins()
        {
            SetMeals(0, 2);
            _random.Value.Returns(FirstWins);

            Assert.AreSame(_second, _resolver.ResolveWinner(_first, _second));
        }

        [Test]
        public void ResolveWinner_EqualMeals_UsesTieBreaker()
        {
            SetMeals(2, 2);

            _random.Value.Returns(FirstWins);
            Assert.AreSame(_first, _resolver.ResolveWinner(_first, _second));

            _random.Value.Returns(SecondWins);
            Assert.AreSame(_second, _resolver.ResolveWinner(_first, _second));
        }

        private void SetMeals(int first, int second)
        {
            _ledger.GetMeals(_first).Returns(first);
            _ledger.GetMeals(_second).Returns(second);
        }
    }
}
