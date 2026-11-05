using UnityEngine;

namespace ZooWorld.Animals.Core
{
    public readonly struct AnimalCollision
    {
        public AnimalCollision(Animal self, Rigidbody other)
        {
            Self = self;
            Other = other;
        }

        public Animal Self { get; }
        public Rigidbody Other { get; }
    }
}
