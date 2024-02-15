using System;
using Common.Enums;
using Cysharp.Threading.Tasks;
using Services.Other;
using UnityEngine;
using VContainer;

namespace UI.MainMenu
{
    public class SheetsRegistration : MonoBehaviour
    {
        [SerializeField] private ContainerKey _containerKey;
        [SerializeField] private ScreenKey[] _screenKeys;
        
        private ScreenNavService _screenNavService;


        [Inject]
        public void Construct(ScreenNavService screenNavService)
        {
            _screenNavService = screenNavService;
        }
        
        private void Start() 
            => RegisterSheets().Forget();

        private void OnDestroy() 
            => ReleaseSheets();

        private async UniTask RegisterSheets()
        {
            foreach (var screenKey in _screenKeys) 
                await _screenNavService.RegisterSheetAsync(_containerKey, screenKey);
        }

        private void ReleaseSheets() 
            => _screenNavService.ReleaseSheets(_containerKey);
    }
}