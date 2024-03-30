using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnrealTeam.SB.Common.Utils;
using VContainer;

namespace UnrealTeam.SB.Common.Ecs.Providers
{
    public class EcsEntityProvider : MonoBehaviour
    {
        [SerializeReference, OnValueChanged(nameof(OnProvidersListChanged))] private List<EcsComponentProviderBase> _componentsProviders = new();
        
        private EcsWorld _ecsWorld;
        private int _entity;

        
        [Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
            InitEntity();
        }

        private void InitEntity()
        {            
            _entity = _ecsWorld.NewEntity();
            foreach (EcsComponentProviderBase componentProvider in _componentsProviders) 
                componentProvider.AddComponent(_entity, _ecsWorld);
        }

        private void OnValidate()
        {
            TryInitComponentsRefs();
        }

        private void OnProvidersListChanged()
        {
            TryInitComponentsRefs();
        }

        private void TryInitComponentsRefs()
        {
#if UNITY_EDITOR
            foreach (EcsComponentProviderBase componentProvider in _componentsProviders)
            {
                if (componentProvider == null)
                    continue;
                
                Type providerType = componentProvider.GetType();
                Type parentType = providerType.BaseType;
                
                if (parentType!.GetGenericTypeDefinition() == typeof(EcsComponentRefProvider<>))
                {
                    if (ReflectionUtils.GetFieldValue(componentProvider, "_component", parentType) == null)
                    {
                        Component component = GetComponent(parentType.GetGenericArguments().Single());
                        ReflectionUtils.SetFieldValue(componentProvider, "_component", component, parentType);
                    }
                }

                if (ReflectionUtils.GetFieldValue(componentProvider, "_component", parentType) != null) 
                    EditorUtility.SetDirty(this);
            }
#endif
        }
    }
}