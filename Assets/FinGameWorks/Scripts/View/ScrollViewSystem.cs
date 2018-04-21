using System;
using DG.Tweening;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
namespace FinGameWorks.Scripts.View
{
    [ExecuteInEditMode]
    public class ScrollViewSystem : Singleton<ScrollViewSystem>
    {
        public Canvas MainCanvas;
        public ETCTouchPad TouchPad;
        
        public float VerticalProgress
        {
            get { return _verticalProgress; }
            set { _verticalProgress = value; }
        }
        private float _verticalProgress = 0;

        public float PanelHeight
        {
            get { return _panelHeight; }
            set
            {
                _panelHeight = value;
                UpdateUI();
            }
        }
        private float _panelHeight = 10000;
        
        
        public float PanelRoundRadius = 32;

        public RectTransform GetRectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = gameObject.GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
            set { _rectTransform = value; }
        }
        private RectTransform _rectTransform = null;


        public GridLayoutGroup GetGridLayoutGroup;

        private void OnEnable()
        {
            WindowRatioMonitor.Instance.ScreenSizeChangedAction.AddListener(new UnityAction<Rect>((rect) =>
            {
                UpdateUI();
            }));
        }

        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                _pageCount = value;
                UpdateUI();
            }
        }

        private int _pageCount = 6;
        
        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set
            {
                _currentPageIndex = value;
                UpdateUI();
#if UNITY_EDITOR
                GetGridLayoutGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2((GetGridLayoutGroup.cellSize.x + GetGridLayoutGroup.spacing.x) * CurrentPageIndex,GetGridLayoutGroup.GetComponent<RectTransform>().anchoredPosition.y);
#else
GetGridLayoutGroup.GetComponent<RectTransform>().DOAnchorPosX(-
                    (GetGridLayoutGroup.cellSize.x + GetGridLayoutGroup.spacing.x) * CurrentPageIndex, 0.5f,
                    false);
#endif
            }
        }

        private int _currentPageIndex = 0;

        private void OnDisable()
        {
            
        }

        private void Awake()
        {
            
        }

        private Vector2 dragSpeed;
        private void Start()
        {
            TouchPad.onMoveStart.AddListener((() =>
            {
                dragSpeed = Vector2.zero;
            }));
            TouchPad.onMove.AddListener((vector =>
            {
                float nextVertical = GetRectTransform.anchoredPosition.y + vector.y;
                if (nextVertical < 320.0f)
                {
                    nextVertical = 320.0f;
                }
                if (nextVertical > 1000.0f)
                {
                    nextVertical = 1000.0f;
                }
                   GetRectTransform.anchoredPosition = new Vector2(GetRectTransform.anchoredPosition.x,nextVertical);
            }));
            TouchPad.onMoveEnd.AddListener((() =>
            {
                float nextVertical = GetRectTransform.anchoredPosition.y + dragSpeed.y;
                float time = dragSpeed.y * 0.1f;
                if (nextVertical < 320.0f)
                {
                    nextVertical = 320.0f;
                    time = 0.0f;
                }
                if (nextVertical > 1000.0f)
                {
                    nextVertical = 1000.0f;
                    time = 0.0f;
                }
                if (Math.Abs(time) > 0.02f)
                {
                    GetRectTransform.DOAnchorPosY(nextVertical,
                        time, false);
                }
                dragSpeed = Vector2.zero;
            }));
            TouchPad.onMoveSpeed.AddListener((vector =>
            {
                Debug.Log(vector);
                dragSpeed = vector;
            }));
        }

        private void Update()
        {
            if (Math.Abs(PanelHeight - Camera.main.pixelHeight) > 0.1)
            {
                PanelHeight = Camera.main.pixelHeight;
            }
        }

        private void UpdateUI()
        {
            GetRectTransform.sizeDelta = new Vector2(GetRectTransform.sizeDelta.x, _panelHeight);

//            Debug.Log("transform.parent.GetComponent<RectTransform>().sizeDelta = " + transform.parent.GetComponent<RectTransform>().sizeDelta);
            float cellWidth = transform.parent.GetComponent<RectTransform>().sizeDelta.x;
//            Debug.Log("cellWidth = " + cellWidth);
            float scrollViewWidth = (cellWidth + GetGridLayoutGroup.spacing.x) * (PageCount + 1);
            
//            Debug.Log("scrollViewWidth = " + scrollViewWidth);
//            Debug.Log("GetGridLayoutGroup.rect = " + GetGridLayoutGroup.GetComponent<RectTransform>().rect);
            
            GetGridLayoutGroup.cellSize = new Vector2(cellWidth,GetRectTransform.sizeDelta.y);
            GetGridLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollViewWidth,GetGridLayoutGroup.GetComponent<RectTransform>().sizeDelta.y);
 
        }
    }
}