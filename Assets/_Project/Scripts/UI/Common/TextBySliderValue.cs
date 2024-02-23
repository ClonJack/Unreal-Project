using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class TextBySliderValue : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _textMesh;


        private void OnEnable() 
            => _slider.onValueChanged.AddListener(UpdateText);

        private void OnDisable() 
            => _slider.onValueChanged.RemoveListener(UpdateText);

        private void UpdateText(float value)
            => _textMesh.text = $"{Mathf.RoundToInt(value)}%";
    }
}