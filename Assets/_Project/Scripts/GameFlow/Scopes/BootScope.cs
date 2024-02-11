using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB. GameFlow.Scopes
{
    public class BootScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEntryPoint(builder);
        }

        private static void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.Register<BootEntryPoint>(Lifetime.Singleton);
            builder.RegisterBuildCallback(r => r.Resolve<BootEntryPoint>().Execute());
        }
    }
}