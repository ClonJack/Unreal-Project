using Common.Constants;
using SaveData;
using Services.Assets;
using Services.Configs;
using Services.Factories;
using Services.Input;
using Services.Save;
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
            => builder.Register<IInputService, StandaloneInputService>(Lifetime.Singleton);

        private static void RegisterOther(IContainerBuilder builder)
        {
            builder.Register<ConfigProvider>(Lifetime.Singleton).As<IConfigLoader, IConfigAccess>();
            builder.Register<IAssetProvider, ResourcesAssetProvider>(Lifetime.Singleton);
        }
    }
}