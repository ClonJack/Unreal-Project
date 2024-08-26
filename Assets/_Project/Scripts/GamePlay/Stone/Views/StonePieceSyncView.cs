using Fusion;
using UnrealTeam.SB.GamePlay.Common.Views;

namespace UnrealTeam.SB.GamePlay.Durability.Views
{
    public class StonePieceSyncView : SyncNetworkBehaviour
    {
        [Networked] public int Mass { get; private set; }
        [Networked] public bool IsInitialized { get; private set; }

        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void InitializePieceData(int mass)
        {
            if (IsInitialized)
                return;

            IsInitialized = true;
            Mass = mass;
        }
    }
}