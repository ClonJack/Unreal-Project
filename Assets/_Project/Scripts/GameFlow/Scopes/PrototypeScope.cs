using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using UnrealTeam.SB.GamePlay.CharacterController.Systems;
using UnrealTeam.SB.GamePlay.Interaction.Systems;
using UnrealTeam.SB.GamePlay.Network;
using UnrealTeam.SB.GamePlay.Mining.Systems;
using UnrealTeam.SB.Services.Factories;
using UnrealTeam.SB.Services.Other;
using UnrealTeam.SB.UI.Refs;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow.Scopes
{
    public class PrototypeScope : LifetimeScope
    {
        [SerializeField] private NetworkStateMachine _networkStateMachine;
        [SerializeField] private HudCanvasRefs _hudRefs;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEntryPoint(builder);
            RegisterEcsLoop(builder);
            RegisterEcsWorld(builder);
            RegisterEcsSystems(builder);
            RegisterFactories(builder);
            RegisterNetwork(builder);
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PrototypeEntryPoint>().AsSelf();
            builder.RegisterBuildCallback(resolver =>
            {
                BindObjectsToProvider(resolver);
                ExecuteEntryPoint(resolver);
            });
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
            builder.Register<CharacterInputSystem>(Lifetime.Singleton);
            builder.Register<CharacterMoveSystem>(Lifetime.Singleton);
            builder.Register<CharacterRotateSystem>(Lifetime.Singleton);
            
            builder.Register<InteractionSystem>(Lifetime.Singleton);
            builder.Register<UseInteractedSystem>(Lifetime.Singleton);
            builder.Register<OutlineInteractedSystem>(Lifetime.Singleton);
            builder.Register<DrawInteractionUiSystem>(Lifetime.Singleton);

            builder.Register<MiningStationInputSystem>(Lifetime.Singleton);
            builder.Register<MiningStationEnterSystem>(Lifetime.Singleton);
            builder.Register<MiningStationLeaveSystem>(Lifetime.Singleton);
            builder.Register<MiningLaserRotationSystem>(Lifetime.Singleton);
            builder.Register<MiningPlatformRotationSystem>(Lifetime.Singleton);
        }

        private void RegisterNetwork(IContainerBuilder builder)
        {
            builder.RegisterComponent(_networkStateMachine);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<PlayerFactory>(Lifetime.Singleton);
        }

        private void BindObjectsToProvider(IObjectResolver resolver)
        {
            var objectsProvider = resolver.Resolve<ObjectsProvider>();
            objectsProvider.HudRefs = _hudRefs;
        }

        private static void ExecuteEntryPoint(IObjectResolver resolver) 
            => resolver.Resolve<PrototypeEntryPoint>().Execute().Forget();
    }
}