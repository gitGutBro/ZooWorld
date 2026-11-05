using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Pool;
using VContainer.Unity;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Predation;
using ZooWorld.Animals.Lifecycle;

namespace ZooWorld.UI.TastyLabel
{
    public sealed class TastyLabelPresenter : IStartable, ILateTickable, IDisposable
    {
        private readonly IPredationService _predation;
        private readonly IAnimalDeathEvents _deathEvents;
        private readonly Camera _camera;
        private readonly TimeSpan _duration;
        private readonly ObjectPool<TastyLabelView> _pool;
        private readonly Dictionary<IAnimal, ActiveLabel> _activeLabels = new();
        private readonly CompositeDisposable _disposables = new();

        public TastyLabelPresenter(
            IPredationService predation,
            IAnimalDeathEvents deathEvents,
            TastyLabelLayer layer,
            Camera camera,
            TimeSpan duration)
        {
            _predation = predation;
            _deathEvents = deathEvents;
            _camera = camera;
            _duration = duration;
            _pool = new ObjectPool<TastyLabelView>(
                layer.CreateLabel, actionOnGet: view => view.Show(), actionOnRelease: view => view.Hide());
        }

        public void Start()
        {
            _predation.Ate.Subscribe(OnAte).AddTo(_disposables);
            _deathEvents.Died.Subscribe(Hide).AddTo(_disposables);
        }

        public void LateTick()
        {
            foreach (KeyValuePair<IAnimal, ActiveLabel> pair in _activeLabels)
                Follow(pair.Value.View, pair.Key);
        }

        public void Dispose()
        {
            _disposables.Dispose();

            foreach (ActiveLabel label in _activeLabels.Values.ToList())
            {
                label.Cancellation.Cancel();
                label.Cancellation.Dispose();
            }

            _activeLabels.Clear();
        }

        private void OnAte(PredationEvent predation)
        {
            if (!predation.Eater.IsAlive)
                return;

            Hide(predation.Eater);
            Show(predation.Eater);
        }

        private void Show(IAnimal eater)
        {
            TastyLabelView view = _pool.Get();
            Follow(view, eater);

            CancellationTokenSource cancellation = new();
            _activeLabels.Add(eater, new ActiveLabel(view, cancellation));
            HideAfterDelayAsync(eater, cancellation.Token).Forget();
        }

        private async UniTaskVoid HideAfterDelayAsync(IAnimal eater, CancellationToken cancellation)
        {
            bool cancelled = await UniTask
                .Delay(_duration, cancellationToken: cancellation)
                .SuppressCancellationThrow();

            if (cancelled)
                return;

            Hide(eater);
        }

        private void Hide(IAnimal eater)
        {
            if (!_activeLabels.Remove(eater, out ActiveLabel label))
                return;

            label.Cancellation.Cancel();
            label.Cancellation.Dispose();
            _pool.Release(label.View);
        }

        private void Follow(TastyLabelView view, IAnimal eater) =>
            view.SetScreenPosition(_camera.WorldToScreenPoint(eater.Position));

        private readonly struct ActiveLabel
        {
            public ActiveLabel(TastyLabelView view, CancellationTokenSource cancellation)
            {
                View = view;
                Cancellation = cancellation;
            }

            public TastyLabelView View { get; }
            public CancellationTokenSource Cancellation { get; }
        }
    }
}
