using NUnit.Framework;
using UnityEngine;
using ZooWorld.Animals.FoodChain.Roles;

namespace ZooWorld.Tests
{
    public sealed class FoodChainRoleTests
    {
        private FoodChainRole _prey;
        private FoodChainRole _predator;

        [SetUp]
        public void SetUp()
        {
            _prey = TestData.CreateRole("Prey");
            _predator = TestData.CreateRole("Predator");
            TestData.SetDiet(_predator, _prey, _predator);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_prey);
            Object.DestroyImmediate(_predator);
        }

        [Test]
        public void CanEat_RoleInDiet_ReturnsTrue()
        {
            Assert.IsTrue(_predator.CanEat(_prey));
            Assert.IsTrue(_predator.CanEat(_predator));
        }

        [Test]
        public void CanEat_RoleNotInDiet_ReturnsFalse()
        {
            Assert.IsFalse(_prey.CanEat(_predator));
            Assert.IsFalse(_prey.CanEat(_prey));
        }

        [Test]
        public void CanEat_Null_ReturnsFalse() =>
            Assert.IsFalse(_predator.CanEat(null));
    }
}
