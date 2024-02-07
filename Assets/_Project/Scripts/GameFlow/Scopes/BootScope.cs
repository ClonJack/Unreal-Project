using Cysharp.Threading.Tasks;
using Services.Other;
using UI.Loading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameFlow.Scopes
{
    public class BootScope : LifetimeScope
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private CurtainRefs _curtainRefs;


        protected override void Configure(IContainerBuilder builder) 
            => RegisterEntryPoint(builder);

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.Register<BootEntryPoint>(Lifetime.Singleton);
            builder.RegisterBuildCallback(r =>
            {
                BindObjectsToProvider(r);
                ExecuteEntryPoint(r);
            });
        }

        private void BindObjectsToProvider(IObjectResolver resolver)
        {
            var objectsProvider = resolver.Resolve<ObjectsProvider>();
            objectsProvider.UiCamera = _uiCamera;
            objectsProvider.CurtainRefs = _curtainRefs;
        }

        private void ExecuteEntryPoint(IObjectResolver resolver) 
            => resolver.Resolve<BootEntryPoint>().ExecuteAsync().Forget();
    }
}