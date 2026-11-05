using System.Collections.Generic;
using R3;
using ZooWorld.Animals.FoodChain.Roles;

namespace ZooWorld.Statistics
{
    public interface IDeathStatistics
    {
        IReadOnlyList<FoodChainRole> Roles { get; }

        ReadOnlyReactiveProperty<int> GetDeaths(FoodChainRole role);
    }
}
