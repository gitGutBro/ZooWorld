using UnityEngine;
using ZooWorld.Animals.FoodChain.Roles;
using ZooWorld.Animals.Movement.Configuration;

namespace ZooWorld.Animals.Core
{
    [CreateAssetMenu(fileName = "Animal", menuName = "ZooWorld/Animal")]
    public sealed class AnimalConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Animal _prefab;
        [SerializeField] private FoodChainRole _role;
        [SerializeField] private MovementConfig _movement;

        public string Id => _id;
        public Animal Prefab => _prefab;
        public FoodChainRole Role => _role;
        public MovementConfig Movement => _movement;
    }
}
