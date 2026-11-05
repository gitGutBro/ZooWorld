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
    public sealed class AnimalInteractionServiceTests
    {
        private FoodChainRole _prey;
        private FoodChainRole _predator;
        private PredationService _predation;
        private AnimalInteractionService _service;
        private int _mealsCount;

        [SetUp]
        public void SetUp()
        {
            _prey = TestData.CreateRole("Prey");
            _predator = TestData.CreateRole("Predator");
            TestData.SetDiet(_predator, _prey, _predator);

            _predation = new PredationService();
            _predation.Ate.Subscribe(_ => _mealsCount++);
            _mealsCount = 0;

            var duel = Substitute.For<IDuelResolver>();
            duel.ResolveWinner(Arg.Any<IAnimal>(), Arg.Any<IAnimal>()).Returns(call => call.ArgAt<IAnimal>(0));

            _service = new AnimalInteractionService(Substitute.For<IAnimalRegistry>(), TestData.CreateRules(duel, _predation, Substitute.For<IBounceService>()));
        }

        [TearDown]
        public void TearDown()
        {
            _service.Dispose();
            _predation.Dispose();
            Object.DestroyImmediate(_prey);
            Object.DestroyImmediate(_predator);
        }

        [Test]
        public void Resolve_RepeatedFromSecondParticipant_ChangesNothing()
        {
            IAnimal snake = TestData.CreateAnimal(_predator);
            IAnimal frog = TestData.CreateAnimal(_prey);

            _service.Resolve(snake, frog);
            _service.Resolve(frog, snake);

            frog.Received(1).Kill();
            snake.DidNotReceive().Kill();
            Assert.AreEqual(1, _mealsCount);
        }

        [Test]
        public void Resolve_PredatorDuelRepeated_KillsOnlyOne()
        {
            IAnimal first = TestData.CreateAnimal(_predator);
            IAnimal second = TestData.CreateAnimal(_predator);

            _service.Resolve(first, second);
            _service.Resolve(second, first);

            Assert.IsTrue(first.IsAlive);
            Assert.IsFalse(second.IsAlive);
            Assert.AreEqual(1, _mealsCount);
        }
    }
}
