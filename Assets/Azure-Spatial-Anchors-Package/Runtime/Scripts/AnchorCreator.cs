#if !UNITY_STANDALONE
using System;
using System.Threading.Tasks;
using UnityEngine;

using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;


namespace AzureSpatialAnchorsPackage
{
    public class AnchorCreator
    {
        private readonly SpatialAnchorManager _spatialAnchorManager;
        private readonly IAnchorService _anchorService;

        public AnchorCreator(SpatialAnchorManager anchorManager, IAnchorService anchorService)
        {
            _spatialAnchorManager = anchorManager;
            _anchorService = anchorService;
        }

        public Task StartSessionAsync() => _spatialAnchorManager.StartSessionAsync();

        public void DestroySession() => _spatialAnchorManager.DestroySession();

        public bool IsReadyForCreateAnchor => _spatialAnchorManager.IsReadyForCreate;

        public async Task<CloudSpatialAnchor> CreateAnchorAsync(
            CloudNativeAnchor nativeAnchor,
            int anchorExpirationDays = 1
        )
        {
            if (!_spatialAnchorManager.IsReadyForCreate)
            {
                return null;
            }

            var anchorTransform = nativeAnchor.transform;
            nativeAnchor.SetPose(anchorTransform.position, anchorTransform.rotation);
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

                await _anchorService.CreateAnchorAsync(new AnchorInfo(
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
}
#endif