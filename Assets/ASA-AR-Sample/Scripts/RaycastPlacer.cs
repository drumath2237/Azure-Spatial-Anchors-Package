using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class RaycastPlacer : MonoBehaviour
{
    [SerializeField] private ARRaycastManager arRaycastManager;

    [SerializeField] private Transform placeObjectTransform;

    private readonly List<ARRaycastHit> _hits = new();

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

        var isRaycastHit = arRaycastManager.Raycast(touchPosition, _hits);
        if (IsPointOverUIObject(touchPosition) || !isRaycastHit)
        {
            return;
        }

        placeObjectTransform.position = _hits[0].pose.position;
        placeObjectTransform.rotation = _hits[0].pose.rotation;
    }

    /// <summary>
    /// this code is quoted from
    /// https://github.com/Unity-Technologies/arfoundation-samples/issues/25
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

        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(pos.x, pos.y)
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        return results.Count > 0;
    }
}