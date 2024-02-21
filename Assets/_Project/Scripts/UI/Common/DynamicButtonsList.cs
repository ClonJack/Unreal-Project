using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Common
{
    public class DynamicButtonsList : MonoBehaviour
    {
        [SerializeField] private DynamicButtonData[] _buttonsData = {};
        [SerializeField] private TextMeshProUGUI _tooltipPrefab;
        [SerializeField] private bool _clickButtonOnAwake;
        [SerializeField, Min(0)] private int _clickButtonIndex;

        private UnityAction[] _buttonsClickActions;
        private Action<PointerEventData>[] _buttonsEnterActions;
        private Action<PointerEventData>[] _buttonsExitActions;

        private Button _activeButton;
        private TextMeshProUGUI _activeButtonTooltip;
        private Dictionary<Button, TextMeshProUGUI> _hoveredButtonsTooltips = new();


        private void Start()
        {
            RegisterButtonsActions();
            ClickDefaultButton();
        }

        private void OnDestroy()
        {
            UnregisterButtonsActions();
        }

        private void OnButtonClicked(DynamicButtonData buttonData)
        {
            if (_activeButtonTooltip != null)
                Destroy(_activeButtonTooltip.gameObject);

            if (_activeButton != null)
                _activeButton.interactable = true;

            _activeButton = buttonData.Button;
            buttonData.Button.interactable = false;

            _activeButtonTooltip = Instantiate(_tooltipPrefab, buttonData.Button.transform);
            _activeButtonTooltip.text = buttonData.Name;
        }

        private void OnButtonEntered(DynamicButtonData buttonData, PointerEventData eventData)
        {
            if (buttonData.Button == _activeButton)
                return;
            
            _hoveredButtonsTooltips[buttonData.Button] = Instantiate(_tooltipPrefab, buttonData.Button.transform);
            _hoveredButtonsTooltips[buttonData.Button].text = buttonData.Name;
        }

        private void OnButtonExited(DynamicButtonData buttonData, PointerEventData eventData)
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
            _buttonsEnterActions = new Action<PointerEventData>[_buttonsData.Length];
            _buttonsExitActions = new Action<PointerEventData>[_buttonsData.Length];
            
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

        private void ClickDefaultButton()
        {
            if (_clickButtonOnAwake)
                _buttonsData[_clickButtonIndex].Button.onClick.Invoke();
        }

        private void RegisterButtonClickAction(DynamicButtonData buttonData, int index)
        {
            UnityAction buttonAction = () => OnButtonClicked(buttonData);
            buttonData.Button.onClick.AddListener(buttonAction);
            _buttonsClickActions[index] = buttonAction;
        }

        private void RegisterButtonEnterAction(DynamicButtonData buttonData, int index)
        {
            Action<PointerEventData> buttonAction = eventData => OnButtonEntered(buttonData, eventData);
            buttonData.HoverEvents.Entered += buttonAction;
            _buttonsEnterActions[index] = buttonAction;
        }

        private void RegisterButtonExitAction(DynamicButtonData buttonData, int index)
        {
            Action<PointerEventData> buttonAction = eventData => OnButtonExited(buttonData, eventData);
            buttonData.HoverEvents.Exited += buttonAction;
            _buttonsEnterActions[index] = buttonAction;
        }

        private void UnregisterButtonClickAction(DynamicButtonData buttonData, int index)
        {
            UnityAction buttonAction = _buttonsClickActions[index];
            buttonData.Button.onClick.RemoveListener(buttonAction);
        }

        private void UnregisterButtonEnterAction(DynamicButtonData buttonData, int index)
        {
            Action<PointerEventData> buttonAction = _buttonsEnterActions[index];
            buttonData.HoverEvents.Entered -= buttonAction;
        }

        private void UnregisterButtonExitAction(DynamicButtonData buttonData, int index)
        {
            Action<PointerEventData> buttonAction = _buttonsEnterActions[index];
            buttonData.HoverEvents.Exited -= buttonAction;
        }
    }
}