using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnrealTeam.SB.UI.Common
{
    public class DynamicButtonsList : MonoBehaviour
    {
        [SerializeField] private DynamicButtonData[] _buttonsData = {};
        [SerializeField] private TextMeshProUGUI _tooltipPrefab;
        [SerializeField] private bool _clickButtonOnAwake;
        [SerializeField, Min(0)] private int _clickButtonIndex;
        [SerializeField, Min(0)] private float _clickButtonDelay;

        private UnityAction[] _buttonsClickActions;
        private Action[] _buttonsEnterActions;
        private Action[] _buttonsExitActions;

        private Button _activeButton;
        private TextMeshProUGUI _activeButtonTooltip;
        private readonly Dictionary<Button, TextMeshProUGUI> _hoveredButtonsTooltips = new();

        public event Action<DynamicButtonData> AnyButtonClicked;


        private void Start()
        {
            RegisterButtonsActions();
            InvokeDefaultButton().Forget();
        }

        private void OnDestroy()
        {
            UnregisterButtonsActions();
        }

        private void OnButtonClicked(DynamicButtonData buttonData)
        {
            AnyButtonClicked?.Invoke(buttonData);
            
            if (_activeButtonTooltip != null)
                Destroy(_activeButtonTooltip.gameObject);

            if (_activeButton != null)
                _activeButton.interactable = true;

            _activeButton = buttonData.Button;
            buttonData.Button.interactable = false;

            _activeButtonTooltip = Instantiate(_tooltipPrefab, buttonData.Button.transform);
            _activeButtonTooltip.text = buttonData.Name;
        }

        private void OnButtonEntered(DynamicButtonData buttonData)
        {
            if (buttonData.Button == _activeButton)
                return;
            
            if (_hoveredButtonsTooltips.ContainsKey(buttonData.Button))
                return;
            
            _hoveredButtonsTooltips[buttonData.Button] = Instantiate(_tooltipPrefab, buttonData.Button.transform);
            _hoveredButtonsTooltips[buttonData.Button].text = buttonData.Name;
        }

        private void OnButtonExited(DynamicButtonData buttonData)
        {
            if (_hoveredButtonsTooltips.TryGetValue(buttonData.Button, out TextMeshProUGUI buttonTooltip))
            {
                Destroy(buttonTooltip.gameObject);
                _hoveredButtonsTooltips.Remove(buttonData.Button);
            }
        }

        private void RegisterButtonsActions()
        {
            _buttonsClickActions = new UnityAction[_buttonsData.Length];
            _buttonsEnterActions = new Action[_buttonsData.Length];
            _buttonsExitActions = new Action[_buttonsData.Length];
            
            for (var i = 0; i < _buttonsData.Length; i++)
            {
                var buttonData = _buttonsData[i];
                RegisterButtonClickAction(buttonData, i);
                RegisterButtonEnterAction(buttonData, i);
                RegisterButtonExitAction(buttonData, i);
            }
        }

        private void UnregisterButtonsActions()
        {
            for (var i = 0; i < _buttonsData.Length; i++)
            {
                var buttonData = _buttonsData[i];
                UnregisterButtonClickAction(buttonData, i);
                UnregisterButtonEnterAction(buttonData, i);
                UnregisterButtonExitAction(buttonData, i);
            }
        }

        private async UniTask InvokeDefaultButton()
        {
            if (_clickButtonOnAwake)
            {
                if (_clickButtonDelay > 0)
                    await UniTask.WaitForSeconds(_clickButtonDelay, delayTiming: PlayerLoopTiming.FixedUpdate);
                
                _buttonsData[_clickButtonIndex].Button.onClick.Invoke();
            }
        }

        private void RegisterButtonClickAction(DynamicButtonData buttonData, int index)
        {
            UnityAction buttonAction = () => OnButtonClicked(buttonData);
            buttonData.Button.onClick.AddListener(buttonAction);
            _buttonsClickActions[index] = buttonAction;
        }

        private void RegisterButtonEnterAction(DynamicButtonData buttonData, int index)
        {
            Action buttonAction = () => OnButtonEntered(buttonData);
            buttonData.HoverEvents.Entered += buttonAction;
            buttonData.HoverEvents.Selected += buttonAction;
            _buttonsEnterActions[index] = buttonAction;
        }

        private void RegisterButtonExitAction(DynamicButtonData buttonData, int index)
        {
            Action buttonAction = () => OnButtonExited(buttonData);
            buttonData.HoverEvents.Exited += buttonAction;
            buttonData.HoverEvents.Deselected += buttonAction;
            _buttonsExitActions[index] = buttonAction;
        }

        private void UnregisterButtonClickAction(DynamicButtonData buttonData, int index)
        {
            UnityAction buttonAction = _buttonsClickActions[index];
            buttonData.Button.onClick.RemoveListener(buttonAction);
        }

        private void UnregisterButtonEnterAction(DynamicButtonData buttonData, int index)
        {
            Action buttonAction = _buttonsEnterActions[index];
            buttonData.HoverEvents.Entered -= buttonAction;
            buttonData.HoverEvents.Selected -= buttonAction;
        }

        private void UnregisterButtonExitAction(DynamicButtonData buttonData, int index)
        {
            Action buttonAction = _buttonsExitActions[index];
            buttonData.HoverEvents.Exited -= buttonAction;
            buttonData.HoverEvents.Deselected -= buttonAction;
        }
    }
}