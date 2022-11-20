# Azure Spatial Anchors Package

## About

WIP

## Install via UPM

```txt
https://github.com/drumath2237/Azure-Spatial-Anchors-Package.git?path=Assets/Azure-Spatial-Anchors-Package
```

## Usage (anchor localization)

```cs
public class AnchorLocalizer : MonoBehaviour
{
    [SerializeField] private CloudNativeAnchor cloudNativeAnchor;

    [SerializeField] private SpatialAnchorManager spatialAnchorManager;

    private AnchorFinder _anchorFinder;

    private async void Start()
    {
        var anchorService = new AzureServerlessAnchorService();
        _anchorFinder = new AnchorFinder(spatialAnchorManager, anchorService);

        await _anchorFinder.StartSessionAsync();
        _anchorFinder.OnAnchorFound += OnAnchorFound;
        await _anchorFinder.StartFindAnchorAsync(cloudNativeAnchor);
    }

    private static void OnAnchorFound(CloudNativeAnchor anchor)
    {
        Debug.Log("anchor found!");
    }

    private void OnDestroy()
    {
        _anchorFinder.OnAnchorFound -= OnAnchorFound;
        _anchorFinder.DestroySession();
    }
}
```
