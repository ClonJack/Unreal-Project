using SaveData;
using Services.Loading;
using Services.Other;
using UnrealTeam.SB.Assets;
using UnrealTeam.SB.Configs;
using UnrealTeam.SB.Constants;
using UnrealTeam.SB.Factories;
using UnrealTeam.SB.Input;
using UnrealTeam.SB.Save;
using VContainer;
using VContainer.Unity;

namespace GameFlow.Scopes
{
    public class ProjectScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterFactories(builder);
            RegisterSaveLoad(builder);
            RegisterInput(builder);
            RegisterLoading(builder);
            RegisterOther(builder);
        }

        private static void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<ObjectInjector>(Lifetime.Singleton);
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