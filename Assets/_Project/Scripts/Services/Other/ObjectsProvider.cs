using UI;
using UI.Refs;
using UnityEngine;

namespace Services.Other
{
    public class ObjectsProvider
    {
        public Camera UiCamera { get; set; }
        public LoadingCurtainCanvasRefs CurtainRefs { get; set; }
        public MainMenuCanvasRefs MenuCanvasRefs { get; set; }
    }
}