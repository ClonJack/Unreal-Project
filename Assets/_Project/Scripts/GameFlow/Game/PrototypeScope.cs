using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnrealTeam.SB.GamePlay.CharacterController.Systems;
using UnrealTeam.SB.GamePlay.Interaction.Systems;
using UnrealTeam.SB.GamePlay.Mining.Systems;
using UnrealTeam.SB.Services.Factories;
using UnrealTeam.SB.Services.Network;
using UnrealTeam.SB.Services.Other;
using UnrealTeam.SB.UI.Refs;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow.Game
{
    public class PrototypeScope : LifetimeScope
    {
        [SerializeField] private NetworkStateMachine _networkStateMachine;
        [SerializeField] private HudCanvasRefs _hudRefs;
        [SerializeField] private AssemblyDefinitionAsset[] _ecsSystemsAssemblies = {};
        
        
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
            => builder.Register<EcsLoop>(Lifetime.Scoped);

        private void RegisterEcsWorld(IContainerBuilder builder) 
            => builder.RegisterInstance(new EcsWorld());

        private void RegisterEcsSystems(IContainerBuilder builder)
        {
            foreach (var assemblyDefinition in _ecsSystemsAssemblies)
            {
                var ecsSystemsTypes = TypeCache.GetTypesDerivedFrom<IEcsSystem>(assemblyDefinition.name);
                foreach (var ecsSystemType in ecsSystemsTypes) 
                    builder.Register(ecsSystemType, Lifetime.Scoped);
            }
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