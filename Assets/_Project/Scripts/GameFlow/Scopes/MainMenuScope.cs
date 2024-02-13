using Cysharp.Threading.Tasks;
using Services.Other;
using UI;
using UI.Refs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameFlow.Scopes
{
    public class MainMenuScope : LifetimeScope
    {
        [SerializeField] private MainMenuCanvasRefs _menuCanvasRefs;


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
            objectsProvider.MenuCanvasRefs = _menuCanvasRefs;
        }

        private static void ExecuteEntryPoint(IObjectResolver resolver) 
            => resolver.Resolve<MainMenuEntryPoint>().ExecuteAsync().Forget();
    }
}