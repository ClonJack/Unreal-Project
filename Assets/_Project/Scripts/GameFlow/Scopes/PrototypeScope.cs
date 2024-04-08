using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using UnrealTeam.SB.Configs.Spawn;
using UnrealTeam.SB.GamePlay.AI.Common;
using UnrealTeam.SB.GamePlay.CharacterController.Systems;
using UnrealTeam.SB.GamePlay.Interaction.Systems;
using UnrealTeam.SB.Services.Factories;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow.Scopes
{
    public class PrototypeScope : LifetimeScope
    {
        [SerializeField] private GoapConfigInjector _goapConfigs;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEntryPoint(builder);
            RegisterEcsLoop(builder);
            RegisterEcsWorld(builder);
            RegisterEcsSystems(builder);
            RegisterGoap(builder);
            RegisterSpawnPoints(builder);
            RegisterFactories(builder);
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PrototypeEntryPoint>().AsSelf();
            builder.RegisterBuildCallback(r => r.Resolve<PrototypeEntryPoint>().Execute().Forget());
        }

        private void RegisterEcsLoop(IContainerBuilder builder)
        {
            builder.Register<EcsService>(Lifetime.Singleton);
        }

        private void RegisterEcsWorld(IContainerBuilder builder)
        {
            builder.RegisterInstance(new EcsWorld());
        }

        private void RegisterEcsSystems(IContainerBuilder builder)
        {
            builder.Register<PlayerInputSystem>(Lifetime.Singleton);
            builder.Register<CharacterMoveSystem>(Lifetime.Singleton);
            builder.Register<CharacterRotateSystem>(Lifetime.Singleton);
            builder.Register<CleanupInteractSystem>(Lifetime.Singleton);
            builder.Register<InteractSystem>(Lifetime.Singleton);
            builder.Register<OutlineInteractedSystem>(Lifetime.Singleton);
        }

        private void RegisterSpawnPoints(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SpawnPoint>();
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<PlayerFactory>(Lifetime.Singleton);
        }

        private void RegisterGoap(IContainerBuilder builder)
        {
            builder.RegisterComponent(_goapConfigs).As<IGoapConfigAccess>();
        }
    }
}