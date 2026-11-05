using System;
using R3;
using UnityEngine;
using ZooWorld.Animals.Core;

namespace ZooWorld.Animals.FoodChain.Bounce
{
    public sealed class BounceService : IBounceService, IDisposable
    {
        private const float MinSeparationSqr = 0.0001f;
        private const float ContactBlend = 0.5f;

        private readonly BounceConfig _config;
        private readonly Subject<BounceEvent> _bounced = new();

        public BounceService(BounceConfig config) =>
            _config = config;

        public Observable<BounceEvent> Bounced => _bounced;

        public void Bounce(IAnimal first, IAnimal second)
        {
            if (first.IsKnockedBack || second.IsKnockedBack)
                return;

            Vector3 toSecond = CalculateDirection(first, second);
            float speedChange = CalculateSpeedChange(first, second, toSecond);
            Vector3 lift = Vector3.up * (speedChange * _config.Lift);

            Vector3 velocityChange = toSecond * speedChange + lift;
            first.Push(-velocityChange, _config.RecoverySeconds);
            second.Push(velocityChange, _config.RecoverySeconds);

            Vector3 point = Vector3.Lerp(first.Position, second.Position, ContactBlend);
            _bounced.OnNext(new BounceEvent(first, second, point, CalculateStrength(speedChange)));
        }

        public void Dispose() =>
            _bounced.Dispose();

        private static Vector3 CalculateDirection(IAnimal first, IAnimal second)
        {
            Vector3 toSecond = Flatten(second.Position - first.Position);
            return toSecond.sqrMagnitude > MinSeparationSqr ? toSecond.normalized : Vector3.forward;
        }

        private float CalculateSpeedChange(IAnimal first, IAnimal second, Vector3 toSecond)
        {
            float closingSpeed = Mathf.Max(0f, Vector3.Dot(Flatten(first.Velocity - second.Velocity), toSecond));
            float speedChange = _config.MinSpeedChange + closingSpeed * _config.SpeedChangePerClosingSpeed;
            return Mathf.Min(speedChange, _config.MaxSpeedChange);
        }

        private float CalculateStrength(float speedChange) =>
            Mathf.InverseLerp(_config.MinSpeedChange, _config.MaxSpeedChange, speedChange);

        private static Vector3 Flatten(Vector3 vector) =>
            new(vector.x, 0f, vector.z);
    }
}
