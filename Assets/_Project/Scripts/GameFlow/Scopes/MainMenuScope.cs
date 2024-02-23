using Cysharp.Threading.Tasks;
using Services.Other;
using VContainer;
using VContainer.Unity;

namespace GameFlow.Scopes
{
    public class MainMenuScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder) 
            => RegisterEntryPoint(builder);

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.Register<MainMenuEntryPoint>(Lifetime.Singleton);
            builder.RegisterBuildCallback(r =>
            {
                BindObjectsToProvider(r);
                ExecuteEntryPoint(r);
            });
        }

        private void BindObjectsToProvider(IObjectResolver resolver)
        {
            var objectsProvider = resolver.Resolve<ObjectsProvider>();
        }

        private static void ExecuteEntryPoint(IObjectResolver resolver) 
            => resolver.Resolve<MainMenuEntryPoint>().ExecuteAsync().Forget();
    }
}