using System;
using Cysharp.Threading.Tasks;
using Services.Other;
using UI.Refs;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.MainMenu
{
    public class BackToMenuButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ScreenNavType _backFrom;
        
        private MainMenuCanvasRefs _menuCanvasRefs;


        [Inject]
        public void Construct(ObjectsProvider objectsProvider)
        {
            _menuCanvasRefs = objectsProvider.MenuCanvasRefs;
        }

        private void Awake() 
            => _button.onClick.AddListener(BackToMenu);

        private void OnDestroy() 
            => _button.onClick.RemoveListener(BackToMenu);

        private void BackToMenu()
        {
            switch (_backFrom)
            {
                case ScreenNavType.Page:
                    _menuCanvasRefs.MainPageContainer.Pop(true).ToUniTask().Forget();
                    break;
                case ScreenNavType.Modal:
                    _menuCanvasRefs.MainModalContainer.Pop(true).ToUniTask().Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}