using UnityEngine;

namespace UnrealTeam.SB.Common.Ecs.Providers
{
    public abstract class EcsComponentRefProvider<TComponent> : EcsComponentProvider<ComponentRef<TComponent>> 
        where TComponent : Component
    {
        [SerializeField] private TComponent _component;


        protected sealed override void InitData(ref ComponentRef<TComponent> componentRef)
            => componentRef.Component = _component;
    }
}