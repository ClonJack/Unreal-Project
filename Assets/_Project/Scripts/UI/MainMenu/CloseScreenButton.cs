using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnrealTeam.SB.Common.Enums;
using UnrealTeam.SB.Services.Other;
using VContainer;

namespace UnrealTeam.SB.UI.MainMenu
{
    public class CloseScreenButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ScreenType _screenType = ScreenType.Page;
        [SerializeField] private ContainerKey _containerKey;
        
        private ScreenNavService _screenNavService;


        [Inject]
        public void Construct(ScreenNavService screenNavService)
        {
            _screenNavService = screenNavService;
        }
        
        private void Awake() 
            => _button.onClick.AddListener(CloseScreen);

        private void OnDestroy() 
            => _button.onClick.RemoveListener(CloseScreen);

        private void CloseScreen()
            => _screenNavService.CloseAsync(_screenType, _containerKey).Forget();
    }
}