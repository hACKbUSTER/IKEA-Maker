using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace FinGameWorks.Scripts.View
{
    public class SplitScreenCanvas : MonoBehaviour
    {
        public CanvasScaler Scaler;
        public LayoutElement LeftLayoutElement;
        public LayoutElement RightLayoutElement;

        private void OnEnable()
        {
            WindowRatioMonitor.Instance.ScreenSizeChangedAction.AddListener(new UnityAction<Rect>((rect) =>
            {
                bool isLandscape = (rect.width > rect.height);
                LeftLayoutElement.DOFlexibleSize(new Vector2(isLandscape ? 7 : 0, LeftLayoutElement.flexibleHeight),
                    0.5f, true);
                Scaler.matchWidthOrHeight = isLandscape ? 1.0f : 0.5f;
            }));
        }

        private void OnDisable()
        {
        }
    }
}