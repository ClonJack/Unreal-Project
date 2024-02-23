using System.Collections.Generic;
using Common.Enums;
using Cysharp.Threading.Tasks;
using Services.Other;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Sheet;
using VContainer;

namespace UI.Common
{
    public class SheetsRegistration : MonoBehaviour
    {
        [SerializeField] private ContainerKey _containerKey;
        [SerializeField] private ScreenKey[] _screenKeys;

        private ScreenNavService _screenNavService;
        private readonly Dictionary<ScreenKey, Sheet> _registeredSheets = new();


        [Inject]
        public void Construct(ScreenNavService screenNavService)
        {
            _screenNavService = screenNavService;
            RegisterSheets();
        }

        private void OnDestroy()
            => ReleaseSheets();

        public Sheet GetRegistered(ScreenKey screenKey)
            => _registeredSheets[screenKey];

        private void RegisterSheets()
        {
            foreach (var screenKey in _screenKeys)
                RegisterSheet(screenKey).Forget();
        }

        private void ReleaseSheets()
            => _screenNavService.ReleaseSheets(_containerKey);

        private async UniTask RegisterSheet(ScreenKey screenKey)
            => _registeredSheets[screenKey] = await _screenNavService.RegisterSheetAsync(_containerKey, screenKey);
    }
}