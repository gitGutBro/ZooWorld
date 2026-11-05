using System;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.Spawning;
using ZooWorld.Infrastructure;
using ZooWorld.World;
using Object = UnityEngine.Object;

namespace ZooWorld.Tests
{
    public sealed class WeightedSpawnSelectorTests
    {
        private AnimalConfig _frog;
        private AnimalConfig _snake;
        private GameConfig _config;
        private IRandomService _random;

        [SetUp]
        public void SetUp()
        {
            _frog = ScriptableObject.CreateInstance<AnimalConfig>();
            _snake = ScriptableObject.CreateInstance<AnimalConfig>();
            _config = ScriptableObject.CreateInstance<GameConfig>();
            _random = Substitute.For<IRandomService>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_frog);
            Object.DestroyImmediate(_snake);
            Object.DestroyImmediate(_config);
        }

        [Test]
        public void Next_RollInsideEntryWeight_ReturnsThatEntry()
        {
            WeightedSpawnSelector selector = CreateSelector((_frog, 1f), (_snake, 3f));

            _random.Range(0f, 4f).Returns(0.5f);
            Assert.AreSame(_frog, selector.Next());

            _random.Range(0f, 4f).Returns(2.5f);
            Assert.AreSame(_snake, selector.Next());
        }

        [Test]
        public void Next_ZeroRollWithZeroWeightFirst_SkipsZeroWeightEntry()
        {
            WeightedSpawnSelector selector = CreateSelector((_frog, 0f), (_snake, 1f));
            _random.Range(Arg.Any<float>(), Arg.Any<float>()).Returns(0f);

            Assert.AreSame(_snake, selector.Next());
        }

        [Test]
        public void Constructor_NoPositiveWeight_Throws() =>
            Assert.Throws<InvalidOperationException>(() => CreateSelector((_frog, 0f)));

        [Test]
        public void Constructor_EntryWithoutAnimal_Throws() =>
            Assert.Throws<InvalidOperationException>(() => CreateSelector((_frog, 1f), (null, 1f)));

        private WeightedSpawnSelector CreateSelector(params (AnimalConfig Animal, float Weight)[] entries)
        {
            TestData.SetSpawnEntries(_config, entries);
            return new WeightedSpawnSelector(_config.Spawn, _random);
        }
    }
}
