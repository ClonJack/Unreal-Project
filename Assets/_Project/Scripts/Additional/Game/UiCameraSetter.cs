using UnityEngine;
using UnrealTeam.SB.Services.Other;
using VContainer;

namespace UnrealTeam.SB.Common.Game
{
    [RequireComponent(typeof(Canvas))]
    public class UiCameraSetter : MonoBehaviour
    {
        [SerializeField] private RenderMode _renderMode = RenderMode.ScreenSpaceCamera;
        [SerializeField] private int _planeDistance = 5;
        
        private ObjectsProvider _objectsProvider;
        
        
        [Inject]
        public void Construct(ObjectsProvider objectsProvider)
        {
            _objectsProvider = objectsProvider;
        }

        private void Start()
        {
            var canvas = GetComponent<Canvas>();
            canvas.renderMode = _renderMode;
            canvas.worldCamera = _objectsProvider?.UiCamera;
            canvas.planeDistance = _planeDistance;
        }
    }
}
