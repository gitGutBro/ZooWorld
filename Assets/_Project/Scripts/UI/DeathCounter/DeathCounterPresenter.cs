using System;
using R3;
using VContainer.Unity;
using ZooWorld.Animals.FoodChain.Roles;
using ZooWorld.Statistics;

namespace ZooWorld.UI.DeathCounter
{
    public sealed class DeathCounterPresenter : IStartable, IDisposable
    {
        private readonly IDeathStatistics _statistics;
        private readonly DeathCounterView _view;
        private readonly CompositeDisposable _disposables = new();

        public DeathCounterPresenter(IDeathStatistics statistics, DeathCounterView view)
        {
            _statistics = statistics;
            _view = view;
        }

        public void Start()
        {
            foreach (FoodChainRole role in _statistics.Roles)
                Bind(role);
        }

        public void Dispose() =>
            _disposables.Dispose();

        private void Bind(FoodChainRole role)
        {
            CounterItemView item = _view.CreateItem();
            item.SetTitle(role.DisplayName);

            _statistics.GetDeaths(role)
                .Subscribe(item.SetValue)
                .AddTo(_disposables);
        }
    }
}
