using UnityEngine;

namespace FinGameWorks.Scripts.View
{
    public class KeepRotation : MonoBehaviour
    {
        public float rotateSpeed = 1; 
        private void FixedUpdate()
        {
            Transform t = GetComponent<Transform>();
            t.localEulerAngles = new Vector3(t.localEulerAngles.x,t.localEulerAngles.y,t.localEulerAngles.z + rotateSpeed);
        }
    }
}