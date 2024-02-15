using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class ConfirmQuitGameButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        
        private void Awake() 
            => _button.onClick.AddListener(QuitApplication);

        private void OnDestroy()
            => _button.onClick.RemoveListener(QuitApplication);

        private void QuitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}