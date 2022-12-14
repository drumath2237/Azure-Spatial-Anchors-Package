using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

#if !UNITY_STANDALONE
using Microsoft.Azure.SpatialAnchors.Unity;
#endif

namespace AzureSpatialAnchorsPackage.Sample
{
    internal enum AnchorOperationStatus
    {
        UnderstandingEnvironment,
        AnchorCreated,
        FindingAnchor,
        AnchorFound,
        None
    }

    public class SpaceSharingDemo : MonoBehaviour
    {
#if !UNITY_STANDALONE
        [SerializeField] private SpatialAnchorManager spatialAnchorManager;

        [SerializeField] private CloudNativeAnchor creationAnchor;
        [SerializeField] private CloudNativeAnchor finderAnchor;

        private IAnchorService _anchorService;

        private AnchorCreator _anchorCreator;
        private AnchorFinder _anchorFinder;

        [SerializeField] private TextMeshProUGUI statusText;

        private AnchorOperationStatus _anchorOperationStatus;


        private void Start()
        {
            // InMemoryAnchorService()から変更
            _anchorService = new AzureServerlessAnchorService();
            _anchorCreator = new AnchorCreator(spatialAnchorManager, _anchorService);
            _anchorFinder = new AnchorFinder(spatialAnchorManager, _anchorService);
            _anchorOperationStatus = AnchorOperationStatus.None;
        }

        private int AnchorCreateReadyProgress()
            => (int)(spatialAnchorManager.SessionStatus.ReadyForCreateProgress * 100f);

        private void Update()
        {
            statusText.text = _anchorOperationStatus switch
            {
                AnchorOperationStatus.None => "None",
                AnchorOperationStatus.UnderstandingEnvironment => $"Understanding Env:{AnchorCreateReadyProgress()}%",
                AnchorOperationStatus.AnchorCreated => "Anchor Created",
                AnchorOperationStatus.FindingAnchor => "Finding Anchor",
                AnchorOperationStatus.AnchorFound => "Anchor Found",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public async void ProceedOperations()
        {
            _anchorOperationStatus = _anchorOperationStatus switch
            {
                AnchorOperationStatus.None or AnchorOperationStatus.AnchorFound
                    => await StartCreation(_anchorOperationStatus),

                AnchorOperationStatus.UnderstandingEnvironment
                    => await CreateAnchorAsync(_anchorOperationStatus),

                AnchorOperationStatus.AnchorCreated
                    => await StartFindingAnchor(_anchorOperationStatus),

                _ => _anchorOperationStatus
            };
        }

        private async Task<AnchorOperationStatus> StartCreation(AnchorOperationStatus status)
        {
            if (status != AnchorOperationStatus.None
                && status != AnchorOperationStatus.AnchorFound)
            {
                return status;
            }

            await _anchorCreator.StartSessionAsync();
            return AnchorOperationStatus.UnderstandingEnvironment;
        }

        private async Task<AnchorOperationStatus> CreateAnchorAsync(AnchorOperationStatus status)
        {
            if (status != AnchorOperationStatus.UnderstandingEnvironment || !spatialAnchorManager.IsReadyForCreate)
            {
                return status;
            }

            await _anchorCreator.CreateAnchorAsync(creationAnchor);
            _anchorCreator.DestroySession();
            return AnchorOperationStatus.AnchorCreated;
        }

        private async Task<AnchorOperationStatus> StartFindingAnchor(AnchorOperationStatus status)
        {
            if (status != AnchorOperationStatus.AnchorCreated)
            {
                return status;
            }

            _anchorFinder.OnAnchorFound += OnAnchorFound;
            await _anchorFinder.StartSessionAsync();
            await _anchorFinder.StartFindAnchorAsync(finderAnchor);
            return AnchorOperationStatus.FindingAnchor;
        }


        private void OnAnchorFound(CloudNativeAnchor nativeAnchor)
        {
            _anchorOperationStatus = AnchorOperationStatus.AnchorFound;
            _anchorFinder.DestroySession();
            _anchorFinder.OnAnchorFound -= OnAnchorFound;
        }
#endif
    }
}