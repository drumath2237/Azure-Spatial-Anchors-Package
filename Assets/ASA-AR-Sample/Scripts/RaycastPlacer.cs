using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class RaycastPlacer : MonoBehaviour
{
    [SerializeField] private ARRaycastManager arRaycastManager;
    
    [SerializeField] private Transform placeObjectTransform;

    private List<ARRaycastHit> _hits = new();

    private void Update()
    {
        if (!placeObjectTransform)
        {
            return;
        }

        if (Input.touchCount == 0)
        {
            return;
        }
        var touchPosition = Input.GetTouch(0).position;

        if (!arRaycastManager.Raycast(touchPosition, _hits))
        {
            return;
        }

        placeObjectTransform.position = _hits[0].pose.position;
        placeObjectTransform.rotation = _hits[0].pose.rotation;

    }
}
