using UnityEngine;

namespace FinGameWorks.Scripts.Controller
{
    public class ChairController : Singleton<ChairController>
    {
        public GameObject baseGO;
        public GameObject leftGO;
        public GameObject rightGO;
        public GameObject upGO;
        public GameObject downGO;

        private void Start()
        {
            baseGO.SetActive(true);
            leftGO.SetActive(false);
            rightGO.SetActive(false);
            upGO.SetActive(false);
            downGO.SetActive(false);

        }

        public void HideBase()
        {
            baseGO.SetActive(false);
        }

        public void EnableLeftLeg()
        {
            leftGO.SetActive(true);
        }

        public void EnableAllLeg()
        {
            rightGO.SetActive(true);
            upGO.SetActive(true);
            downGO.SetActive(true);
        }
    }
}