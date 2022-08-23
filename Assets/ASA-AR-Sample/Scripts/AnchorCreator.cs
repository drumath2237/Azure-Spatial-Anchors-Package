using System;
using System.Threading.Tasks;
using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;

public class AnchorCreator
{
    private readonly SpatialAnchorManager _spatialAnchorManager;
    private readonly IAnchorService _anchorService;

    public AnchorCreator(SpatialAnchorManager anchorManager, IAnchorService anchorService)
    {
        _spatialAnchorManager = anchorManager;
        _anchorService = anchorService;
    }

    public Task StartSession() => _spatialAnchorManager.StartSessionAsync();

    public async Task<CloudSpatialAnchor> CreateAnchor(CloudNativeAnchor nativeAnchor, int anchorExpirationDays = 1)
    {
        if (!_spatialAnchorManager.IsReadyForCreate)
        {
            return null;
        }

        await nativeAnchor.NativeToCloud(false);

        var cloudSpatialAnchor = nativeAnchor.CloudAnchor;
        cloudSpatialAnchor.Expiration = DateTimeOffset.Now.AddDays(anchorExpirationDays);

        try
        {
            await _spatialAnchorManager.CreateAnchorAsync(cloudSpatialAnchor);
            if (cloudSpatialAnchor.Identifier == string.Empty)
            {
                return null;
            }

            await _anchorService.CreateAnchor(new AnchorInfo(
                cloudSpatialAnchor.Identifier,
                cloudSpatialAnchor.Expiration.DateTime
            ));

            return cloudSpatialAnchor;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
}