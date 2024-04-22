using System;
using Cysharp.Threading.Tasks;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityScreenNavigator.Runtime.Core.Sheet;
using UnrealTeam.SB.Additional.Enums.ScreenNav;
using VContainer;
using VContainer.Unity;

namespace UnrealTeam.SB.Services.Other
{
    public class ScreenNavService
    {
        private readonly IObjectResolver _objectResolver;


        public ScreenNavService(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public async UniTask OpenAsync(
            ScreenType screenType, ContainerKey containerKey, ScreenKey screenKey,
            bool playAnim = true, Action onOpened = null)
        {
            switch (screenType)
            {
                case ScreenType.Page:
                    await OpenPageAsync(containerKey, screenKey, playAnim, onOpened);
                    break;
                case ScreenType.Modal:
                    await OpenModalAsync(containerKey, screenKey, playAnim, onOpened);
                    break;
                case ScreenType.Sheet:
                    await OpenSheetAsync(containerKey, screenKey, playAnim, onOpened);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(screenType), screenType, null);
            }
        }

        public async UniTask CloseAsync(
            ScreenType screenType, ContainerKey containerKey, 
            bool playAnim = true)
        {
            switch (screenType)
            {
                case ScreenType.Page:
                    await ClosePageAsync(containerKey, playAnim);
                    break;
                case ScreenType.Modal:
                    await CloseModalAsync(containerKey, playAnim);
                    break;
                case ScreenType.Sheet:
                    await CloseSheetAsync(containerKey, playAnim);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(screenType), screenType, null);
            }
        }

        public async UniTask OpenPageAsync(
            ContainerKey containerKey, ScreenKey screenKey, 
            bool playAnim = true, Action onOpened = null)
        {
            var pageContainer = PageContainer.Find(containerKey.ToString());
            await pageContainer
                .Push(
                    screenKey.ToString(), playAnim,
                    onLoad: x =>
                    {
                        _objectResolver.InjectGameObject(x.page.gameObject);
                    })
                .ToUniTask();
            
            onOpened?.Invoke();
        }

        public async UniTask ClosePageAsync(ContainerKey containerKey, bool playAnim = true)
        {
            var pageContainer = PageContainer.Find(containerKey.ToString());
            await pageContainer.Pop(playAnim).ToUniTask();
        }

        public async UniTask OpenModalAsync(
            ContainerKey containerKey, ScreenKey screenKey, 
            bool playAnim = true, Action onOpened = null)
        {
            var modalContainer = ModalContainer.Find(containerKey.ToString());
            await modalContainer
                .Push(
                    screenKey.ToString(), playAnim,
                    onLoad: x => _objectResolver.InjectGameObject(x.modal.gameObject))
                .ToUniTask();
            
            onOpened?.Invoke();
        }

        public async UniTask CloseModalAsync(ContainerKey containerKey, bool playAnim = true)
        {
            var modalContainer = ModalContainer.Find(containerKey.ToString());
            await modalContainer.Pop(playAnim).ToUniTask();
        }

        public async UniTask<Sheet> RegisterSheetAsync(ContainerKey containerKey, ScreenKey screenKey)
        {
            var sheetContainer = SheetContainer.Find(containerKey.ToString());
            Sheet sheet = null;
            await sheetContainer
                .Register(
                    screenKey.ToString(),
                    onLoad: x =>
                    {
                        _objectResolver.InjectGameObject(x.sheet.gameObject);
                        sheet = x.sheet;
                    })
                .ToUniTask();

            return sheet;
        }

        public void ReleaseSheets(ContainerKey containerKey)
        {
            var sheetContainer = SheetContainer.Find(containerKey.ToString());
            if (sheetContainer != null)
                sheetContainer.UnregisterAll();
        }

        public async UniTask OpenSheetAsync(
            ContainerKey containerKey, ScreenKey screenKey, 
            bool playAnim = true, Action onOpened = null)
        {
            var sheetContainer = SheetContainer.Find(containerKey.ToString());
            await sheetContainer.ShowByResourceKey(screenKey.ToString(), playAnim).ToUniTask();
            onOpened?.Invoke();
        }
        
        public async UniTask CloseSheetAsync(ContainerKey containerKey, bool playAnim = true)
        {
            var sheetContainer = SheetContainer.Find(containerKey.ToString());
            await sheetContainer.Hide(playAnim).ToUniTask();
        }
    }
}