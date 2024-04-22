using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace UnrealTeam.SB.Common.Game
{
    [AddComponentMenu("Localization/Localize TMP")]
    public class LocalizeTMP : LocalizeStringEvent
    {
        [SerializeField] private TextMeshProUGUI _textMesh;


        private void Awake()
            => OnUpdateString.AddListener(UpdateTextMesh);

        private void UpdateTextMesh(string localizedString)
            => _textMesh.text = localizedString;


        private void OnValidate()
        {
            RefreshString();
            
#if UNITY_EDITOR
            if (_textMesh != null) 
                return;
            
            _textMesh = GetComponent<TextMeshProUGUI>();

            if (_textMesh == null) 
                _textMesh = GetComponentInChildren<TextMeshProUGUI>();

            if (_textMesh != null) 
                EditorUtility.SetDirty(this);
#endif
        }
    }
}