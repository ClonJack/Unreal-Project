using Cysharp.Threading.Tasks;
using Services.Other;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using VContainer;

namespace UI.Menu
{
    public class BackMenuButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private PageContainer _pageContainer;


        [Inject]
        public void Construct(ObjectsProvider objectsProvider)
        {
            _pageContainer = objectsProvider.MenuCanvasRefs.MainPageContainer;
            _button.onClick.AddListener(BackToMenu);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(BackToMenu);
        }

        private void BackToMenu() 
            => _pageContainer.Pop(true).ToUniTask().Forget();
    }
}