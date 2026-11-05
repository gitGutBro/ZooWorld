using System;
using UnityEngine;

namespace ZooWorld.Animals.FoodChain.Roles
{
    [CreateAssetMenu(fileName = "FoodChainRole", menuName = "ZooWorld/Food Chain Role")]
    public sealed class FoodChainRole : ScriptableObject
    {
        [SerializeField] private string _displayName;
        [SerializeField] private FoodChainRole[] _diet = Array.Empty<FoodChainRole>();

        public string DisplayName => _displayName;

        public bool CanEat(FoodChainRole other) =>
            other && Array.IndexOf(_diet, other) >= 0;
    }
}
