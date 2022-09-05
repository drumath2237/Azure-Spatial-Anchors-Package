using System;
using System.Threading.Tasks;
using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;

public class AnchorFinder
{
    private readonly SpatialAnchorManager _spatialAnchorManager;
    private readonly IAnchorService _anchorService;

    public event Action<CloudNativeAnchor> OnAnchorFound = null;

    private CloudNativeAnchor _nativeAnchor = null;

    public AnchorFinder(
        SpatialAnchorManager spatialAnchorManager, IAnchorService anchorService)
    {
        _spatialAnchorManager = spatialAnchorManager;
        _anchorService = anchorService;
    }

    public Task StartSessionAsync()
    {
        _spatialAnchorManager.AnchorLocated += UpdateNativeAnchor_OnAnchorFound;
        return _spatialAnchorManager.StartSessionAsync();
    }

    public void DestroySession()
    {
        _spatialAnchorManager.AnchorLocated -= UpdateNativeAnchor_OnAnchorFound;
        _spatialAnchorManager.DestroySession();
    }

    private void UpdateNativeAnchor_OnAnchorFound(
        object sender, AnchorLocatedEventArgs args)
    {
        if (args.Status != LocateAnchorStatus.Located)
        {
            return;
        }

        if (_nativeAnchor is null)
        {
            return;
        }

        _nativeAnchor.CloudToNative(args.Anchor);
        OnAnchorFound?.Invoke(_nativeAnchor);
    }

    public async Task StartFindAnchorAsync(CloudNativeAnchor nativeAnchor)
    {
        var anchorKey = await _anchorService.TryGetLatestAnchorAsync();
        if (!anchorKey.HasValue)
        {
            return;
        }

        _nativeAnchor = nativeAnchor;
        var anchorCriteria = new AnchorLocateCriteria
        {
            Identifiers = new[] { anchorKey.Value.anchorKey }
        };
        _spatialAnchorManager.Session.CreateWatcher(anchorCriteria);
    }
}