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

        private const string _refBinderFieldName = "_component";
        private EcsWorld _ecsWorld;
        private int _entity = -1;

        public int Entity => _entity;


        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;

            //  BuildEntity();
        }

        public void BuildEntity()
        {
            _entity = _ecsWorld.NewEntity();

            foreach (var componentProvider in _componentsBinders)
                componentProvider.Init(_entity, _ecsWorld);
        }

        public void BuildEntity(EcsWorld world)
        {
            var entity = world.NewEntity();

            foreach (var componentProvider in _componentsBinders)
                componentProvider.Init(entity, world);
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

            foreach (EcsBinderBase componentBinder in _componentsBinders)
            {
                if (componentBinder == null)
                {
                    otherBinders.Add(null);
                    continue;
                }

                var providerType = componentBinder.GetType();
                var parentType = providerType.BaseType;

                if (parentType!.IsGenericType &&
                    parentType.GetGenericTypeDefinition() == typeof(EcsComponentRefBinder<>))
                    refBinders.Add(componentBinder);
                else if (providerType.Name.Contains("Tag"))
                    tagBinders.Add(componentBinder);
                else if (providerType.Name.Contains("Data"))
                    dataBinders.Add(componentBinder);
                else
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