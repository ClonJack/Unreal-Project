using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnrealTeam.SB.GamePlay.Systems;
using UnrealTeam.SB.Services.Factories;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow.Scopes
{
    public class LevelScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEntryPoint(builder);
            RegisterEcsLoop(builder);
            RegisterEcsWorld(builder);
            RegisterEcsSystems(builder);
            RegisterEntitiesFactories(builder);
        }

        private static void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LevelEntryPoint>().AsSelf();
            builder.RegisterBuildCallback(ExecuteEntryPoint);
        }

        private static void ExecuteEntryPoint(IObjectResolver r) 
            => r.Resolve<LevelEntryPoint>().ExecuteAsync().Forget();

        private static void RegisterEcsLoop(IContainerBuilder builder) 
            => builder.Register<EcsService>(Lifetime.Singleton);

        private static void RegisterEcsWorld(IContainerBuilder builder)
            => builder.RegisterInstance(new EcsWorld());

        private static void RegisterEcsSystems(IContainerBuilder builder)
        {
            builder.Register<CharacterMoveSystem>(Lifetime.Singleton);
            builder.Register<CharacterRotateSystem>(Lifetime.Singleton);
            builder.Register<PlayerInputSystem>(Lifetime.Singleton);
        }

        private static void RegisterEntitiesFactories(IContainerBuilder builder)
        {
            builder.Register<PlayerFactory>(Lifetime.Singleton);
        }
    }
}