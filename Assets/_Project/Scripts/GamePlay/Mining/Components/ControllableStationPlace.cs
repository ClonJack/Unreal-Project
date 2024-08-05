using UnityEngine;

namespace UnrealTeam.SB.GamePlay.Mining.Components
{
    public struct ControllableStationPlace
    {
        public Collider Collider;
        public Transform SitPoint;
        public LastPlacePlayer LastPlacePlayer;
    }

    public struct LastPlacePlayer
    {
        public Vector3 Position;
        public Quaternion Rotate;
    }
}