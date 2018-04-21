using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

namespace FinGameWorks.Scripts.Manager
{
    public class CenterPlaceHolderManager : Singleton<CenterPlaceHolderManager>
    {
		public Transform m_HitTransform;
		public float maxRayDistance = 30.0f;
		public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

        bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                foreach (var hitResult in hitResults) {
//                    Debug.Log ("Got hit!");
                    m_HitTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                    m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
//                    Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                    return true;
                }
            }
            return false;
        }

	    public void CastCenter()
	    {
		    var screenPosition = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width/2,Screen.height/2));
		    Debug.Log(screenPosition);
		    ARPoint point = new ARPoint {
			    x = screenPosition.x,
			    y = screenPosition.y
		    };

		    // prioritize reults types
		    ARHitTestResultType[] resultTypes = {
			    //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
			    ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
			    // if you want to use infinite planes use this:
			    //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
			    //ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
			    //ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
			    //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
		    }; 
					
		    foreach (ARHitTestResultType resultType in resultTypes)
		    {
			    if (HitTestWithResultType (point, resultType))
			    {
				    return;
			    }
		    }
	    }

	    public bool canDrag = false;

	    // Update is called once per frame
		void Update () 
		{
			if (!canDrag)
			{
				return;
			}
		}
	}
}