namespace ZooWorld.World
{
    public sealed class UnityRandomService : IRandomService
    {
        public float Value => UnityEngine.Random.value;

        public float Range(float minInclusive, float maxInclusive) =>
            UnityEngine.Random.Range(minInclusive, maxInclusive);
    }
}
