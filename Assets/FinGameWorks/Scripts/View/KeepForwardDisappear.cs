using UnityEngine;
using DG.Tweening;

namespace FinGameWorks.Scripts.View
{
    public class KeepForwardDisappear : MonoBehaviour
    {
        public Vector3 InitialLocalPos;
        public Vector3 EndLocalPos;

        private void OnEnable()
        {
            transform.localPosition = InitialLocalPos;
            transform.DOLocalMove(EndLocalPos, 2f)
                .SetEase(Ease.InOutCubic)
                .OnComplete((() =>
                {
                    transform.localPosition = InitialLocalPos;
                })).SetLoops(-1,LoopType.Restart);
        }
    }
}