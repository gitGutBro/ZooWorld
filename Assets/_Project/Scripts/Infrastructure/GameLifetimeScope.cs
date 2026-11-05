using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.Direction;
using ZooWorld.Animals.FoodChain.Bounce;
using ZooWorld.Animals.FoodChain.Duels;
using ZooWorld.Animals.FoodChain.Interaction;
using ZooWorld.Animals.FoodChain.Predation;
using ZooWorld.Animals.FoodChain.Statistics;
using ZooWorld.Animals.Lifecycle;
using ZooWorld.Animals.Movement.Builders;
using ZooWorld.Animals.Movement.Factory;
using ZooWorld.Animals.Spawning;
using ZooWorld.Effects.Bounce;
using ZooWorld.Statistics;
using ZooWorld.UI.DeathCounter;
using ZooWorld.UI.TastyLabel;
using ZooWorld.World;
using ZooWorld.World.Obstacles.Runtime;
using ZooWorld.World.Terrain.Generation;
using ZooWorld.World.Terrain.Runtime;

namespace ZooWorld.Infrastructure
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameConfig _config;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterWorld(builder);
            RegisterTerrain(builder);
            RegisterObstacles(builder);
            RegisterMovement(builder);
            RegisterFoodChain(builder);
            RegisterBounce(builder);
            RegisterLifecycle(builder);
            RegisterSpawning(builder);
            RegisterStatistics(builder);
            RegisterUI(builder);
            RegisterEffects(builder);
        }

        private static void RegisterWorld(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<Camera>();
            builder.Register<ScreenBounds>(Lifetime.Singleton).As<IScreenBounds>();
            builder.Register<UnityRandomService>(Lifetime.Singleton).As<IRandomService>();
            builder.RegisterEntryPoint<ArenaWalls>();
        }

        private void RegisterTerrain(IContainerBuilder builder)
        {
            builder.RegisterInstance(_config.Terrain);
            builder.Register(resolver => TerrainGenerator.GenerateHeightField(
                _config.Terrain, random: resolver.Resolve<IRandomService>()), Lifetime.Singleton);
            builder.Register<HeightFieldTerrain>(Lifetime.Singleton).As<ITerrain>();
            builder.RegisterEntryPoint<TerrainBuilder>();
        }

        private void RegisterObstacles(IContainerBuilder builder)
        {
            builder.RegisterInstance(_config.Obstacles);
            builder.Register<ObstacleField>(Lifetime.Singleton).As<IObstacleMap, IObstacleRegistry>();
            builder.RegisterEntryPoint<ObstacleBuilder>();
        }

        private static void RegisterMovement(IContainerBuilder builder)
        {
            builder.Register<JumpMovementBuilder>(Lifetime.Singleton).As<IMovementStrategyBuilder>();
            builder.Register<LinearMovementBuilder>(Lifetime.Singleton).As<IMovementStrategyBuilder>();
            builder.Register<DashMovementBuilder>(Lifetime.Singleton).As<IMovementStrategyBuilder>();
            builder.Register<MovementStrategyFactory>(Lifetime.Singleton).As<IMovementStrategyFactory>();
            builder.Register<WanderDirectionProviderFactory>(Lifetime.Singleton).As<IDirectionProviderFactory>();
        }

        private static void RegisterFoodChain(IContainerBuilder builder)
        {
            builder.Register<MutualPredationRule>(Lifetime.Singleton).As<IInteractionRule>();
            builder.Register<PredationRule>(Lifetime.Singleton).As<IInteractionRule>();
            builder.Register<BounceRule>(Lifetime.Singleton).As<IInteractionRule>();
            builder.Register<RandomDuelResolver>(Lifetime.Singleton);
            builder.Register<IDuelResolver>(resolver => new MostMealsDuelResolver(
                resolver.Resolve<IMealLedger>(), tieBreaker: resolver.Resolve<RandomDuelResolver>()), Lifetime.Singleton);
            builder.RegisterEntryPoint<MealLedger>();
            builder.Register<PredationService>(Lifetime.Singleton).As<IPredationService>();
            builder.RegisterEntryPoint<AnimalInteractionService>();
        }

        private void RegisterBounce(IContainerBuilder builder)
        {
            builder.RegisterInstance(_config.Bounce);
            builder.Register<BounceService>(Lifetime.Singleton).As<IBounceService>();
        }

        private static void RegisterLifecycle(IContainerBuilder builder)
        {
            builder.Register<AnimalRegistry>(Lifetime.Singleton).As<IAnimalRegistry, IMutableAnimalRegistry>();
            builder.Register<AnimalPool>(Lifetime.Singleton).As<IAnimalPool>();
            builder.Register<AnimalLifecycleService>(Lifetime.Singleton).As<IAnimalLifecycleService, IAnimalDeathEvents>();
            builder.Register<AnimalFactory>(Lifetime.Singleton).As<IAnimalFactory>();
            builder.RegisterEntryPoint<AnimalSimulation>();
        }

        private void RegisterSpawning(IContainerBuilder builder)
        {
            builder.RegisterInstance(_config.Spawn);
            builder.Register<WeightedSpawnSelector>(Lifetime.Singleton).As<ISpawnSelector>();
            builder.Register<ScreenSpawnPositionProvider>(Lifetime.Singleton).As<ISpawnPositionProvider>();
            builder.RegisterEntryPoint<AnimalSpawner>();
        }

        private void RegisterStatistics(IContainerBuilder builder)
        {
            builder.Register<DeathStatistics>(Lifetime.Singleton)
                .As<IDeathStatistics>()
                .WithParameter(_config.CountedRoles);
        }

        private void RegisterUI(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<DeathCounterView>();
            builder.RegisterComponentInHierarchy<TastyLabelLayer>();
            builder.RegisterEntryPoint<DeathCounterPresenter>();
            builder.RegisterEntryPoint<TastyLabelPresenter>()
                .WithParameter(_config.TastyLabelDuration);
        }

        private void RegisterEffects(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BounceEffectPresenter>()
                .WithParameter(_config.BounceEffectPrefab);
        }
    }
}
