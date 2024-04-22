using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using TriInspector;
using UnityEngine;
using UnrealTeam.SB.Common.Extensions;
using UnrealTeam.SB.Common.Utils;
using VContainer;

namespace UnrealTeam.SB.Common.Ecs.Binders
{
    public class EcsEntityProvider : MonoBehaviour
    {
        [SerializeReference, OnValueChanged(nameof(OnBindersChanged))]
        private List<EcsBinderBase> _componentsBinders = new();
        
        [field: ShowInInspector, ReadOnly] 
        public int Entity { get; private set; } = -1;

        [SerializeField] private bool _autoBuild = true;

        private const string _refBinderFieldName = "_component";
        
        private EcsWorld _ecsWorld;


        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;

            if (_autoBuild)
            {
                CreateEntity();
                BuildComponents();
            }
        }

        public void CreateEntity() 
            => Entity = _ecsWorld.NewEntity();

        public void BuildComponents()
        {
            foreach (var componentProvider in _componentsBinders)
                componentProvider.Init(Entity, _ecsWorld);
        }

        private void OnValidate()
        {
            SortBinders();
            InitRefsBinders();
        }

        private void OnBindersChanged()
        {
            SortBinders();
            InitRefsBinders();
        }

        private void InitRefsBinders()
        {
            foreach (var componentBinder in _componentsBinders)
            {
                if (componentBinder == null)
                    continue;

                var providerType = componentBinder.GetType();
                var parentType = providerType.BaseType;

                if (!IsRefBinderEmptyField(parentType, componentBinder))
                    continue;

                var genericArgType = parentType!.GetGenericArguments().Single();
                var component = GetComponentInChildren(genericArgType);
                if (component == null)
                    continue;

                ReflectionUtils.SetFieldValue(componentBinder, _refBinderFieldName, component, parentType);
                EditorUtils.SetDirtyIfNot(this);
            }
        }

        private void SortBinders()
        {
            var tagBinders = new List<EcsBinderBase>();
            var dataBinders = new List<EcsBinderBase>();
            var refBinders = new List<EcsBinderBase>();
            var otherBinders = new List<EcsBinderBase>();

            foreach (var componentBinder in _componentsBinders)
            {
                if (componentBinder == null)
                {
                    otherBinders.Add(null);
                    continue;
                }

                var providerType = componentBinder.GetType();
                var parentType = providerType.BaseType;
                var parentGenericType = parentType!.IsGenericType 
                    ? parentType.GetGenericTypeDefinition() 
                    : null;

                if (parentGenericType == typeof(EcsComponentRefBinder<>))
                {
                    refBinders.Add(componentBinder);
                    continue;
                }

                if (parentGenericType == typeof(EcsComponentBinder<>))
                {
                    var componentType = parentType.GetGenericArguments().Single();
                    
                    if (componentType.Name.Contains("Tag"))
                        tagBinders.Add(componentBinder);
                    else if (componentType.Name.Contains("Data"))
                        dataBinders.Add(componentBinder);
                    
                    continue;
                }
                
                otherBinders.Add(componentBinder);
            }

            var newBinders = new List<EcsBinderBase>();
            newBinders.AddRange(tagBinders);
            newBinders.AddRange(dataBinders);
            newBinders.AddRange(refBinders);
            newBinders.AddRange(otherBinders);

            if (_componentsBinders.Same(newBinders))
                return;

            _componentsBinders = newBinders;
            EditorUtils.SetDirtyIfNot(this);
        }

        private static bool IsRefBinderEmptyField(Type parentType, EcsBinderBase componentBinder)
            => parentType.IsGenericType &&
               parentType.GetGenericTypeDefinition() == typeof(EcsComponentRefBinder<>) &&
               ReflectionUtils.GetFieldValue(componentBinder, _refBinderFieldName, parentType) == null;
    }
}