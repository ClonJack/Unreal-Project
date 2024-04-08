using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnrealTeam.SB.Common.Enums.ScreenNav;
using UnrealTeam.SB.Services.Other;
using VContainer;

namespace UnrealTeam.SB.UI.MainMenu
{
    public class OpenScreenButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ScreenType _screenType;
        [SerializeField] private ContainerKey _containerKey;
        [SerializeField] private ScreenKey _screenKey;
        
        private ScreenNavService _screenNavService;


        [Inject]
        public void Construct(ScreenNavService screenNavService)
        {
            _screenNavService = screenNavService;
        }

        private void Awake() 
            => _button.onClick.AddListener(OpenScreen);

        private void OnDestroy() 
            => _button.onClick.RemoveListener(OpenScreen);

        private void OpenScreen() 
            => _screenNavService.OpenAsync(_screenType, _containerKey, _screenKey).Forget();
    }
}