using ZooWorld.Animals.Movement.Configuration;

namespace ZooWorld.Animals.Direction
{
    public interface IDirectionProviderFactory
    {
        IDirectionProvider Create(MovementConfig config);
    }
}
