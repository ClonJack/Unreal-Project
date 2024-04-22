using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityScreenNavigator.Runtime.Core.Sheet;
using UnrealTeam.SB.Additional.Enums.ScreenNav;

namespace UnrealTeam.SB.UI.MainMenu
{
    public class SelectOnScreenChanged : MonoBehaviour
    {
        [SerializeField] private ScreenType _screenType = ScreenType.Page;
        [SerializeField] private GameObject _toSelect;
        
        private EventSystem _eventSystem;
        private GameObject _lastSelected;
        private bool _isOpened;
        private bool _alreadyChanged;


        private void Awake()
        {
            _eventSystem = EventSystem.current;
        }

        private void OnEnable()
        {
            switch (_screenType)
            {
                case ScreenType.Page:
                    var page = GetComponent<Page>();
                    page.TransitionAnimationProgressChanged += OnTransitionProgress;
                    break;
                case ScreenType.Modal:
                    var modal = GetComponent<Modal>();
                    modal.TransitionAnimationProgressChanged += OnTransitionProgress;
                    break;
                case ScreenType.Sheet:
                    var sheet = GetComponent<Sheet>();
                    sheet.TransitionAnimationProgressChanged += OnTransitionProgress;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnDisable()
        {
            switch (_screenType)
            {
                case ScreenType.Page:
                    var page = GetComponent<Page>();
                    page.TransitionAnimationProgressChanged -= OnTransitionProgress;
                    break;
                case ScreenType.Modal:
                    var modal = GetComponent<Modal>();
                    modal.TransitionAnimationProgressChanged -= OnTransitionProgress;
                    break;
                case ScreenType.Sheet:
                    var sheet = GetComponent<Sheet>();
                    sheet.TransitionAnimationProgressChanged -= OnTransitionProgress;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnTransitionProgress(float progress)
        {
            if (progress == 0)
            {
                if (_alreadyChanged)
                    _alreadyChanged = false;
                
                if (!_isOpened)
                    _lastSelected = _eventSystem.currentSelectedGameObject;
            }
            
            if (progress == 1)
            {
                if (!_alreadyChanged)
                {
                    _alreadyChanged = true;
                    _isOpened = !_isOpened;
                }

                _eventSystem.SetSelectedGameObject(_isOpened ? _toSelect : _lastSelected);
            }
        }
    }
}