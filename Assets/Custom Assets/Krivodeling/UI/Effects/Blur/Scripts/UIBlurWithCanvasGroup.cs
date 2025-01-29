using UnityEngine;

namespace Krivodeling.UI.Effects.Examples
{
    public class UIBlurWithCanvasGroup : MonoBehaviour
    {
        #region Variables
        private UIBlur uiblur;
        private CanvasGroup CanvasGroup;
        #endregion

        #region Methods
        void Start()
        {
            SetComponents();

            uiblur.onBeginBlur.AddListener(() => CanvasGroup.blocksRaycasts = true);
            uiblur.onBlurChanged.AddListener(OnBlurChanged);
            uiblur.onEndBlur.AddListener(() => CanvasGroup.blocksRaycasts = false);
        }

        private void SetComponents()
        {
            uiblur = GetComponent<UIBlur>();
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnBlurChanged(float value)
        {
            CanvasGroup.alpha = value;
        }
        #endregion
    }
}
