using Cysharp.Threading.Tasks;
using UnityEngine;
using UnrealTeam.SB.Services.Other;
using UnrealTeam.SB.UI.Refs;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.GameFlow.Boot
{
    public class BootScope : LifetimeScope
    {
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private LoadingCurtainCanvasRefs _curtainCanvasRefs;


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
            objectsProvider.CurtainRefs = _curtainCanvasRefs;
        }

        private void ExecuteEntryPoint(IObjectResolver resolver) 
            => resolver.Resolve<BootEntryPoint>().ExecuteAsync().Forget();
    }
}