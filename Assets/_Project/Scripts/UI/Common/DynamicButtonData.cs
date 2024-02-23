using System;
using UnityEngine;
using UnityEngine.UI;
using UnrealTeam.SB.Common.Enums;

namespace UnrealTeam.SB.UI.Common
{
    [Serializable]
    public class DynamicButtonData
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public HoverEvents HoverEvents { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ScreenKey SheetKey { get; private set; }
    }
}