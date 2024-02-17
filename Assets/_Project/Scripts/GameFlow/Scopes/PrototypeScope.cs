using Leopotam.EcsLite;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using UnrealTeam.SB.Factories;
using UnrealTeam.SB.Spawn;
using UnrealTeam.SB.Systems;

namespace UnrealTeam.SB.GameFlow.Scopes
{
    public class PrototypeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEntryPoint(builder);
            RegisterEcsLoop(builder);
            RegisterEcsWorld(builder);
            RegisterEcsSystems(builder);
            RegisterSpawnPoints(builder);
            RegisterFactories(builder);
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PrototypeEntryPoint>().AsSelf();
        //    builder.RegisterBuildCallback(r => r.Resolve<PrototypeEntryPoint>().Execute());
        }

        private void RegisterEcsLoop(IContainerBuilder builder)
        {
            Debug.Log("RegisterEcsLoop");
            
            builder.Register<EcsService>(Lifetime.Singleton);
        }

        private void RegisterEcsWorld(IContainerBuilder builder)
        {
            Debug.Log("RegisterEcsWorld");
            
            builder.RegisterInstance(new EcsWorld());
        }

        private void RegisterEcsSystems(IContainerBuilder builder)
        {
            Debug.Log("RegisterEcsSystems");
            
            builder.Register<PlayerInputSystem>(Lifetime.Singleton);
            builder.Register<CharacterMoveSystem>(Lifetime.Singleton);
            builder.Register<CharacterRotateSystem>(Lifetime.Singleton);
        }

        private void RegisterSpawnPoints(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SpawnPoint>();
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<PlayerFactory>(Lifetime.Singleton);
        }
    }
}