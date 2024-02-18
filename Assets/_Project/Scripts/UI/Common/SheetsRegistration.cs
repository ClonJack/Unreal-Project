using Common.Enums;
using Cysharp.Threading.Tasks;
using Services.Other;
using UnityEngine;
using VContainer;

namespace UI.Common
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
            RegisterSheets();
        }

        private void OnDestroy() 
            => ReleaseSheets();

        private void RegisterSheets()
        {
            foreach (var screenKey in _screenKeys) 
                _screenNavService.RegisterSheetAsync(_containerKey, screenKey).Forget();
        }

        private void ReleaseSheets() 
            => _screenNavService.ReleaseSheets(_containerKey);
    }
}