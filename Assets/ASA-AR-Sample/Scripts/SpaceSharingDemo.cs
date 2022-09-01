using System;
using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using TMPro;
using UnityEngine;

public class SpaceSharingDemo : MonoBehaviour
{
    [SerializeField] private SpatialAnchorManager spatialAnchorManager;

    [SerializeField] private CloudNativeAnchor creationAnchor;
    [SerializeField] private CloudNativeAnchor finderAnchor;

    private IAnchorService _anchorService;

    private AnchorCreator _anchorCreator;
    private AnchorFinder _anchorFinder;

    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private TextMeshProUGUI sessionReadyText;


    private void Start()
    {
        _anchorService = new InMemoryAnchorService();
        _anchorCreator = new AnchorCreator(spatialAnchorManager, _anchorService);
        _anchorFinder = new AnchorFinder(spatialAnchorManager, _anchorService);
    }

    private void Update()
    {
        sessionReadyText.text = _anchorCreator.IsReadyForCreateAnchor ? "ready for create" : "not ready";
    }

    public async void OnClickStartCreation()
    {
        try
        {
            await _anchorCreator.StartSessionAsync();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public async void OnClickCreateAnchor()
    {
        if (!_anchorCreator.IsReadyForCreateAnchor)
        {
            return;
        }

        await _anchorCreator.CreateAnchorAsync(creationAnchor);
        _anchorCreator.DestroySession();
    }

    public async void OnClickStartFinding()
    {
        _anchorFinder.OnAnchorFound += OnAnchorFound;
        await _anchorFinder.StartSessionAsync();
        await _anchorFinder.StartFindAnchorAsync(finderAnchor);
    }

    private void OnAnchorFound(CloudNativeAnchor nativeAnchor)
    {
        _anchorFinder.DestroySession();
    }
}