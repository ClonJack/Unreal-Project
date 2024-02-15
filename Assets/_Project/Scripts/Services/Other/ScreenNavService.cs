using System;
using Common.Enums;
using Cysharp.Threading.Tasks;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Page;
using VContainer;
using VContainer.Unity;

namespace Services.Other
{
    public class ScreenNavService
    {
        private readonly IObjectResolver _objectResolver;


        public ScreenNavService(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public async UniTask OpenAsync(ScreenType screenType, ContainerKey containerKey, ScreenKey screenKey, bool playAnim = true)
        {
            switch (screenType)
            {
                case ScreenType.Page:
                    await OpenPageAsync(containerKey, screenKey, playAnim);
                    break;
                case ScreenType.Modal:
                    await OpenModalAsync(containerKey, screenKey, playAnim);
                    break;
                case ScreenType.Sheet:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(screenType), screenType, null);
            }
        }

        public async UniTask OpenPageAsync(ContainerKey containerKey, ScreenKey screenKey, bool playAnim = true)
        {
            var pageContainer = PageContainer.Find(containerKey.ToString());
            await pageContainer
                .Push(
                    screenKey.ToString(), playAnim,
                    onLoad: x => _objectResolver.InjectGameObject(x.page.gameObject))
                .ToUniTask();
        }

        public async UniTask OpenModalAsync(ContainerKey containerKey, ScreenKey screenKey, bool playAnim = true)
        {
            var modalContainer = ModalContainer.Find(containerKey.ToString());
            await modalContainer
                .Push(
                    screenKey.ToString(), playAnim,
                    onLoad: x => _objectResolver.InjectGameObject(x.modal.gameObject))
                .ToUniTask();
        }

        public async UniTask CloseAsync(ScreenType screenType, ContainerKey containerKey, bool playAnim = true)
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
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(screenType), screenType, null);
            }
        }

        public async UniTask ClosePageAsync(ContainerKey containerKey, bool playAnim = true)
        {
            var pageContainer = PageContainer.Find(containerKey.ToString());
            await pageContainer.Pop(playAnim).ToUniTask();
        }

        public async UniTask CloseModalAsync(ContainerKey containerKey, bool playAnim = true)
        {
            var modalContainer = ModalContainer.Find(containerKey.ToString());
            await modalContainer.Pop(playAnim).ToUniTask();
        }
    }
}