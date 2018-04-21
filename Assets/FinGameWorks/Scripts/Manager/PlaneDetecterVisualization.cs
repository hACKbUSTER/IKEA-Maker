using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.iOS;

namespace FinGameWorks.Scripts.Manager
{
    public class PlaneDetecterVisualization : Singleton<PlaneDetecterVisualization>
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

	    public void MakePlane()
	    {
		    PlaneDetecterVisualization.Instance.GetComponent<MeshRenderer>().material.SetColor("_V_WIRE_Color",new Color(1.0f,1.0f,1.0f,0.0f)); 
		    
		    
		    var screenPosition = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width/2,Screen.height/2));
		    Debug.Log(screenPosition);
		    ARPoint point = new ARPoint {
			    x = screenPosition.x,
			    y = screenPosition.y
		    };

		    // prioritize reults types
		    ARHitTestResultType[] resultTypes = {
			    //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
//			    ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
			    // if you want to use infinite planes use this:
			    //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
			    ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
			    //ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
			    //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
		    }; 
					
		    foreach (ARHitTestResultType resultType in resultTypes)
		    {
			    if (HitTestWithResultType (point, resultType))
			    {
				    PlaneDetecterVisualization.Instance.GetComponent<MeshRenderer>().material.DOColor(new Color(1.0f,1.0f,1.0f,1.0f),"_V_WIRE_Color",0.5f); 
				    return;
			    }
		    }
	    }

	    private bool CanUpdate = true;
		void Update () 
		{
			IEnumerable<ARPlaneAnchorGameObject> arpags = UnityARGeneratePlane.Instance.unityARAnchorManager.GetCurrentPlaneAnchors ();
			if (arpags.Count() > 0 && CanUpdate)
			{
				MakePlane();
				CanUpdate = false;
			}
		}
	}
}