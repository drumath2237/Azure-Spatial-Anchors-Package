using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

        if (IsPointOverUIObject(touchPosition) || !arRaycastManager.Raycast(touchPosition, _hits))
        {
            return;
        }

        placeObjectTransform.position = _hits[0].pose.position;
        placeObjectTransform.rotation = _hits[0].pose.rotation;
    }

    /// <summary>
    /// this code is quoted from
    /// https://github.com/Unity-Technologies/arfoundation-samples/issues/25#issuecomment-567860260
    /// thanks for digitalmkt.
    /// </summary>
    /// <param name="pos">screen position</param>
    /// <returns></returns>
    private static bool IsPointOverUIObject(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }
        
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(pos.x, pos.y)
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}