using System.Collections.Generic;
using DestroyIt;
using Fusion;
using UnityEngine;
using UnrealTeam.SB.Common.Extensions;
using UnrealTeam.SB.Common.Utils;
using UnrealTeam.SB.GamePlay.Common.Views;
using UnrealTeam.SB.GamePlay.Durability.Additional;

namespace UnrealTeam.SB.GamePlay.Durability.Views
{
    public class BreakStoneSyncView : SyncNetworkBehaviour
    {
        [SerializeField] private DurabilitySyncView _durabilitySyncView;
        [SerializeField] private Destructible _destructible;
        [SerializeField] private GameObject[] _destroyedVariants = {};
        [SerializeField] private int _minMass;
        [SerializeField] private int _maxMass;
        
        [Tooltip("In Percentage"), Range(0, 100)]
        [SerializeField] private float _mineralsContent;
        [SerializeField] private MineralWeightData _mineralsWeights;
        
        private readonly Dictionary<int, List<GameObject>> _destroyedVariantsMap = new();
        

        public override void Spawned()
        {
            base.Spawned();
            _durabilitySyncView.ZeroReached += BreakStoneRpc;
            InitDestroyedObjectsMap();
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void BreakStoneRpc()
        {
            _destructible.Destroy();
            if (HasStateAuthority) 
                ReplaceWithDestroyedObject();
        }

        private void ReplaceWithDestroyedObject()
        {
            var (piecesCount, destroyedVariants) = RandomUtils.PickOne(_destroyedVariantsMap);
            var targetVariant = RandomUtils.PickOne(destroyedVariants);
            var objectId = Runner.Spawn(targetVariant, transform.position, Quaternion.identity).Id;
            InjectByNetworkIdRpc(objectId);
        }

        private void InitDestroyedObjectsMap()
        {
            foreach (var destroyedVariant in _destroyedVariants)
            {
                var piecesCount = destroyedVariant.transform.childCount;
                var destroyedVariantsCollection = _destroyedVariantsMap.GetOrAdd(piecesCount, new List<GameObject>());
                destroyedVariantsCollection.Add(destroyedVariant);
            }
        }
    }
}