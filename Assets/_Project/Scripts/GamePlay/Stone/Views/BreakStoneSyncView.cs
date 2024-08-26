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
        [Header("References")]
        [SerializeField] private DurabilitySyncView _durabilitySyncView;
        [SerializeField] private Destructible _destructible;
        [SerializeField] private GameObject[] _destroyedVariants = {};
        
        [Header("Minerals")]
        [Tooltip("In kilograms"), Min(0)]
        [SerializeField] private int _minMass;
        
        [Tooltip("In kilograms"), Min(0)]
        [SerializeField] private int _maxMass;
        
        [Tooltip("Percentage of minerals from the total mass"), Range(0, 100)]
        [SerializeField] private float _mineralsMinContent;
        
        [Tooltip("Percentage of minerals from the total mass"), Range(0, 100)]
        [SerializeField] private float _mineralsMaxContent;
        
        [SerializeField] private StoneMineralData[] _mineralsData;

        
        [field: HideInInspector]
        [Networked] public bool IsInitialized { get; private set; }

        [field: HideInInspector]
        [Networked] public int Mass { get; private set; }

        [field: HideInInspector]
        [Networked] public float MineralsContent { get; private set; }

        [field: HideInInspector] [Capacity(20)] 
        [Networked] public NetworkArray<float> MineralsWeights { get; } = default;
        
        
        private readonly Dictionary<int, List<GameObject>> _destroyedVariantsMap = new();
        

        public override void Spawned()
        {
            base.Spawned();
            _durabilitySyncView.ZeroReached += BreakStoneRpc;
            InitDestroyedObjectsMap();
            InitializeStoneData();
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void BreakStoneRpc()
        {
            _destructible.Destroy();
            if (HasStateAuthority)
                ReplaceWithDestroyedObject();
        }

        private void InitializeStoneData()
        {
            if (!HasStateAuthority || IsInitialized)
                return;

            Mass = RandomUtils.Generate(_minMass, _maxMass);
            MineralsContent = RandomUtils.Generate(_mineralsMinContent, _mineralsMaxContent);
            InitMineralWeights();
            IsInitialized = true;
        }

        private void ReplaceWithDestroyedObject()
        {
            var (piecesCount, destroyedVariants) = RandomUtils.PickOne(_destroyedVariantsMap);
            var destroyedPrefab = RandomUtils.PickOne(destroyedVariants);
            var destroyedRoot = Runner.Spawn(destroyedPrefab, transform.position, Quaternion.identity);
            InjectByNetworkIdRpc(destroyedRoot.Id);
            
            var accumulatedMass = 0;
            var massEqualShare = Mass / piecesCount;
            
            for (var i = 0; i < piecesCount - 1; i++)
            {
                var pieceMass = RandomUtils.Generate(massEqualShare / 2, massEqualShare);
                accumulatedMass += pieceMass;
                destroyedRoot.transform.GetChild(i).GetComponent<StonePieceSyncView>();
            }
        }

        private void InitMineralWeights()
        {
            for (var i = 0; i < _mineralsData.Length; i++)
            {
                var mineralWeights = _mineralsData[i];
                MineralsWeights.Set(i, RandomUtils.Generate(mineralWeights.MinWeights, mineralWeights.MaxWeights));
            }
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