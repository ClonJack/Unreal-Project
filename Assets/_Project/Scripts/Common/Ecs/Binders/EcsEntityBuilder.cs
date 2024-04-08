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
    public class EcsEntityBuilder : MonoBehaviour
    {
        [SerializeReference, OnValueChanged(nameof(OnProvidersListChanged))]
        private List<EcsBaseBinder> _bindersComponents = new();

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

            foreach (var componentProvider in _bindersComponents)
                componentProvider.Init(_entity, _ecsWorld);
        }

        private void OnValidate()
        {
            TryDrawComponents();
        }

        private void OnProvidersListChanged()
        {
            TryDrawComponents();
        }

        private void TryDrawComponents()
        {
#if UNITY_EDITOR
            foreach (var componentProvider in _bindersComponents)
            {
                if (componentProvider == null)
                    continue;

                var providerType = componentProvider.GetType();
                var parentType = providerType.BaseType;

                if (parentType != null && parentType.IsGenericType && 
                    parentType.GetGenericTypeDefinition() == typeof(EcsComponentBinderRef<>))
                {
                    var componentValue = ReflectionUtils.GetFieldValue(componentProvider, "_component", parentType);
                    if (componentValue == null)
                    {
                        var genericArgumentType = parentType.GetGenericArguments().FirstOrDefault(); 
                        if (genericArgumentType != null)
                        {
                            var component = GetComponentInChildren(genericArgumentType);
                            if (component != null)
                            {
                                ReflectionUtils.SetFieldValue(componentProvider, "_component", component, parentType);
                                EditorUtility.SetDirty(this);
                            }
                        }
                    }
                }
            }
#endif
        }
    }
}