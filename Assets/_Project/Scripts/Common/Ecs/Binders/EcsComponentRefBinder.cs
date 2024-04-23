using UnityEngine;

namespace UnrealTeam.SB.Common.Ecs.Binders
{
    public abstract class EcsComponentRefBinder<TComponent> : EcsComponentBinder<ComponentRef<TComponent>>
        where TComponent : Component
    {
        [SerializeField] private TComponent _component;

        protected sealed override void InitData(ref ComponentRef<TComponent> componentRef)
        {
            componentRef.Component = _component;
            InitView(_component);
        }

        protected virtual void InitView(TComponent component) {}
    }
}