namespace ZooWorld.World
{
    public interface IRandomService
    {
        float Value { get; }

        float Range(float minInclusive, float maxInclusive);
    }
}
