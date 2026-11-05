namespace ZooWorld.Animals.Timing
{
    public sealed class CountdownTimer
    {
        private float _remainingSeconds;

        public bool IsFinished => _remainingSeconds <= 0f;

        public void Tick(float deltaTime)
        {
            if (IsFinished)
                return;

            _remainingSeconds -= deltaTime;

            if (_remainingSeconds < 0f)
                _remainingSeconds = 0f;
        }

        public void Restart(float durationSeconds) =>
            _remainingSeconds = durationSeconds;
    }
}
