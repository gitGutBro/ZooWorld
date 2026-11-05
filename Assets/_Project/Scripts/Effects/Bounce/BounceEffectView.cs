using R3;
using UnityEngine;

namespace ZooWorld.Effects.Bounce
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class BounceEffectView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private Vector2 _scaleRange = new(1f, 1.6f);
        [SerializeField] private Vector2 _speedMultiplierRange = new(0.8f, 1.4f);
        [SerializeField] private Vector2Int _burstCountRange = new(16, 36);

        private readonly Subject<BounceEffectView> _finished = new();

        private ParticleSystem.MinMaxCurve _baseStartSpeed;

        public Observable<BounceEffectView> Finished => _finished;

        private void Awake() =>
            _baseStartSpeed = _particles.main.startSpeed;

        private void OnDestroy() =>
            _finished.Dispose();

        private void OnParticleSystemStopped() =>
            _finished.OnNext(this);

        private void OnValidate()
        {
            if (_particles)
                return;

            _particles = GetComponent<ParticleSystem>();
        }

        public void Play(Vector3 position, float strength)
        {
            transform.SetPositionAndRotation(position, Quaternion.identity);
            transform.localScale = Vector3.one * Mathf.Lerp(_scaleRange.x, _scaleRange.y, strength);

            ApplyStrength(strength);
            _particles.Play(withChildren: true);
        }

        private void ApplyStrength(float strength)
        {
            float speedMultiplier = Mathf.Lerp(_speedMultiplierRange.x, _speedMultiplierRange.y, strength);

            ParticleSystem.MainModule main = _particles.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(
                _baseStartSpeed.constantMin * speedMultiplier,
                _baseStartSpeed.constantMax * speedMultiplier);

            int burstCount = Mathf.RoundToInt(Mathf.Lerp(_burstCountRange.x, _burstCountRange.y, strength));
            _particles.emission.SetBurst(0, new ParticleSystem.Burst(0f, burstCount));
        }
    }
}
