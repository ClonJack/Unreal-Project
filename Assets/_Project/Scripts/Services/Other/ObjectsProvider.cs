using UnityEngine;
using UnrealTeam.SB.UI.Refs;

namespace UnrealTeam.SB.Services.Other
{
    public class ObjectsProvider
    {
        private Transform _gameCameraTransform;
        
        public Transform SpawnPoint { get; set; }
        public Transform PlayerBound { get; set; } 
        public Camera UiCamera { get; set; }
        public LoadingCurtainCanvasRefs CurtainRefs { get; set; }
        public HudCanvasRefs HudRefs { get; set; }

        public Camera GameCamera { get; set; }
        public Transform GameCameraTransform => _gameCameraTransform ??= GameCamera.transform;
    }
}