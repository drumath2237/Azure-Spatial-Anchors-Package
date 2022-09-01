using System;
using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;

public class SpaceSharingDemo : MonoBehaviour
{
    [SerializeField] private SpatialAnchorManager spatialAnchorManager;

    [SerializeField] private CloudNativeAnchor creationAnchor;
    [SerializeField] private CloudNativeAnchor finderAnchor;

    private IAnchorService _anchorService;

    private AnchorCreator _anchorCreator;
    private AnchorFinder _anchorFinder;

    private void Start()
    {
        _anchorService = new InMemoryAnchorService();
        _anchorCreator = new AnchorCreator(spatialAnchorManager, _anchorService);
        _anchorFinder = new AnchorFinder(spatialAnchorManager, _anchorService);
    }

    public async void OnClickStartCreation()
    {
        await _anchorCreator.StartSessionAsync();
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