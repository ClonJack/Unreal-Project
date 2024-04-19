using Fusion;
using TriInspector;

namespace UnrealTeam.SB.GamePlay.Mining.Views
{
    public class MiningStationSyncView : NetworkBehaviour
    {
        [Networked]
        [field: ShowInInspector, TriInspector.ReadOnly]
        public int ControlledBy { get; set; } = -1;
    }
}