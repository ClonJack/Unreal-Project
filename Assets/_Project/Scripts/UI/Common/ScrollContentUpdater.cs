using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class ScrollContentUpdater : MonoBehaviour
    {
        [SerializeField] private SheetsRegistration _sheetsRegistration;
        [SerializeField] private DynamicButtonsList _dynamicButtonsList;
        [SerializeField] private ScrollRect _scrollRect;


        private void Awake()
        {
            _dynamicButtonsList.AnyButtonClicked += UpdateScrollContent;
        }

        private void OnDestroy()
        {
            _dynamicButtonsList.AnyButtonClicked -= UpdateScrollContent;
            
        }

        private void UpdateScrollContent(DynamicButtonData buttonData)
        {
            var scrollContentRef = _sheetsRegistration.GetRegistered(buttonData.SheetKey).GetComponent<ScrollContentRef>();
            _scrollRect.content = scrollContentRef.Content;
        }
    }
}