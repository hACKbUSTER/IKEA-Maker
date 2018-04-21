using System;
using System.Collections.Generic;
using UnityEngine;
using FinGameWorks.Scripts.Manager;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using FinGameWorks.Scripts.Controller;
using FinGameWorks.Scripts.View;
using UnityEngine.XR.iOS;

namespace FinGameWorks.Scripts.Manager
{
    public class WorkThroughManager : Singleton<WorkThroughManager>
    {
        public Text StatusText;
        public UnityARGeneratePlane PlaneManager;

        public Slider ProgressBar;
        

        public int CurrentIndex = 0;
        public string GetStepMethod(int stepIndex)
        {
            switch (stepIndex)
            {
                case 1:
                    return "StartGoSmoothPlane";
                case 2:
                    return "StartPositioningBase";
                case 3:
                    return "StartStageOne";
                    case 4:
                        return "StartStageTwo";
                        case 5:
                            return "ResultPage";
            }
            return "";
        }

        public void Next()
        {
            Debug.Log(CurrentIndex);
            string method = GetStepMethod(CurrentIndex + 1);
            Debug.Log(method);
            Invoke(method,0);
            ScrollViewSystem.Instance.CurrentPageIndex++;
            UpdateProgressBar();
        }

        private void Awake()
        {
            
        }

        private void Start()
        {
            
            CurrentIndex = 0;
            QRReader.Instance.QRCodeDetetedEvent.AddListener(res => 
            {
                setToast("Found : " + res);
                Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ =>
                {
                       Next();
                }).AddTo(this);
            });
            QRReader.Instance.StartUpdateRenderTarget();
            setToast("Scanning Product QR Code");
        }

        private void StartGoSmoothPlane()
        {
            setToast("Please find a smooth plane and gaze.");
            CurrentIndex++;
        }

        private void StartPositioningBase()
        {
            setToast("Please put the base to the marked location.");
            
//            PlaneDetecterVisualization.Instance.GetComponent<MeshRenderer>().material.SetColor("_V_WIRE_Color",new Color(1.0f,1.0f,1.0f,0.0f)); 
//            PlaneDetecterVisualization.Instance.MakePlane();
//            PlaneDetecterVisualization.Instance.GetComponent<MeshRenderer>().material.DOColor(new Color(1.0f,1.0f,1.0f,1.0f),"_V_WIRE_Color",0.5f); 
            
            CenterPlaceHolderManager.Instance.GetComponent<ChairController>().baseGO.GetComponent<MeshRenderer>().material.SetColor("_Color",new Color(1.0f,1.0f,1.0f,0.0f));
            CenterPlaceHolderManager.Instance.canDrag = true;
            CenterPlaceHolderManager.Instance.CastCenter();
            CenterPlaceHolderManager.Instance.GetComponent<ChairController>().baseGO.GetComponent<MeshRenderer>().material.DOColor(new Color(1.0f,1.0f,1.0f,0.5f),"_Color",0.5f);
            
//            UnityARGeneratePlane.Instance.unityARAnchorManager.UnsubscribeEvents();
            
            CurrentIndex++;
        }

        private void StartStageOne()
        {
            setToast("Please install the leg");
            ChairController.Instance.HideBase();
            ChairController.Instance.EnableLeftLeg();
            CenterPlaceHolderManager.Instance.canDrag = false;
            CurrentIndex++;
        }

        private void StartStageTwo()
        {
            ChairController.Instance.EnableAllLeg();
            CurrentIndex++;
        }

        private void ResultPage()
        {
            setToast("Yay, you did it yourself! ");
            float currentHeight = ScrollViewSystem.Instance.PanelHeight;
            ScrollViewSystem.Instance.GetRectTransform.DOAnchorPosY(1000,
                1.0f, false);
            CurrentIndex++;
        }

        private void UpdateProgressBar()
        {
            DOTween.To(x => ProgressBar.value = x, ProgressBar.value, (float)(ScrollViewSystem.Instance.CurrentPageIndex + 1)/(float)(ScrollViewSystem.Instance.PageCount), 1.0f);
            
        }

        private void setToast(string text)
        {
            StatusText.DOFade(0.0f, 0.2f);
            StatusText.text = text;
            StatusText.DOFade(1.0f, 0.2f);
        }

 
    }
}