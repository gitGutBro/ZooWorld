using NSubstitute;
using NUnit.Framework;
using R3;
using UnityEngine;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Predation;
using ZooWorld.Animals.FoodChain.Roles;
using ZooWorld.Animals.FoodChain.Statistics;
using ZooWorld.Animals.Lifecycle;

namespace ZooWorld.Tests
{
    public sealed class MealLedgerTests
    {
        private FoodChainRole _prey;
        private FoodChainRole _predator;
        private PredationService _predation;
        private Subject<IAnimal> _died;
        private MealLedger _ledger;

        [SetUp]
        public void SetUp()
        {
            _prey = TestData.CreateRole("Prey");
            _predator = TestData.CreateRole("Predator");
            _predation = new PredationService();
            _died = new Subject<IAnimal>();

            IAnimalDeathEvents deathEvents = Substitute.For<IAnimalDeathEvents>();
            deathEvents.Died.Returns(_died);

            _ledger = new MealLedger(_predation, deathEvents);
            _ledger.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _ledger.Dispose();
            _predation.Dispose();
            _died.Dispose();
            Object.DestroyImmediate(_prey);
            Object.DestroyImmediate(_predator);
        }

        [Test]
        public void GetMeals_NewAnimal_ReturnsZero() =>
            Assert.AreEqual(0, _ledger.GetMeals(TestData.CreateAnimal(_predator)));

        [Test]
        public void Eat_CountsMealsOfEaterOnly()
        {
            IAnimal snake = TestData.CreateAnimal(_predator);
            IAnimal fox = TestData.CreateAnimal(_predator);

            _predation.Eat(snake, TestData.CreateAnimal(_prey));
            _predation.Eat(snake, TestData.CreateAnimal(_prey));
            _predation.Eat(fox, TestData.CreateAnimal(_prey));

            Assert.AreEqual(2, _ledger.GetMeals(snake));
            Assert.AreEqual(1, _ledger.GetMeals(fox));
        }

        [Test]
        public void Eat_PredatorVictim_CountsAsMeal()
        {
            IAnimal winner = TestData.CreateAnimal(_predator);

            _predation.Eat(winner, TestData.CreateAnimal(_predator));

            Assert.AreEqual(1, _ledger.GetMeals(winner));
        }

        [Test]
        public void Died_ResetsMeals()
        {
            IAnimal snake = TestData.CreateAnimal(_predator);
            _predation.Eat(snake, TestData.CreateAnimal(_prey));

            _died.OnNext(snake);

            Assert.AreEqual(0, _ledger.GetMeals(snake));
        }
    }
}
