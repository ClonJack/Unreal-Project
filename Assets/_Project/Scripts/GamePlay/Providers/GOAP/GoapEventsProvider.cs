using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using Leopotam.EcsLite;
using UnityEngine;
using UnrealTeam.SB.Common.Ecs.Providers;

namespace UnrealTeam.SB.GamePlay.AI.Events
{
    /*[Serializable]
    public class GoapEventsProvider : EcsProvider
    {
        [SerializeField] private AgentBehaviour _agent;
        
        private int _entity;
        private EcsWorld _ecsWorld;
        
        
        public override void Init(int entity, EcsWorld ecsWorld)
        {
            _ecsWorld = ecsWorld;
            _entity = entity;
            _agent.Events.OnActionStart += OnAction;
        }

        private void OnAction<TComponent>(IActionBase action)
            where TComponent : struct
        {
            _ecsWorld.GetPool<TComponent>().Add(_entity).
        }
    }*/
}