using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnrealTeam.SB.GamePlay.CharacterController.Components;
using UnrealTeam.SB.GamePlay.Interaction.Components;

namespace UnrealTeam.SB.GamePlay.Interaction.Systems
{
    public class UseInteractedSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CharacterUseAction>> _useFilter;
        private readonly EcsFilterInject<Inc<InteractAction, UsableObjectTag>> _usableFilter;
        private readonly EcsPoolInject<UsedObjectAction> _usedObjectPool;
        private readonly EcsPoolInject<InteractAction> _interactPool;
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (int playerEntity in _useFilter.Value)
            foreach (int usableEntity in _usableFilter.Value)
                UseObject(usableEntity, playerEntity);
        }

        private void UseObject(int usableEntity, int playerEntity)
        {
            int interactedBy = _interactPool.Value.Get(usableEntity).InteractedBy;
            if (interactedBy == playerEntity)
                _usedObjectPool.Value.Add(usableEntity).UsedBy = playerEntity;
        }
    }
}