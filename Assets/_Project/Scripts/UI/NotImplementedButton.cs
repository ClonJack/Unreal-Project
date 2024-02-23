using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class NotImplementedButton : MonoBehaviour
    {
        private Button _button;
        

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ThrowNotImpl);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ThrowNotImpl);
        }

        private void ThrowNotImpl()
            => throw new NotImplementedException("Button is not implemented");
    }
}