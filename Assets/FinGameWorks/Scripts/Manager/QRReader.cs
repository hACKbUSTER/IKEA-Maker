using System;
using CI.TaskParallel;
using UnityEngine;
using UnityEngine.Events;
using ZXing;

namespace FinGameWorks.Scripts.Manager
{
    public class QRReader : Singleton<QRReader>
    {
        public RenderTexture GetRenderTexture;
        public Material Skybox;
        public Texture2D Texture;
        
        public class QRCodeEvent : UnityEvent<String>
        {
			
        }
        public QRCodeEvent QRCodeDetetedEvent
        {
            get { return _QRCodeDetetedEvent ?? (_QRCodeDetetedEvent = new QRCodeEvent()); }
            set { _QRCodeDetetedEvent = value; }
        }

        private QRCodeEvent _QRCodeDetetedEvent;

        private void Start()
        {
            UnityTask.InitialiseDispatcher();
        }

        public void StartUpdateRenderTarget()
        {
            InvokeRepeating("UpdateRenderTarget", 1.0f, 1.0f);
        }

        void UpdateRenderTarget()
        {
            try
            {
                if (GetRenderTexture!= null) 
                {
                    GetRenderTexture.Release();
                    Destroy(GetRenderTexture);
                }
                GetRenderTexture = new RenderTexture( Screen.width/4, Screen.height/4,24);
                if (Texture != null)
                {
                    Destroy(Texture);
                }
                Texture = new Texture2D(GetRenderTexture.width, GetRenderTexture.height);
                
                Camera.main.targetTexture = GetRenderTexture;
                RenderTexture.active = GetRenderTexture;
                int width = Texture.width;
                int height = Texture.height;
                Camera.main.Render();
                Texture.ReadPixels(new Rect(0, 0, width,height), 0, 0,false);
                Texture.Apply(false);
//                Skybox.mainTexture = Texture;
                Color32[] colores = Texture.GetPixels32();
                var result = "";
                UnityTask.Run(() =>
                {
                    IBarcodeReader barcodeReader = new BarcodeReader();
                    var res = barcodeReader.Decode(colores, width, height);
                    result = res.Text;
                }).ContinueOnUIThread((r) =>
                {
                    RenderTexture.active = null;
                    Camera.main.targetTexture = null;
                    colores = null;
                    
                    if (result != "")
                    {
                        Debug.Log(result);
                        CancelInvoke();
                        Destroy(GetRenderTexture);
                        Destroy(Texture);
                        QRCodeDetetedEvent.Invoke(result);
                    }
                });
                
//                if (result != null)
//                {
//                    Debug.Log(result.Text);
//                    CancelInvoke();
//                    Destroy(GetRenderTexture);
//                    Destroy(Texture);
//                    QRCodeDetetedEvent.Invoke(result.Text);
//                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning (ex.Message); 
            }
        }
    }
}