using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnrealTeam.SB.Common.Utils;
using VContainer;

namespace UnrealTeam.SB.Common.Ecs.Binders
{
    public class EcsEntityProvider : MonoBehaviour
    {
        [SerializeReference, OnValueChanged(nameof(OnBindersChanged))]
        private List<EcsBinderBase> _componentsBinders = new();

#if UNITY_EDITOR
        private const string _cmpRefBinderFieldName = "_component";
#endif

        private EcsWorld _ecsWorld;
        private int _entity = -1;

        public int Entity => _entity;


        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;

            BuildEntity();
        }

        private void BuildEntity()
        {
            _entity = _ecsWorld.NewEntity();

            foreach (var componentProvider in _componentsBinders)
                componentProvider.Init(_entity, _ecsWorld);
        }

        private void OnValidate()
        {
            TryInitComponentsRef();
        }

        private void OnBindersChanged()
        {
            TryInitComponentsRef();
        }

        private void TryInitComponentsRef()
        {
#if UNITY_EDITOR
            foreach (var componentBinder in _componentsBinders)
            {
                if (componentBinder == null)
                    continue;

                var providerType = componentBinder.GetType();
                var parentType = providerType.BaseType;

                if (IsComponentRefBinderEmptyField(parentType, componentBinder))
                {
                    var genericArgType = parentType!.GetGenericArguments().Single();
                    var component = GetComponentInChildren(genericArgType);
                    if (component == null)
                        continue;

                    ReflectionUtils.SetFieldValue(componentBinder, _cmpRefBinderFieldName, component, parentType);
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }

        private static bool IsComponentRefBinderEmptyField(Type parentType, EcsBinderBase componentBinder)
            => parentType.IsGenericType &&
               parentType.GetGenericTypeDefinition() == typeof(EcsComponentRefBinder<>) &&
               ReflectionUtils.GetFieldValue(componentBinder, _cmpRefBinderFieldName, parentType) == null;
    }
}