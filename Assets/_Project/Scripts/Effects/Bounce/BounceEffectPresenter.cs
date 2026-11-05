using System;
using R3;
using UnityEngine;
using UnityEngine.Pool;
using VContainer.Unity;
using ZooWorld.Animals.FoodChain.Bounce;
using Object = UnityEngine.Object;

namespace ZooWorld.Effects.Bounce
{
    public sealed class BounceEffectPresenter : IStartable, IDisposable
    {
        private const string RootName = "BounceEffects";

        private readonly IBounceService _bounce;
        private readonly BounceEffectView _prefab;
        private readonly ObjectPool<BounceEffectView> _pool;
        private readonly CompositeDisposable _disposables = new();

        private Transform _root;

        public BounceEffectPresenter(IBounceService bounce, BounceEffectView prefab)
        {
            _bounce = bounce;
            _prefab = prefab;
            _pool = new ObjectPool<BounceEffectView>(
                Create,
                actionOnGet:     view => view.gameObject.SetActive(true),
                actionOnRelease: view => view.gameObject.SetActive(false));
        }

        public void Start() =>
            _bounce.Bounced.Subscribe(OnBounced).AddTo(_disposables);

        public void Dispose() =>
            _disposables.Dispose();

        private void OnBounced(BounceEvent bounce) =>
            _pool.Get().Play(bounce.Point, bounce.Strength);

        private BounceEffectView Create()
        {
            BounceEffectView view = Object.Instantiate(_prefab, GetRoot());
            view.Finished.Subscribe(_pool.Release).AddTo(_disposables);
            return view;
        }

        private Transform GetRoot()
        {
            if (!_root)
                _root = new GameObject(RootName).transform;

            return _root;
        }
    }
}
