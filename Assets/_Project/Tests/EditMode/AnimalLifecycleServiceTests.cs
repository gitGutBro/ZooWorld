using NSubstitute;
using NUnit.Framework;
using R3;
using UnityEngine;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.Lifecycle;

namespace ZooWorld.Tests
{
    public sealed class AnimalLifecycleServiceTests
    {
        private IMutableAnimalRegistry _registry;
        private IAnimalPool _pool;
        private AnimalLifecycleService _lifecycle;
        private Animal _animal;
        private int _deathsCount;

        [SetUp]
        public void SetUp()
        {
            _registry = Substitute.For<IMutableAnimalRegistry>();
            _pool = Substitute.For<IAnimalPool>();
            _lifecycle = new AnimalLifecycleService(_registry, _pool);
            _lifecycle.Died.Subscribe(_ => _deathsCount++);
            _deathsCount = 0;
            _animal = TestData.CreateLiveAnimal();
        }

        [TearDown]
        public void TearDown() =>
            Object.DestroyImmediate(_animal.gameObject);

        [Test]
        public void Kill_TrackedAnimal_RemovesReleasesAndPublishesOnce()
        {
            _lifecycle.Track(_animal);

            _animal.Kill();
            _animal.Kill();

            _registry.Received(1).Remove(_animal);
            _pool.Received(1).Release(_animal);
            Assert.AreEqual(1, _deathsCount);

            _lifecycle.Dispose();
        }

        [Test]
        public void Kill_AfterDispose_IgnoresAnimal()
        {
            _lifecycle.Track(_animal);
            _lifecycle.Dispose();

            _animal.Kill();

            _registry.DidNotReceive().Remove(Arg.Any<Animal>());
            _pool.DidNotReceive().Release(Arg.Any<Animal>());
        }
    }
}
