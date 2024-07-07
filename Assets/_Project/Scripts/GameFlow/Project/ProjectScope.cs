using Fusion;
using UnrealTeam.SB.Additional.Constants;
using UnrealTeam.SB.SaveData;
using UnrealTeam.SB.Services.Assets;
using UnrealTeam.SB.Services.Configs;
using UnrealTeam.SB.Services.Factories;
using UnrealTeam.SB.Services.InputControl;
using UnrealTeam.SB.Services.InputControl.Interfaces;
using UnrealTeam.SB.Services.Loading;
using UnrealTeam.SB.Services.Other;
using UnrealTeam.SB.Services.Save;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow.Project
{
    public class ProjectScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterSaveLoad(builder);
            RegisterInput(builder);
            RegisterLoading(builder);
            RegisterOther(builder);
        }

        private static void RegisterSaveLoad(IContainerBuilder builder)
        {
            builder
                .RegisterInstance(new PlayerPrefsStorage<PlayerProgress>(GameConstants.PrefsProgressKey))
                .As<ISaveStorage<PlayerProgress>>();
            
            builder.Register<SaveService>(Lifetime.Singleton);
        }

        private static void RegisterInput(IContainerBuilder builder) 
            => builder.Register<IInputService, InputService>(Lifetime.Singleton);

        private static void RegisterLoading(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<LoadingCurtain>(Lifetime.Singleton);
        }

        private static void RegisterOther(IContainerBuilder builder)
        {
            builder.Register<IAssetProvider, ResourcesAssetProvider>(Lifetime.Singleton);
            builder.Register<ConfigProvider>(Lifetime.Singleton).As<IConfigLoader, IConfigAccess>();
            builder.Register<ObjectsProvider>(Lifetime.Singleton);
            builder.Register<ScreenNavService>(Lifetime.Singleton);
        }
    }
}