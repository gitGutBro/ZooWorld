using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using R3;
using UnityEngine;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Bounce;

namespace ZooWorld.Tests
{
    public sealed class BounceServiceTests
    {
        private const float Tolerance = 0.001f;

        private BounceConfig _config;
        private BounceService _service;
        private List<BounceEvent> _bounces;

        [SetUp]
        public void SetUp()
        {
            _config = new BounceConfig();
            _service = new BounceService(_config);
            _bounces = new List<BounceEvent>();
            _service.Bounced.Subscribe(_bounces.Add);
        }

        [TearDown]
        public void TearDown() =>
            _service.Dispose();

        [Test]
        public void Bounce_StandingAnimals_PushesApartWithMinimalForce()
        {
            IAnimal left = CreateAnimal(Vector3.zero, Vector3.zero);
            IAnimal right = CreateAnimal(Vector3.right, Vector3.zero);

            _service.Bounce(left, right);

            Vector3 leftPush = ReceivedPush(left);
            Vector3 rightPush = ReceivedPush(right);
            Assert.Less(leftPush.x, 0f);
            Assert.Greater(rightPush.x, 0f);
            Assert.AreEqual(_config.MinSpeedChange, Flatten(rightPush).magnitude, Tolerance);
        }

        [Test]
        public void Bounce_FasterApproach_PushesHarder()
        {
            IAnimal slowLeft = CreateAnimal(Vector3.zero, Vector3.right);
            IAnimal slowRight = CreateAnimal(Vector3.right, Vector3.zero);
            IAnimal fastLeft = CreateAnimal(Vector3.zero, Vector3.right * 4f);
            IAnimal fastRight = CreateAnimal(Vector3.right, Vector3.zero);

            _service.Bounce(slowLeft, slowRight);
            _service.Bounce(fastLeft, fastRight);

            Assert.Greater(ReceivedPush(fastRight).magnitude, ReceivedPush(slowRight).magnitude);
            Assert.Greater(_bounces[1].Strength, _bounces[0].Strength);
        }

        [Test]
        public void Bounce_VeryFastApproach_ClampsToMaxForce()
        {
            IAnimal left = CreateAnimal(Vector3.zero, Vector3.right * 100f);
            IAnimal right = CreateAnimal(Vector3.right, Vector3.zero);

            _service.Bounce(left, right);

            Assert.AreEqual(_config.MaxSpeedChange, Flatten(ReceivedPush(right)).magnitude, Tolerance);
            Assert.AreEqual(1f, _bounces[0].Strength, Tolerance);
        }

        [Test]
        public void Bounce_PublishesEventBetweenAnimals()
        {
            IAnimal left = CreateAnimal(Vector3.zero, Vector3.zero);
            IAnimal right = CreateAnimal(Vector3.right * 2f, Vector3.zero);

            _service.Bounce(left, right);

            Assert.AreEqual(1, _bounces.Count);
            Assert.AreEqual(Vector3.right, _bounces[0].Point);
        }

        [Test]
        public void Bounce_AnimalAlreadyKnockedBack_Ignored()
        {
            IAnimal left = CreateAnimal(Vector3.zero, Vector3.zero);
            IAnimal right = CreateAnimal(Vector3.right, Vector3.zero);
            right.IsKnockedBack.Returns(true);

            _service.Bounce(left, right);

            left.DidNotReceive().Push(Arg.Any<Vector3>(), Arg.Any<float>());
            right.DidNotReceive().Push(Arg.Any<Vector3>(), Arg.Any<float>());
            Assert.IsEmpty(_bounces);
        }

        private static IAnimal CreateAnimal(Vector3 position, Vector3 velocity)
        {
            IAnimal animal = Substitute.For<IAnimal>();
            animal.Position.Returns(position);
            animal.Velocity.Returns(velocity);
            return animal;
        }

        private static Vector3 ReceivedPush(IAnimal animal)
        {
            ICall push = animal.ReceivedCalls().Single(call => call.GetMethodInfo().Name == nameof(IAnimal.Push));
            return (Vector3)push.GetArguments()[0];
        }

        private static Vector3 Flatten(Vector3 vector) =>
            new(vector.x, 0f, vector.z);
    }
}
