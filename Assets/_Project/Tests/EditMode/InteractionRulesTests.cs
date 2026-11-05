using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using R3;
using UnityEngine;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Bounce;
using ZooWorld.Animals.FoodChain.Duels;
using ZooWorld.Animals.FoodChain.Interaction;
using ZooWorld.Animals.FoodChain.Predation;
using ZooWorld.Animals.FoodChain.Roles;

namespace ZooWorld.Tests
{
    public sealed class InteractionRulesTests
    {
        private FoodChainRole _prey;
        private FoodChainRole _predator;
        private PredationService _predation;
        private IDuelResolver _duel;
        private IBounceService _bounce;
        private IReadOnlyList<IInteractionRule> _rules;
        private List<PredationEvent> _meals;

        [SetUp]
        public void SetUp()
        {
            _prey = TestData.CreateRole("Prey");
            _predator = TestData.CreateRole("Predator");
            TestData.SetDiet(_predator, _prey, _predator);

            _predation = new PredationService();
            _duel = Substitute.For<IDuelResolver>();
            _duel.ResolveWinner(Arg.Any<IAnimal>(), Arg.Any<IAnimal>()).Returns(call => call.ArgAt<IAnimal>(0));
            _bounce = Substitute.For<IBounceService>();
            _rules = TestData.CreateRules(_duel, _predation, _bounce);

            _meals = new List<PredationEvent>();
            _predation.Ate.Subscribe(_meals.Add);
        }

        [TearDown]
        public void TearDown()
        {
            _predation.Dispose();
            Object.DestroyImmediate(_prey);
            Object.DestroyImmediate(_predator);
        }

        [Test]
        public void PreyAndPrey_BounceApartWithoutDeaths()
        {
            IAnimal first = TestData.CreateAnimal(_prey);
            IAnimal second = TestData.CreateAnimal(_prey);

            Apply(first, second);

            _bounce.Received(1).Bounce(first, second);
            first.DidNotReceive().Kill();
            second.DidNotReceive().Kill();
            Assert.IsEmpty(_meals);
        }

        [Test]
        public void PredatorAndPrey_NoBounce()
        {
            IAnimal frog = TestData.CreateAnimal(_prey);
            IAnimal snake = TestData.CreateAnimal(_predator);

            Apply(frog, snake);

            _bounce.DidNotReceive().Bounce(Arg.Any<IAnimal>(), Arg.Any<IAnimal>());
        }

        [Test]
        public void AnyPair_ExactlyOneRuleApplies()
        {
            IAnimal prey = TestData.CreateAnimal(_prey);
            IAnimal predator = TestData.CreateAnimal(_predator);

            Assert.AreEqual(1, _rules.Count(rule => rule.CanApply(prey, prey)));
            Assert.AreEqual(1, _rules.Count(rule => rule.CanApply(prey, predator)));
            Assert.AreEqual(1, _rules.Count(rule => rule.CanApply(predator, prey)));
            Assert.AreEqual(1, _rules.Count(rule => rule.CanApply(predator, predator)));
        }

        [Test]
        public void PredatorAndPrey_PreyDiesAndMealIsPublished()
        {
            IAnimal frog = TestData.CreateAnimal(_prey);
            IAnimal snake = TestData.CreateAnimal(_predator);

            Apply(frog, snake);

            frog.Received(1).Kill();
            snake.DidNotReceive().Kill();
            Assert.AreEqual(1, _meals.Count);
            Assert.AreSame(snake, _meals[0].Eater);
            Assert.AreSame(frog, _meals[0].Victim);
        }

        [Test]
        public void PredatorAndPredator_ExactlyOneSurvives()
        {
            IAnimal first = TestData.CreateAnimal(_predator);
            IAnimal second = TestData.CreateAnimal(_predator);

            Apply(first, second);

            Assert.AreNotEqual(first.IsAlive, second.IsAlive);
            Assert.AreEqual(1, _meals.Count);
        }

        private void Apply(IAnimal first, IAnimal second)
        {
            IInteractionRule rule = _rules.First(candidate => candidate.CanApply(first, second));
            rule.Apply(first, second);
        }
    }
}
