using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Mining.Components
{
    public struct ControllableStationPlace
    {
        public Collider Collider;
        public Transform SitPoint;
        public Vector3 LastPosition;
    }
}